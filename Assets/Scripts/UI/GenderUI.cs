using UnityEngine;
using UnityEngine.UI;

public class GenderUI : UIBase
{
    [SerializeField] private Button Button_Close;
    [SerializeField] private Button Button_Girl;
    [SerializeField] private Button Button_Boy;

    private void Awake()
    {
        Button_Close.onClick.AddListener(OnClickClose);
        Button_Girl.onClick.AddListener(OnClickGirl);
        Button_Boy.onClick.AddListener(OnClickBoy);
    }

    private void OnClickClose()
    {
        UIManager.Inst.CloseGenderUI();
    }

    private void OnClickGirl()
    {
        GameManager.Inst.PlayerModel.Gender = "Girl";
        SetDialogue();
    }

    private void OnClickBoy()
    {
        GameManager.Inst.PlayerModel.Gender = "Boy";
        SetDialogue();
    }

    private void SetDialogue()
    {
        UIManager.Inst.OpenNamePopup();
    }
}
