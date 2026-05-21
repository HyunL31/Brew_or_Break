using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : UIBase
{
    [Header("플레이어 정보")]
    [SerializeField] private TextMeshProUGUI Text_PlayerName;
    [SerializeField] private TextMeshProUGUI Text_StoreName;

    [Header("가게 정보")]
    [SerializeField] private TextMeshProUGUI Text_Gold;
    [SerializeField] private TextMeshProUGUI Text_Reputation;
    [SerializeField] private Image Image_LevelStat;
    [SerializeField] private TextMeshProUGUI Text_LevelStat;
    [SerializeField] private Image Image_ReputationStat;
    [SerializeField] private TextMeshProUGUI Text_ReputationStat;
    [SerializeField] private Image Image_DebtStat;
    [SerializeField] private TextMeshProUGUI Text_DebtStat;
    [SerializeField] private TextMeshProUGUI Text_Day;

    [Header("버튼")]
    [SerializeField] private Button Button_Inventory;
    [SerializeField] private Button Button_OpenStore;

    private void Awake()
    {
        Button_Inventory.onClick.AddListener(UIManager.Inst.OpenInventory);
        Button_OpenStore.onClick.AddListener(OnClickOpenStore);
    }

    private void OnEnable()
    {
        SetPlayerInfo();

        SetStatus();
    }

    private void SetPlayerInfo()
    {
        Text_Day.text = $"Day {GameManager.Inst.GetDay()}";

        Text_PlayerName.text = GameManager.Inst.GetPlayerName();
        Text_StoreName.text = GameManager.Inst.GetStoreName();
    }

    private void SetStatus()
    {
        Text_LevelStat.text = $"가게 레벨 : Lv.{GameManager.Inst.GetStoreLevel()}";
        Text_ReputationStat.text = $"가게 명성 : {GameManager.Inst.GetStoreReputation()}";
        Text_DebtStat.text = $"갚은 빚 : {GameManager.Inst.GetStoreDebt()}";

        Image_LevelStat.fillAmount = GameManager.Inst.GetStoreLevel() / MaxStoreStatus.MaxLevel;
        Image_ReputationStat.fillAmount = GameManager.Inst.GetStoreReputation() / MaxStoreStatus.MaxReputation;
        Image_DebtStat.fillAmount = GameManager.Inst.GetStoreDebt() / MaxStoreStatus.MaxCompensation;
    }

    private void OnClickOpenStore()
    {
        VisualNovelManager.Inst.SetCurrentDialogueID();
        UIManager.Inst.OpenVisualNovelUI();
        UIManager.Inst.OpenDialogueUI();
        UIManager.Inst.CloseLobbyUI();
    }
}
