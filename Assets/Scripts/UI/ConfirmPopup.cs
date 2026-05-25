using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmPopup : UIBase
{
    [SerializeField] Button Button_Confirm;
    [SerializeField] Button Button_Close;
    [SerializeField] TextMeshProUGUI Text_Info;

    private void Awake()
    {
        Button_Confirm.onClick.AddListener(OnClickConfirm);
        Button_Close.onClick.AddListener(OnClickClose);
    }

    public void SetText(string text)
    {
        Text_Info.text = text;
    }

    private void OnClickConfirm()
    {
        UIManager.Inst.CloseConfirmPopup();
    }

    private void OnClickClose()
    {
        UIManager.Inst.CloseConfirmPopup();
    }
}
