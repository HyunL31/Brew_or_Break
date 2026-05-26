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
    private int _todayCluePoint = 0;
    private int _maxLevel = 10;
    private int _maxReputation = 500;
    private int _maxCompensation = 10000;

    private void Awake()
    {
        Inst = this;
    }

    public void StoreInit()
    {
        _storeModel = GameManager.Inst.GetStoreModel();
    }

    public void SetStoreLevel()
    {
        _storeModel.Level++;
    }

    public int GetStoreLevel()
    {
        return _storeModel.Level;
    }

    public int GetStoreReputation()
    {
        return _storeModel.Reputation;
    }

    public void SetReputation(int reputation)
    {
        _storeModel.Reputation += reputation;
    }

    public int GetStoreDebt()
    {
        return _storeModel.Compensation;
    }

    public void SetStoreDebt()
    {
        _storeModel.Compensation -= 100;
    }

    public void SetGold(int gold)
    {
        _storeModel.Gold += gold;
    }

    public int GetGold()
    {
        return _storeModel.Gold;
    }

    public void SetCluePoint(int point)
    {
        _todayCluePoint += point;
    }

    public void ResetCluePoint()
    {
        _todayCluePoint = 0;
    }

    public int GetCluePoint()
    {
        return _todayCluePoint;
    }

    public float CalculatStat(StatType type)
    {
        switch (type)
        {
            case StatType.Level:
                return (float)GetStoreLevel() / _maxLevel;

            case StatType.Reputation:
                return (float)GetStoreReputation() / _maxReputation;

            case StatType.Compensation:
                return 1f - ((float)GetStoreDebt() / _maxCompensation);

            default:
                return 0;
        }
    }
}