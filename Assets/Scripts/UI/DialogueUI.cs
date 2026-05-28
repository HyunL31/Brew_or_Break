using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : UIBase
{
    [Header("오디오")]
    [SerializeField] private AudioSource AudioSource;

    [Header("버튼")]
    [SerializeField] private Button Button_Return;
    [SerializeField] private Button Button_Log;
    [SerializeField] private Toggle Toggle_Auto;
    [SerializeField] private Button Button_Skip;

    [Header("텍스트")]
    [SerializeField] private TextMeshProUGUI Text_Dialogue;
    [SerializeField] private TextMeshProUGUI Text_Speaker;

    [Header("이미지")]
    [SerializeField] private Image Image_NextArrow;
    [SerializeField] private Image Image_Speaker;

    private bool _isTyping = false;
    private bool _isAuto = false;
    private float _typingWaitTime = 0.03f;
    private float _autoWaitTime = 0.5f;
    private CancellationTokenSource _typingToken;
    private Dictionary<string, Dialogue> _dialogues;
    private Dictionary<string, Character> _characters;
    private string _lastLogID = string.Empty;

    private void Awake()
    {
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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
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
    }

    private void ShowDialogue(string id)
    {
        if (string.IsNullOrEmpty(_dialogues[id].Speaker))
        {
            Image_Speaker.gameObject.SetActive(false);
        }
        else
        {
            string speaker = _dialogues[id].Speaker;

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

        SetDialogueLog(_dialogues[id].Facial, id);
    }

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
        string speakerName = _characters[speakerID].Name;

        if (speakerName == "{Player.Name}")
        {
            speakerName = GameManager.Inst.PlayerModel.PlayerName;
        }

        Image_Speaker.gameObject.SetActive(true);
        Text_Speaker.text = speakerName;
    }

    private void SetDialogueLog(string speakerID, string currentID)
    {
        if (_lastLogID == currentID)
        {
            return;
        }

        if (string.IsNullOrEmpty(speakerID))
        {
            speakerID = "Narr";
        }

        _lastLogID = currentID;
        VisualNovelManager.Inst.OnAddLog?.Invoke(currentID, speakerID);
    }

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

            SetDialogueLog(_dialogues[nextID].Facial, nextID);

            VisualNovelManager.Inst.OnSetDialogueID(nextID);
            nextID = _dialogues[GetCurrentID()].NextID;
        }

        VisualNovelManager.Inst.OnChangeBaseUI?.Invoke(GetCurrentID());
        ShowDialogue(GetCurrentID());
    }

    private void OnClickAuto(bool isOn)
    {
        if (isOn)
        {
            _isAuto = true;
        }
        else
        {
            _isAuto = false;
        }
    }

    private async UniTaskVoid Typing(string id, CancellationToken token)
    {
        _isTyping = true;

        SoundManager.Inst.OnTyping?.Invoke(AudioSource);

        string content = _dialogues[id].Content;
        Text_Dialogue.maxVisibleCharacters = 0;
        Text_Dialogue.text = content;
        Image_NextArrow.gameObject.SetActive(false);

        for (int i = 0; i < content.Length; i++)
        {
            if (!_isTyping)
            {
                break;
            }

            Text_Dialogue.maxVisibleCharacters = i;

            await UniTask.Delay(TimeSpan.FromSeconds(_typingWaitTime), cancellationToken: token);
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
}