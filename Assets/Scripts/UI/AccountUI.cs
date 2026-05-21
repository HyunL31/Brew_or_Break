using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AccountUI : UIBase
{
    [Header("버튼")]
    [SerializeField] private Button Button_Confirm;
    [SerializeField] private Button Button_Level;
    [SerializeField] private Button Button_Compensation;

    [Header("스탯")]
    [SerializeField] private TextMeshProUGUI Text_Gold;
    [SerializeField] private TextMeshProUGUI Text_Reputation;
    [SerializeField] private TextMeshProUGUI Text_Level;
    [SerializeField] private Image Image_Level;
    [SerializeField] private TextMeshProUGUI Text_Compensation;
    [SerializeField] private Image Image_Compensation;

    private void Awake()
    {
        Button_Confirm.onClick.AddListener(OnClickConfirm);
    }

    private void OnClickConfirm()
    {
        GameManager.Inst.SetDay();
        GameManager.Inst.SaveData();

        UIManager.Inst.OpenLobbyUI();
        UIManager.Inst.CloseAccountUI();
    }
}
