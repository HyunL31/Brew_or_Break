using System;
using UnityEngine;

public class VisualNovelManager : MonoBehaviour
{
    public static VisualNovelManager Inst;

    private string CurrentDialogueID { get; set; }

    public Action<string> OnChangeBaseUI;
    public Action<string> OnClickClueButton;
    public Action OnClickChoiceButton;
    public Action<string> OnDropItem;

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
            GameManager.Inst.SetDay();

            UIManager.Inst.OpenLobbyUI();
            UIManager.Inst.CloseDialogueUI();
            UIManager.Inst.CloseVisualNovelUI();

            isMoved = true;
        }
        else if (nextID == "Account")
        {
            SetResult();

            UIManager.Inst.OpenAccountUI();
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
        else if (nextID.Contains("Craft"))
        {
            SetCurrentDialogueID(nextID);

            UIManager.Inst.CloseDialogueUI();
            UIManager.Inst.OpenCraftUI();
            UIManager.Inst.OpenRecipePopup();

            isMoved = true;
        }
        else
        {
            UIManager.Inst.OpenDialogueUI();
        }

        return isMoved;
    }

    private void SetResult()
    {
        string resultID = GameDataManager.Inst.GetDialogueData(CurrentDialogueID).ResultID;

        int gold = GameDataManager.Inst.GetResultData(resultID).Gold;
        int reputation = GameDataManager.Inst.GetResultData(resultID).Reputation;

        StoreManager.Inst.SetGold(gold);
        StoreManager.Inst.SetReputation(reputation);
    }

    public string GetCurrentDialogueID()
    {
        return CurrentDialogueID;
    }

    public void SetCurrentDialogueID()
    {
        CurrentDialogueID = $"Episode_{GetCurrentDay()}_01";
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
