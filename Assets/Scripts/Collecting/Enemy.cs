using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int _instanceID;
    private string _monsterID;

    public void InitMonster(int instanceID, string monsterID)
    {
        _instanceID = instanceID;
        _monsterID = monsterID;
    }
}
