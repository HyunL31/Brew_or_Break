using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AccountUI : UIBase
{
    [Header("버튼")]
    [SerializeField] private TextMeshProUGUI Text_AccountDay;
    [SerializeField] private Button Button_Confirm;
    [SerializeField] private Button Button_Level;
    [SerializeField] private TextMeshProUGUI Text_RequestGold;
    [SerializeField] private Button Button_Compensation;
    [SerializeField] private TextMeshProUGUI Text_RequestCompen;

    [Header("스탯")]
    [SerializeField] private TextMeshProUGUI Text_Gold;
    [SerializeField] private TextMeshProUGUI Text_Reputation;
    [SerializeField] private TextMeshProUGUI Text_Level;
    [SerializeField] private Image Image_Level;
    [SerializeField] private TextMeshProUGUI Text_Compensation;
    [SerializeField] private Image Image_Compensation;

    private int _requestLevel = 5;
    private int _requestCompen = 100;

    private void Awake()
    {
        Button_Confirm.onClick.AddListener(OnClickConfirm);
        Button_Level.onClick.AddListener(OnClickLevel);
        Button_Compensation.onClick.AddListener(OnClickCompensation);
    }

    private void OnEnable()
    {
        Text_AccountDay.text = $"마감 정산 (Day {GameManager.Inst.GetDay()})";

        SetState();
    }

    private void SetState()
    {
        Text_Gold.text = $"{StoreManager.Inst.GetGold()} G";
        Text_Reputation.text = $"{StoreManager.Inst.GetStoreReputation()} REP";

        Text_Level.text = $"가게 레벨 : Lv.{StoreManager.Inst.GetStoreLevel()}";
        Image_Level.fillAmount = StoreManager.Inst.CalculatStat(StatType.Level);

        Text_Compensation.text = $"배상금 : {StoreManager.Inst.GetStoreDebt()}G";
        Image_Compensation.fillAmount = StoreManager.Inst.CalculatStat(StatType.Compensation);

        Text_RequestGold.text = $"다음 레벨까지 {_requestLevel} G";
        Text_RequestCompen.text = $"청산까지 {StoreManager.Inst.GetStoreDebt()} G";
    }

    private void OnClickConfirm()
    {
        GameManager.Inst.SaveData();

        CollectingManager.Inst.SetCollectingMap();
        UIManager.Inst.OpenHUD();
        UIManager.Inst.CloseAccountUI();
    }

    private void OnClickLevel()
    {
        if (StoreManager.Inst.GetGold() < _requestLevel)
        {
            return;
        }

        StoreManager.Inst.SetStoreLevel();
        StoreManager.Inst.SetGold(_requestLevel * -1);

        _requestLevel *= 2;

        SetState();
    }

    private void OnClickCompensation()
    {
        if (StoreManager.Inst.GetGold() < _requestCompen)
        {
            return;
        }

        StoreManager.Inst.SetStoreDebt();
        StoreManager.Inst.SetGold(_requestCompen * -1);

        SetState();
    }
}
