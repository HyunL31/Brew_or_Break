using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NamePopup : UIBase
{
    [Header("이름 입력")]
    [SerializeField] private TMP_InputField PlayerName;
    [SerializeField] private TMP_InputField StoreName;
    [SerializeField] private Button Button_Confirm;
    [SerializeField] private Button Button_Close;

    [Header("경고 팝업")]
    [SerializeField] private GameObject AlertPopup;
    [SerializeField] private Button Button_Alert;

    private void Awake()
    {
        Button_Confirm.onClick.AddListener(OnClickConfirm);
        Button_Alert.onClick.AddListener(OnClickAlert);
        Button_Close.onClick.AddListener(UIManager.Inst.CloseNamePopup);
    }

    private void OnClickConfirm()
    {
        if ((PlayerName.text == string.Empty || StoreName.text == string.Empty) || PlayerName.text.Length > 10 || StoreName.text.Length > 16)
        {
            AlertPopup.SetActive(true);
            return;
        }

        GameManager.Inst.SetName(PlayerName.text, StoreName.text);

        VisualNovelManager.Inst.SetCurrentDialogueID();
        UIManager.Inst.OpenVisualNovelUI();
        UIManager.Inst.OpenDialogueUI();
        UIManager.Inst.CloseNamePopup();
        UIManager.Inst.CloseTitleUI();
    }

    private void OnClickAlert()
    {
        AlertPopup.SetActive(false);
    }
}
