using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// 비주얼 노벨 콘텐츠 관리 매니저
/// </summary>

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
    public Action<string, string> OnAddLog;
    public Action OnExitDialogue;
    public Action<string> OnDialogueCommand;

    private Dictionary<string, Dialogue> _dialogues;
    private Dictionary<string, Result> _results;
    public List<KeyValuePair<string, string>> DialogueLogs { get; private set; } = new List<KeyValuePair<string, string>>();

    private void Awake()
    {
        Inst = this;

        OnSetDialogueID = SetCurrentDialogueID;
        OnMoveNextContent =  MoveToContent;
        OnAddLog = AddLog;
        OnExitDialogue += ClearLog;
    }

    private void Start()
    {
        _dialogues = GameDataManager.Inst.DialogueDataList;
        _results = GameDataManager.Inst.ResultDataList;

        SetCurrentDialogueID();
    }

    // 다음 콘텐츠로 이동
    public bool MoveToContent(string nextID)
    {
        bool isMoved = false;

        if (nextID == "Lobby")
        {
            OnExitDialogue?.Invoke();

            GameManager.Inst.SetDay();

            UIManager.Inst.OpenLobbyUI();
            UIManager.Inst.CloseDialogueUI();
            UIManager.Inst.CloseVisualNovelUI();

            isMoved = true;
        }
        else if (nextID == "Account")
        {
            OnExitDialogue?.Invoke();

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
        else if (nextID.Contains("End") && nextID.Contains("01"))
        {
            GameManager.Inst.PlayerModel.EndingID = nextID;
        }
        else if (nextID == "0")
        {
            GameManager.Inst.PlayerModel.IsComplete = true;
            UIManager.Inst.OpenEndingPopup();

            isMoved = true;
        }
        else
        {
            UIManager.Inst.OpenDialogueUI();
        }

        return isMoved;
    }

    // 결과 적용
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

    // 대화 로그 추가
    private void AddLog(string content, string characterID)
    {
        DialogueLogs.Add(new KeyValuePair<string, string>(content, characterID));
    }

    private void ClearLog()
    {
        DialogueLogs.Clear();
    }

    // 엔딩 적용
    public string GetEndingID()
    {
        return _dialogues[CurrentDialogueID].Command;
    }
}
