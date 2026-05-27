using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Text_Choice;
    [SerializeField] private Button Button_Choice;

    private string _choiceID;
    private Dictionary<string, Choice> _data;

    private void Awake()
    {
        Button_Choice.onClick.AddListener(OnClickChoice);
    }

    private void Start()
    {
        _data = GameDataManager.Inst.ChoiceDataList;
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
        VisualNovelManager.Inst.OnSetDialogueID(SetReturnID());
        VisualNovelManager.Inst.OnClickChoiceButton?.Invoke();

        UIManager.Inst.OpenDialogueUI();
        UIManager.Inst.CloseChoiceUI();
    }

    private string SetReturnID()
    {
        string returnID = _data[_choiceID].ReturnID;

        return returnID;
    }
}
