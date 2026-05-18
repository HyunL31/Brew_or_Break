using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [Header("플레이어 정보")]
    [SerializeField] private TextMeshProUGUI PlayerName;
    [SerializeField] private TextMeshProUGUI StoreName;

    [Header("가게 정보")]
    [SerializeField] private TextMeshProUGUI GoldText;
    [SerializeField] private TextMeshProUGUI ReputationText;
    [SerializeField] private Image LevelStat;
    [SerializeField] private TextMeshProUGUI LevelStatText;
    [SerializeField] private Image ReputationStat;
    [SerializeField] private TextMeshProUGUI ReputationStatText;
    [SerializeField] private Image DebtStat;
    [SerializeField] private TextMeshProUGUI DebtStatText;

    [Header("버튼")]
    [SerializeField] private Button InventoryButton;
    [SerializeField] private Button OpenStoreButton;

    [Header("패널")]
    [SerializeField] private GameObject DialogueUI;

    private void Awake()
    {
        InventoryButton.onClick.AddListener(OnClickInventory);
        OpenStoreButton.onClick.AddListener(OnClickOpenStore);
    }

    private void OnClickInventory()
    {
        // [TODO] 인벤토리 구현
    }

    private void OnClickOpenStore()
    {
        DialogueUI.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
