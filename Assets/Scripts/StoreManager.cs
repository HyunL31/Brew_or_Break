using System;
using UnityEngine;

public enum StatType
{
    None,
    Level,
    Reputation,
    Compensation
}

/// <summary>
/// 가게 정보 매니저
/// </summary>

public class StoreManager : MonoBehaviour
{
    public static StoreManager Inst;

    public StoreModel StoreModel { get; private set; } = new StoreModel();
    private int _todayCluePoint = 0;
    private int _maxLevel = 5;
    private int _maxReputation = 1000;
    private int _maxCompensation = 6000;

    public Action OnResetPoint;

    private void Awake()
    {
        Inst = this;

        OnResetPoint = ResetCluePoint;
    }

    public void StoreInit()
    {
        StoreModel = GameManager.Inst.PlayerModel.Store;
    }

    public void SetStoreLevel()
    {
        if (StoreModel.Level >= _maxLevel)
        {
            UIManager.Inst.OpenConfirmPopup("최대 레벨입니다.");
        }

        StoreModel.Level++;
    }

    public void SetReputation(int reputation)
    {
        StoreModel.Reputation += reputation;
    }


    public void SetStoreDebt()
    {
        if (StoreModel.Compensation <= 0)
        {
            UIManager.Inst.OpenConfirmPopup("모든 변상금을 상환했습니다.");
        }

        StoreModel.Compensation -= 100;
    }

    public void SetGold(int gold)
    {
        StoreModel.Gold += gold;
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

    // 상태바 값 계산
    public float CalculatStat(StatType type, StoreModel targetModel)
    {
        switch (type)
        {
            case StatType.Level:
                return (float)targetModel.Level / _maxLevel;

            case StatType.Reputation:
                return (float)targetModel.Reputation / _maxReputation;

            case StatType.Compensation:
                return 1f - ((float)targetModel.Compensation / _maxCompensation);

            default:
                return 0;
        }
    }
}