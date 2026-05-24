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
            AddMonsterObject(prefab, monsterID);
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

    private void AddMonsterObject(GameObject monster, string monsterID)
    {
        int instanceID = _generatedKey;
        _generatedKey++;

        Enemy enemy = monster.GetComponent<Enemy>();

        _monsters.Add(instanceID, enemy);

        enemy.InitMonster(instanceID, monsterID);
    }
}
