using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 메인 로비 UI
/// </summary>

public class LobbyUI : UIBase
{
    [Header("플레이어 정보")]
    [SerializeField] private Image Image_Player;
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
    [SerializeField] private Button Button_Return;
    [SerializeField] private Button Button_Setting;

    private void Awake()
    {
        Button_Inventory.onClick.AddListener(UIManager.Inst.OpenInventory);
        Button_OpenStore.onClick.AddListener(OnClickOpenStore);
        Button_Return.onClick.AddListener(OnClickReturn);
        Button_Setting.onClick.AddListener(OnClickSetting);
    }

    private void OnEnable()
    {
        SoundManager.Inst.OnBGM?.Invoke("Audio/Base");

        SetPlayerInfo();
        SetStoreInfo();

        SetStatus();
    }

    private void SetPlayerInfo()
    {
        string path = "Character/Girl/Player_01_04";

        if (GameManager.Inst.PlayerModel.Gender == "Boy")
        {
            path = "Character/Boy/Player_01_04";
        }

        GameUtil.LoadSpriteAndSet(path, Image_Player).Forget();

        Text_Day.text = $"Day {GameManager.Inst.PlayerModel.Day}";
        Text_PlayerName.text = GameManager.Inst.PlayerModel.PlayerName;
        Text_StoreName.text = GameManager.Inst.PlayerModel.StoreName;
    }

    private void SetStoreInfo()
    {
        Text_Gold.text = $"{StoreManager.Inst.StoreModel.Gold} G";
        Text_Reputation.text = $"{StoreManager.Inst.StoreModel.Reputation}REP";
    }

    private void SetStatus()
    {
        Text_LevelStat.text = $"가게 레벨 : Lv.{StoreManager.Inst.StoreModel.Level}";
        Text_ReputationStat.text = $"가게 명성 : {StoreManager.Inst.StoreModel.Reputation}";
        Text_DebtStat.text = $"배상금 : {StoreManager.Inst.StoreModel.Gold}";

        Image_LevelStat.fillAmount = StoreManager.Inst.CalculatStat(StatType.Level, StoreManager.Inst.StoreModel);
        Image_ReputationStat.fillAmount = StoreManager.Inst.CalculatStat(StatType.Reputation, StoreManager.Inst.StoreModel);
        Image_DebtStat.fillAmount = StoreManager.Inst.CalculatStat(StatType.Compensation, StoreManager.Inst.StoreModel);
    }

    private void OnClickReturn()
    {
        UIManager.Inst.InitStart();
        UIManager.Inst.CloseLobbyUI();
    }

    private void OnClickOpenStore()
    {
        SoundManager.Inst.SetSFXAndPlay("Audio/OpenStore").Forget();
        VisualNovelManager.Inst.SetCurrentDialogueID();

        GameManager.Inst.IsOpenStore(true);
        UIManager.Inst.OpenLoadingUI();
        UIManager.Inst.CloseLobbyUI();
    }

    private void OnClickSetting()
    {
        UIManager.Inst.OpenSettingPopup();
    }
}
