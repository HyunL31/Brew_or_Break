using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [Header("플레이어 정보")]
    [SerializeField] private TextMeshProUGUI Text_PlayerName;
    [SerializeField] private TextMeshProUGUI Text_StoreName;

    [Header("가게 정보")]
    [SerializeField] private TextMeshProUGUI Text_Gold;
    [SerializeField] private TextMeshProUGUI Text_Reputation;
    [SerializeField] private Image Image_LevelStat;
    [SerializeField] private TextMeshProUGUI TextLevelStat;
    [SerializeField] private Image Image_ReputationStat;
    [SerializeField] private TextMeshProUGUI Text_ReputationStat;
    [SerializeField] private Image Image_DebtStat;
    [SerializeField] private TextMeshProUGUI Text_DebtStat;
    [SerializeField] private TextMeshProUGUI Text_Day;

    [Header("버튼")]
    [SerializeField] private Button Button_Inventory;
    [SerializeField] private Button Button_OpenStore;

    [Header("패널")]
    [SerializeField] private GameObject DialogueUI;

    private void Awake()
    {
        Button_Inventory.onClick.AddListener(OnClickInventory);
        Button_OpenStore.onClick.AddListener(OnClickOpenStore);
    }

    private void OnEnable()
    {
        Text_Day.text = $"Day {GameManager.Inst.GetDay()}";
    }

    private void OnClickInventory()
    {
        // [TODO] 인벤토리 구현
    }

    private void OnClickOpenStore()
    {
        GameManager.Inst.SetCurrentDialogueID();
        DialogueUI.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
