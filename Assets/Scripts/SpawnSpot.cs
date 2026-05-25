using Cysharp.Threading.Tasks;
using UnityEngine;

public enum SpawnType
{
    None,
    DropItem,
    Dialogue,
    Monster
}

public class SpawnSpot : MonoBehaviour
{
    [SerializeField] private SpawnType SpawnType;
    [SerializeField] private string SpawnID;
    [SerializeField] private Collider2D SpawnCollider;

    private void Awake()
    {
        SpawnObject();
    }

    private void SpawnObject()
    {
        switch(SpawnType)
        {
            case SpawnType.Monster:
                CollectingManager.Inst.CreateMonsterObject(GetRandomMonsterID(), this.transform).Forget();
                break;
        }
    }

    private string GetRandomMonsterID()
    {
        int random = Random.Range(1, 7);

        return $"Monster_0{random}";
    }
}
