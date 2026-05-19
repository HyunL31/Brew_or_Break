using UnityEngine;

public class VisualNovelManager : MonoBehaviour
{
    public static VisualNovelManager Inst;

    private string CurrentDialogueID { get; set; }

    private void Awake()
    {
        Inst = this;

        SetCurrentDialogueID();
    }

    private void MoveToContent(string nextID)
    {
        if (nextID == "Lobby")
        {
            GameManager.Inst.AddDay();
            UIManager.Inst.OpenLobbyUI();
            UIManager.Inst.CloseDialogueUI();
            UIManager.Inst.CloseVisualNovelUI();
        }
        else if (nextID.Contains("Clue"))
        {
            UIManager.Inst.CloseDialogueUI();
            UIManager.Inst.OpenClueUI();
        }
        else if (nextID.Contains("Choice"))
        {
            UIManager.Inst.OpenChoiceUI();
        }
        else
        {
            UIManager.Inst.OpenDialogueUI();
        }
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
