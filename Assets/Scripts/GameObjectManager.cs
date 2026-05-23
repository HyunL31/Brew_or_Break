using System.Collections.Generic;
using UnityEngine;

public class GameObjectManager : MonoBehaviour
{
    public static GameObjectManager Inst;

    private int _generatedKey = 0;
    private Dictionary<int, Enemy> _monsters = new Dictionary<int, Enemy>();

    private void Awake()
    {
        Inst = this;
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

    private void AddMonsterObject(GameObject monster, string monsterID)
    {
        int instanceID = _generatedKey;
        _generatedKey++;

        Enemy enemy = monster.GetComponent<Enemy>();

        _monsters.Add(instanceID, enemy);

        enemy.InitMonster(instanceID, monsterID);
    }
}
