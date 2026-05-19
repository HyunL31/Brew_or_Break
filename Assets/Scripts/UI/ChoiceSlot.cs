using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Text_Choice;
    [SerializeField] private Button Button_Choice;

    private string _choiceID;

    private void Awake()
    {
        Button_Choice.onClick.AddListener(OnClickChoice);
    }

    public void SetChoiceText(string choice)
    {
        Text_Choice.text = choice;
    }

    public void SetChoiceID(string id)
    {
        _choiceID = id;
    }

    private void OnClickChoice()
    {
        VisualNovelManager.Inst.SetCurrentDialogueID(SetReturnID());

        VisualNovelManager.Inst.OnClickChoiceButton?.Invoke();

        UIManager.Inst.OpenDialogueUI();
        UIManager.Inst.CloseChoiceUI();
    }

    private string SetReturnID()
    {
        string returnID = GameDataManager.Inst.GetChoiceData(_choiceID).ReturnID;

        return returnID;
    }
}
