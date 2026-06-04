using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 비주얼 노벨 대화창 UI
/// </summary>

public class DialogueUI : UIBase
{
    [Header("오디오")]
    [SerializeField] private AudioSource AudioSource;

    [Header("버튼")]
    [SerializeField] private Button Button_Return;
    [SerializeField] private Button Button_Log;
    [SerializeField] private Toggle Toggle_Auto;
    [SerializeField] private Button Button_Skip;
    [SerializeField] private Button Button_Dialogue;

    [Header("텍스트")]
    [SerializeField] private TextMeshProUGUI Text_Dialogue;
    [SerializeField] private TextMeshProUGUI Text_Speaker;

    [Header("이미지")]
    [SerializeField] private Image Image_NextArrow;
    [SerializeField] private Image Image_Speaker;

    private bool _isTyping = false;
    private bool _isAuto = false;
    private float _typingWaitTime;
    private float _autoWaitTime = 0.5f;
    private CancellationTokenSource _typingToken;
    private Dictionary<string, Dialogue> _dialogues;
    private Dictionary<string, Character> _characters;
    private string _lastLogID = string.Empty;

    private void Awake()
    {
        Button_Dialogue.onClick.AddListener(OnClickDialogoue);
        Button_Skip.onClick.AddListener(SkipDialogue);
        Toggle_Auto.isOn = _isAuto;
        Toggle_Auto.onValueChanged.AddListener(OnClickAuto);

        Button_Return.onClick.AddListener(OpenLobbyPopup);
        Button_Log.onClick.AddListener(UIManager.Inst.OpenDialogueLog);

        _dialogues = GameDataManager.Inst.DialogueDataList;
        _characters = GameDataManager.Inst.CharacterDataList;

        UIManager.Inst.OpenDialogueLog();
        UIManager.Inst.CloseDialogueLog();
    }

    private void OnEnable()
    {
        _typingWaitTime = PlayerPrefs.GetFloat("TextSpeed", 0.03f);

        ShowDialogue(GetCurrentID());

        if (GameManager.Inst.PlayerModel.Day == 0)
        {
            Button_Return.gameObject.SetActive(false);
        }
        else
        {
            Button_Return.gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        CancelTypingRoutine();
    }

    private void OnClickDialogoue()
    {
        if (UIManager.Inst.IsOpenedUI(UIType.DialogueLog))
        {
            return;
        }
        else if (_isTyping)
        {
            _isTyping = false;
            SoundManager.Inst.OnPause?.Invoke(AudioSource);
        }
        else
        {
            MoveToNextDialogue(GetCurrentID());
        }
    }

    // 대사 보여주기
    private void ShowDialogue(string id)
    {
        if (_dialogues[id].Speakers.Count == 0)
        {
            Image_Speaker.gameObject.SetActive(false);
        }
        else
        {
            string speaker = _dialogues[id].Speakers[0];

            if (speaker != string.Empty)
            {
                SetCharacterName(speaker);
            }
        }

        CancelTypingRoutine();
        _typingToken = new CancellationTokenSource();

        Typing(id, _typingToken.Token).Forget();

        VisualNovelManager.Inst.OnChangeBaseUI?.Invoke(id);

        SetBGM(_dialogues[id].BGM);
        SetSFX(_dialogues[id].SFX);

        SetDialogueLog(id);

        if (!string.IsNullOrEmpty(_dialogues[id].Command))
        {
            VisualNovelManager.Inst.OnDialogueCommand?.Invoke(id);
        }
    }

    // 다음 대사로 이동
    private void MoveToNextDialogue(string id)
    {
        string nextID = _dialogues[id].NextID;

        bool isMoved = VisualNovelManager.Inst.OnMoveNextContent(nextID);

        if (!isMoved)
        {
            VisualNovelManager.Inst.OnSetDialogueID(nextID);
            ShowDialogue(GetCurrentID());
        }
    }

    private void SetCharacterName(string speakerID)
    {
        string characterID = GetCharacterID(speakerID);
        string speakerName = _characters[characterID].Name;

        if (speakerName == "{Player.Name}")
        {
            speakerName = GameManager.Inst.PlayerModel.PlayerName;
        }

        Image_Speaker.gameObject.SetActive(true);
        Text_Speaker.text = speakerName;
    }

    // 대사 로그 설정
    private void SetDialogueLog(string currentID)
    {
        if (_lastLogID == currentID)
        {
            return;
        }

        string speakerID = string.Empty;

        if (_dialogues[currentID].Speakers.Count != 0)
        {
            speakerID = _dialogues[currentID].Speakers[0];
        }
        else
        {
            speakerID = "Narr";
        }

        _lastLogID = currentID;
        VisualNovelManager.Inst.OnAddLog?.Invoke(currentID, speakerID);
    }

    // 대사 스킵 (다음 컨텐츠까지)
    private void SkipDialogue()
    {
        CancelTypingRoutine();

        string nextID = _dialogues[GetCurrentID()].NextID;

        foreach (var data in _dialogues)
        {
            if (nextID == "Lobby" || nextID == "Account" || nextID.Contains("Clue") || nextID.Contains("Choice") || nextID.Contains("Craft") || nextID == "0")
            {
                break;
            }

            SetDialogueLog(nextID);

            VisualNovelManager.Inst.OnSetDialogueID(nextID);
            nextID = _dialogues[GetCurrentID()].NextID;
        }

        VisualNovelManager.Inst.OnChangeBaseUI?.Invoke(GetCurrentID());
        ShowDialogue(GetCurrentID());
    }

    // 자동 진행
    private void OnClickAuto(bool isOn)
    {
        if (isOn)
        {
            _isAuto = true;

            if (!_isTyping)
            {
                MoveToNextDialogue(GetCurrentID());
            }
        }
        else
        {
            _isAuto = false;
        }
    }

    // 타이핑 효과
    private async UniTaskVoid Typing(string id, CancellationToken token)
    {
        _isTyping = true;

        SoundManager.Inst.OnTyping?.Invoke(AudioSource);

        string content = _dialogues[id].Content;
        Text_Dialogue.maxVisibleCharacters = 0;
        Text_Dialogue.text = content;
        Image_NextArrow.gameObject.SetActive(false);

        if (_typingWaitTime > 0)
        {
            for (int i = 0; i < content.Length; i++)
            {
                if (!_isTyping)
                {
                    break;
                }

                Text_Dialogue.maxVisibleCharacters = i;

                await UniTask.Delay(TimeSpan.FromSeconds(_typingWaitTime), cancellationToken: token);
            }
        }

        Text_Dialogue.maxVisibleCharacters = content.Length;

        _isTyping = false;
        SoundManager.Inst.OnPause?.Invoke(AudioSource);
        Image_NextArrow.gameObject.SetActive(true);

        if (_isAuto)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_autoWaitTime), cancellationToken: token);

            MoveToNextDialogue(id);
        }
    }

    private void CancelTypingRoutine()
    {
        if (_typingToken != null)
        {
            _typingToken.Cancel();
            _typingToken.Dispose();
            _typingToken = null;
        }
    }

    private string GetCurrentID()
    {
        return VisualNovelManager.Inst.CurrentDialogueID;
    }

    private void OpenLobbyPopup()
    {
        UIManager.Inst.OpenConfirmPopup("지금 로비로 돌아가면 저장이 되지 않습니다.\n돌아가시겠습니까?", ReturnLobby);
    }

    private void ReturnLobby()
    {
        UIManager.Inst.OpenLobbyUI();
        UIManager.Inst.CloseVisualNovelUI();
        UIManager.Inst.CloseDialogueUI();
    }

    // 사운드 설정
    private void SetBGM(string bgm)
    {
        if (bgm != string.Empty)
        {
            SoundManager.Inst.OnBGM?.Invoke($"Audio/{bgm}");
        }
    }

    private void SetSFX(string sfx)
    {
        if (sfx != string.Empty)
        {
            SoundManager.Inst.OnSFX?.Invoke($"Audio/{sfx}");
        }
    }

    private string GetCharacterID(string character)
    {
        string[] results = character.Split('_');
        string characterID = $"{results[0]}_{results[1]}";

        return characterID;
    }
}