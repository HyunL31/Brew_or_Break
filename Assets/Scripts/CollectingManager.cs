using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 사냥 콘텐츠 매니저
/// </summary>

public class CollectingManager : MonoBehaviour
{
    public static CollectingManager Inst;

    private Camera _main;
    private CameraMoving _camera;
    private PlayerMoving _player;
    private PlayerBubble _playerBubble;
    private GameObject _map;
    private int _generatedKey = 0;
    private Dictionary<int, Enemy> _monsters = new Dictionary<int, Enemy>();
    private Dictionary<int, DropItem> _dropItems = new Dictionary<int, DropItem>();
    private List<HPBar> _hpBars = new List<HPBar>();

    public Action<int, int> OnSkillCollision;
    public Action<float, float> OnChangeStamina;
    public Action OnLackStamina;
    public Action OnEndCollecting;
    public Action OnStartCollecting;
    public Action<bool> OnEnterItem;

    private void Awake()
    {
        Inst = this;

        OnEndCollecting += ClearHPBar;
        OnEndCollecting += DestroyPlayer;

        OnStartCollecting += () => { SetCollectingMap().Forget(); };
        _main = Camera.main;
    }

    public void SetCamera(CameraMoving camera)
    {
        _camera = camera;
    }

    public PlayerMoving GetPlayer()
    {
        return _player;
    }

    private void SetCameraTarget(GameObject player)
    {
        _camera.SetTarget(player);
    }

    public Enemy GetMonster(int id)
    {
        return _monsters[id];
    }

    // 플레이어 및 맵 초기화
    public async UniTask SetCollectingMap()
    {
        UIManager.Inst.CloseBackgroundUI();

        string mapPath = "Prefabs/Collecting/Map";
        string playerPath = "Prefabs/Collecting/Girl/Player";

        if (GameManager.Inst.PlayerModel.Gender == "Boy")
        {
            playerPath = "Prefabs/Collecting/Boy/Player";
        }

        GameObject player = await ResourceManager.Inst.InstantiatePrefab(playerPath, null);

        player.transform.position = Vector3.zero;
        SetCameraTarget(player);

        if (_camera != null)
        {
            SetCameraTarget(player);
        }

        PlayerMoving playerMoving = player.GetComponent<PlayerMoving>();
        _player = playerMoving;
        HPBar hpBar = await CreateHPBar(player.transform);
        _player.SetHPBar(hpBar);
        _hpBars.Add(hpBar);

        GameObject map = await ResourceManager.Inst.InstantiatePrefab(mapPath, null);

        map.transform.position = Vector3.zero;
        _map = map;
    }

    public async UniTask CreateMonsterObject(string monsterID, Transform spawnSpot)
    {
        var monsterData = GameDataManager.Inst.GetMonsterData(monsterID);

        string path = $"Prefabs/{monsterID}";

        GameObject prefab = await ResourceManager.Inst.InstantiatePrefab(path, spawnSpot);
        Enemy enemy = AddMonsterObject(prefab, monsterID, spawnSpot);

        HPBar hpBar = await CreateHPBar(prefab.transform);
        enemy.SetHPBar(hpBar);
        _hpBars.Add(hpBar);
    }

    // 맵 정리
    public void DestroyPlayer()
    {
        StoreManager.Inst.OnResetPoint?.Invoke();

        Destroy(_player.gameObject);
        Destroy(_map);

        _player = null;
        _map = null;

        GameManager.Inst.IsOpenStore(false);

        UIManager.Inst.OpenLoadingUI();
        UIManager.Inst.OpenBackgroundUI();
    }

    private Enemy AddMonsterObject(GameObject monster, string monsterID, Transform spawnSpot)
    {
        int instanceID = _generatedKey;
        _generatedKey++;

        Enemy enemy = monster.GetComponent<Enemy>();

        _monsters.Add(instanceID, enemy);

        enemy.InitMonster(instanceID, monsterID);
        enemy.SetParent(spawnSpot);

        return enemy;
    }

    // 드랍 아이템 설정
    public async UniTask DropMonsterItem(List<string> items, Transform parent, Transform dropPos)
    {
        string path = "Prefabs/DropItem";

        foreach(string item in items)
        {
            GameObject prefab = await ResourceManager.Inst.InstantiatePrefab(path, parent);

            float randomX = UnityEngine.Random.Range(0, 0.8f);
            float randomY = UnityEngine.Random.Range(0, 0.8f);
            prefab.transform.position = new Vector2(dropPos.position.x + randomX, dropPos.position.y + randomY);

            DropItem dropItem = prefab.GetComponent<DropItem>();
            dropItem.SetItemID(item);
            dropItem.SetImage().Forget();

            _dropItems[dropItem.GetInstancedID()] = dropItem;
        }
    }

    public DropItem GetTargetItem(int id)
    {
        return _dropItems[id];
    }

    public void CollectItem(DropItem dropItem)
    {
        SoundManager.Inst.OnSFX?.Invoke("Audio/Item");
        GameManager.Inst.AddItem(dropItem.GetItemID());
        Destroy(dropItem.gameObject);
    }

    // HP 바 설정
    public async UniTask<HPBar> CreateHPBar(Transform target)
    {
        string path = "Prefabs/UI/HPBar";

        GameObject prefab = await ResourceManager.Inst.InstantiatePrefab(path, UIManager.Inst.GetUIRootTransform(UIRootType.Background));
        HPBar hpBar = prefab.GetComponent<HPBar>();
        hpBar.SetTarget(target);

        return hpBar;
    }

    public void ClearHPBar()
    {
        foreach(HPBar hpBar in _hpBars)
        {
            if (hpBar.gameObject != null)
            {
                Destroy(hpBar.gameObject);
            }
        }

        _hpBars.Clear();
    }

    // 플레이어 말풍선
    public async UniTask OpenPlayerBubble(string text)
    {
        if (_player == null || _player.gameObject == null)
        {
            return;
        }

        if (_playerBubble == null)
        {
            string path = "Prefabs/UI/PlayerBubble";
            GameObject prefab = await ResourceManager.Inst.InstantiatePrefab(path, UIManager.Inst.GetUIRootTransform(UIRootType.Background));
            PlayerBubble playerBubble = prefab.GetComponent<PlayerBubble>();
            _playerBubble = playerBubble;
        }
        else
        {
            _playerBubble.gameObject.SetActive(true);
        }

        if (_player.IsAlive())
        {
            _playerBubble.SetBubbleText(text, _player.gameObject.transform);
        }

        await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: _playerBubble.GetCancellationTokenOnDestroy());

        _playerBubble.gameObject.SetActive(false);
    }

    public void ClosePlayerBubble()
    {
        _playerBubble.gameObject.SetActive(false);
    }

    public bool IsOpenPlayerBubble()
    {
        if (_playerBubble == null)
        {
            return false;
        }

        if (_playerBubble.gameObject.activeSelf)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ClearPlayerBubble()
    {
        if (_playerBubble == null)
        {
            return;
        }

        Destroy(_playerBubble.gameObject);
        _playerBubble = null;
    }

    public void SetHUDPos(Transform playerPos, RectTransform rect, float yOffset)
    {
        if (playerPos == null || _main == null)
        {
            return;
        }

        Vector3 targetPos = playerPos.position + new Vector3(0, yOffset, 0);
        Vector3 screenPos = _main.WorldToScreenPoint(targetPos);

        rect.transform.position = screenPos;
    }
}
