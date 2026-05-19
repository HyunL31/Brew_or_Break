using System;
using UnityEngine;

public class VisualNovelManager : MonoBehaviour
{
    public static VisualNovelManager Inst;

    private string CurrentDialogueID { get; set; }

    public Action<string> OnChangeBaseUI;
    public Action<string> OnClickClueButton;
    public Action OnClickChoiceButton;

    private void Awake()
    {
        Inst = this;
    }

    private void Start()
    {
        SetCurrentDialogueID();
    }

    public bool MoveToContent(string nextID)
    {
        bool isMoved = false;

        if (nextID == "Lobby")
        {
            GameManager.Inst.AddDay();
            UIManager.Inst.OpenLobbyUI();
            UIManager.Inst.CloseDialogueUI();
            UIManager.Inst.CloseVisualNovelUI();

            isMoved = true;
        }
        else if (nextID.Contains("Clue"))
        {
            UIManager.Inst.CloseDialogueUI();
            UIManager.Inst.OpenClueUI();

            isMoved = true;
        }
        else if (nextID.Contains("Choice"))
        {
            SetCurrentDialogueID(nextID);

            UIManager.Inst.CloseDialogueUI();
            UIManager.Inst.OpenChoiceUI();

            isMoved = true;
        }
        else
        {
            UIManager.Inst.OpenDialogueUI();
        }

        return isMoved;
    }

    public string GetCurrentDialogueID()
    {
        return CurrentDialogueID;
    }

    public void SetCurrentDialogueID()
    {
        if (GetCurrentDay() >= 10)
        {
            CurrentDialogueID = $"Episode_{GetCurrentDay()}_01";
        }
        else
        {
            CurrentDialogueID = $"Episode_0{GetCurrentDay()}_01";
        }
    }

    public void SetCurrentDialogueID(string id)
    {
        CurrentDialogueID = id;
    }

    private int GetCurrentDay()
    {
        return GameManager.Inst.GetDay();
    }
}
