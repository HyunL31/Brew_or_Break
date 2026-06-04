using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RestartPopup : UIBase
{
    [SerializeField] private Button Button_Restart;
    [SerializeField] private TextMeshProUGUI Text_Ending;
    [SerializeField] private TextMeshProUGUI Text_Description;

    private Dictionary<string, Ending> _data;
    private string _endingID;

    private void Awake()
    {
        _data = GameDataManager.Inst.EndingDataList;

        Button_Restart.onClick.AddListener(OnClickRestart);
    }

    private void OnEnable()
    {
        _endingID = VisualNovelManager.Inst.GetEndingID();
        SetEndingText();
    }

    private void SetEndingText()
    {
        Text_Ending.text = _data[_endingID].Name;
        Text_Description.text = _data[_endingID].Description;
    }

    private void OnClickRestart()
    {
        GameManager.Inst.SaveData();
        UIManager.Inst.CloseDialogueUI();
        UIManager.Inst.CloseVisualNovelUI();
        UIManager.Inst.InitStart();

        UIManager.Inst.CloseEndingPopup();
    }
}
