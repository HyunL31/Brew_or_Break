using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 마감 정산 UI
/// </summary>

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

    private int _requestLevel = 10;
    private int _requestCompen = 100;

    private void Awake()
    {
        Button_Confirm.onClick.AddListener(OnClickConfirm);
        Button_Level.onClick.AddListener(OnClickLevel);
        Button_Compensation.onClick.AddListener(OnClickCompensation);
    }

    private void OnEnable()
    {
        Text_AccountDay.text = $"마감 정산 (Day {GameManager.Inst.PlayerModel.Day})";

        SetState();
    }

    private void SetState()
    {
        Text_Gold.text = $"{StoreManager.Inst.StoreModel.Gold} G";
        Text_Reputation.text = $"{StoreManager.Inst.StoreModel.Reputation} REP";

        Text_Level.text = $"가게 레벨 : Lv.{StoreManager.Inst.StoreModel.Level}";
        Image_Level.fillAmount = StoreManager.Inst.CalculatStat(StatType.Level, StoreManager.Inst.StoreModel);

        Text_Compensation.text = $"배상금 : {StoreManager.Inst.StoreModel.Compensation}G";
        Image_Compensation.fillAmount = StoreManager.Inst.CalculatStat(StatType.Compensation, StoreManager.Inst.StoreModel);

        if (StoreManager.Inst.CheckMaxLevel())
        {
            Text_RequestGold.text = "최대 레벨 도달";
        }
        else
        {
            Text_RequestGold.text = $"다음 레벨까지 {_requestLevel} G";
        }

        if (StoreManager.Inst.CheckCompensation())
        {
            Text_RequestCompen.text = "청산 완료";
        }
        else
        {
            Text_RequestCompen.text = $"청산까지 {StoreManager.Inst.StoreModel.Compensation} G";
        }
    }

    private void OnClickConfirm()
    {
        SoundManager.Inst.OnSFX?.Invoke("Audio/Account");

        if (GameManager.Inst.PlayerModel.Day == 10)
        {
            VisualNovelManager.Inst.CheckEnding();
            UIManager.Inst.OpenVisualNovelUI();
            UIManager.Inst.OpenDialogueUI();
            UIManager.Inst.CloseAccountUI();

            return;
        }

        CollectingManager.Inst.OnStartCollecting?.Invoke();
        UIManager.Inst.OpenHUD();
        UIManager.Inst.CloseAccountUI();
    }

    private void OnClickLevel()
    {
        if (StoreManager.Inst.CheckMaxLevel())
        {
            UIManager.Inst.OpenConfirmPopup("최대 레벨에 도달했습니다.");
            return;
        }

        if (StoreManager.Inst.StoreModel.Gold < _requestLevel)
        {
            UIManager.Inst.OpenConfirmPopup($"가진 금화가 적습니다.\n{_requestLevel}의 금화가 필요합니다.");
            return;
        }

        StoreManager.Inst.SetGold(_requestLevel * -1);
        StoreManager.Inst.SetStoreLevel();

        _requestLevel *= 2;

        SetState();
    }

    private void OnClickCompensation()
    {
        if (StoreManager.Inst.CheckCompensation())
        {
            UIManager.Inst.OpenConfirmPopup("모든 변상금을 상환했습니다.");
            return;
        }

        if (StoreManager.Inst.StoreModel.Gold < _requestCompen)
        {
            UIManager.Inst.OpenConfirmPopup($"가진 금화가 적습니다.\n{_requestCompen}의 금화가 필요합니다.");
            return;
        }


        StoreManager.Inst.SetGold(_requestCompen * -1);
        StoreManager.Inst.SetStoreDebt();

        SetState();
    }
}
