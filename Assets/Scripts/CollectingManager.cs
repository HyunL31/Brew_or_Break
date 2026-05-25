using System;
using System.Collections.Generic;
using UnityEngine;

public class CollectingManager : MonoBehaviour
{
    public static CollectingManager Inst;

    private CameraMoving _camera;
    private PlayerMoving _player;
    private GameObject _map;
    private int _generatedKey = 0;
    private Dictionary<int, Enemy> _monsters = new Dictionary<int, Enemy>();
    private Dictionary<int, DropItem> _dropItems = new Dictionary<int, DropItem>();

    public Action<int, int> OnSkillCollision;

    private void Awake()
    {
        Inst = this;
    }

    public void SetCamera(CameraMoving camera)
    {
        _camera = camera;
    }

    public PlayerMoving GetPlayer()
    {
        return _player;
    }

    public int SetSkillATK(string skillID)
    {
        int atk = GameDataManager.Inst.GetSkillData(skillID).ATK;

        return atk;
    }

    public void SetCollectingMap()
    {
        string playerPath = "Prefabs/Collecting/Player";
        string mapPath = "Prefabs/Collecting/Map";

        ResourceManager.Inst.InstantiatePrefab(playerPath, null, (player) =>
        {
            player.transform.position = Vector3.zero;
            SetCameraTarget(player);

            PlayerMoving playerMoving = player.GetComponent<PlayerMoving>();
            _player = playerMoving;
        });

        ResourceManager.Inst.InstantiatePrefab(mapPath, null, (map) =>
        {
            map.transform.position = Vector3.zero;
            _map = map;
        });
    }

    public void DestroyPlayer()
    {
        Destroy(_player.gameObject);
        Destroy(_map);

        _player = null;
        _map = null;
    }

    private void SetCameraTarget(GameObject player)
    {
        _camera.SetTarget(player);
    }

    public void CreateMonsterObject(string monsterID, Transform spawnSpot)
    {
        var monsterData = GameDataManager.Inst.GetMonsterData(monsterID);

        string path = $"Prefabs/{monsterID}";
        ResourceManager.Inst.InstantiatePrefab(path, spawnSpot, (prefab) =>
        {
            AddMonsterObject(prefab, monsterID, spawnSpot);
        });
    }

    public Enemy GetMonster(int id)
    {
        return _monsters[id];
    }

    public void DestroyMonster(GameObject monster)
    {
        Destroy(monster);
    }

    private void AddMonsterObject(GameObject monster, string monsterID, Transform spawnSpot)
    {
        int instanceID = _generatedKey;
        _generatedKey++;

        Enemy enemy = monster.GetComponent<Enemy>();

        _monsters.Add(instanceID, enemy);

        enemy.InitMonster(instanceID, monsterID);
        enemy.SetParent(spawnSpot);
    }

    public void DropMonsterItem(List<string> items, Transform parent)
    {
        string path = "Prefabs/DropItem";

        foreach(string item in items)
        {
            ResourceManager.Inst.InstantiatePrefab(path, parent, (prefab) =>
            {
                float randomX = UnityEngine.Random.Range(0, 1.5f);
                float randomY = UnityEngine.Random.Range(0, 1.5f);
                prefab.transform.position = new Vector2(parent.position.x + randomX, parent.position.y + randomY);

                DropItem dropItem = prefab.GetComponent<DropItem>();
                dropItem.SetItemID(item);
                dropItem.SetImage();

                _dropItems[dropItem.GetInstancedID()] = dropItem;
            });
        }
    }

    public DropItem GetTargetItem(int id)
    {
        return _dropItems[id];
    }

    public void CollectItem(DropItem dropItem)
    {
        GameManager.Inst.AddItem(dropItem.GetItemID());
        Destroy(dropItem.gameObject);
    }
}
