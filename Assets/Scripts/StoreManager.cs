using UnityEngine;

public enum StatType
{
    None,
    Level,
    Reputation,
    Compensation
}

public class StoreManager : MonoBehaviour
{
    public static StoreManager Inst;

    private StoreModel _storeModel = new StoreModel();
    private int _maxLevel = 10;
    private int _maxReputation = 500;
    private int _maxCompensation = 10000;

    private void Awake()
    {
        Inst = this;
    }

    private void Start()
    {
        _storeModel = GameManager.Inst.GetStoreModel();
    }

    public int GetStoreLevel()
    {
        return _storeModel.Level;
    }

    public int GetStoreReputation()
    {
        return _storeModel.Reputation;
    }

    public int GetStoreDebt()
    {
        return _storeModel.Compensation;
    }

    public int CalculatStat(StatType type)
    {
        switch (type)
        {
            case StatType.Level:
                return GetStoreLevel() / _maxLevel;

            case StatType.Reputation:
                return GetStoreReputation() / _maxReputation;

            case StatType.Compensation:
                return _maxCompensation / GetStoreDebt();

            default:
                return 0;
        }
    }
}