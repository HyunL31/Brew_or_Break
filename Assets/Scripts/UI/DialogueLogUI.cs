using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 대사 로그 팝업 UI
/// </summary>

public class DialogueLogUI : UIBase
{
    [SerializeField] private Button Button_Close;
    [SerializeField] private Transform SlotParent;

    private List<LogSlot> _logs = new List<LogSlot>();

    private void Awake()
    {
        Button_Close.onClick.AddListener(UIManager.Inst.CloseDialogueLog);
        VisualNovelManager.Inst.OnExitDialogue += ClearLogSlot;
    }

    private void OnEnable()
    {
        SetDialogueLog().Forget();
    }

    private async UniTaskVoid SetDialogueLog()
    {
        var dialogueLogs = VisualNovelManager.Inst.DialogueLogs;

        for (int i = _logs.Count; i < dialogueLogs.Count; i++)
        {
            string currentSpeaker = dialogueLogs[i].Value;
            bool isSameSpeaker = (i != 0) && (currentSpeaker == dialogueLogs[i - 1].Value);

            string path = SetSlotPath(currentSpeaker);
            GameObject prefab = await ResourceManager.Inst.InstantiatePrefab(path, SlotParent);
            LogSlot logSlot = prefab.GetComponent<LogSlot>();

            logSlot.InitSlot(currentSpeaker, VisualNovelManager.Inst.DialogueLogs[i].Key, isSameSpeaker);

            _logs.Add(logSlot);
        }
    }

    // 캐릭터 유형 별 슬롯 path 설정
    private string SetSlotPath(string characterID)
    {
        if (characterID.Contains("Narr"))
        {
            return "Prefabs/UI/NarrationLog";
        }
        else if (characterID.Contains("Player"))
        {
            return "Prefabs/UI/PlayerLogSlot";
        }
        else
        {
            return "Prefabs/UI/CharacterLogSlot";
        }
    }

    private void ClearLogSlot()
    {
        foreach (var log in _logs)
        {
            Destroy(log.gameObject);
        }

        _logs.Clear();
    }
}