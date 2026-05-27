using System;
using System.Collections.Generic;
using UnityEngine;

public class VisualNovelManager : MonoBehaviour
{
    public static VisualNovelManager Inst;

    public string CurrentDialogueID { get; private set; }

    public Action<string> OnChangeBaseUI;
    public Action<string> OnClickClueButton;
    public Action OnClickChoiceButton;
    public Action<string> OnDropItem;
    public Action<string> OnSetDialogueID;
    public Func<string, bool> OnMoveNextContent;

    private Dictionary<string, Dialogue> _dialogues;
    private Dictionary<string, Result> _results;

    private void Awake()
    {
        Inst = this;

        OnSetDialogueID = SetCurrentDialogueID;
        OnMoveNextContent =  MoveToContent;
    }

    private void Start()
    {
        _dialogues = GameDataManager.Inst.DialogueDataList;
        _results = GameDataManager.Inst.ResultDataList;

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
        else if (nextID == "0")
        {
            UIManager.Inst.CloseDialogueUI();
            UIManager.Inst.InitStart();
        }
        else
        {
            UIManager.Inst.OpenDialogueUI();
        }

        return isMoved;
    }

    private void SetResult()
    {
        string resultID = _dialogues[CurrentDialogueID].ResultID;

        int gold = _results[resultID].Gold;
        int reputation = _results[resultID].Reputation;

        StoreManager.Inst.SetGold(gold);
        StoreManager.Inst.SetReputation(reputation);
    }

    public void SetCurrentDialogueID()
    {
        CurrentDialogueID = $"Episode_{GetCurrentDay()}_01";
    }

    private void SetCurrentDialogueID(string id)
    {
        CurrentDialogueID = id;
    }

    private int GetCurrentDay()
    {
        return GameManager.Inst.PlayerModel.Day;
    }

    public void CheckEnding()
    {
        string end = string.Empty;

        if (StoreManager.Inst.StoreModel.Compensation >= 0)
        {
            end = "Bad";
        }
        else if (StoreManager.Inst.StoreModel.Reputation < 1000)
        {
            end = "Normal";
        }
        else
        {
            end = "Good";
        }

        CurrentDialogueID = $"{end}_End_01";
    }
}
