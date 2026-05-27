using Cysharp.Threading.Tasks;
using System;
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

    private void Awake()
    {
        Button_Skip.onClick.AddListener(SkipDialogue);
        Toggle_Auto.isOn = _isAuto;
        Toggle_Auto.onValueChanged.AddListener(OnClickAuto);

        Button_Return.onClick.AddListener(OpenLobbyPopup);
    }

    private void OnEnable()
    {
        ShowDialogue(GetCurrentID());
    }

    private void OnDisable()
    {
        CancelTypingRoutine();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_isTyping)
            {
                _isTyping = false;
                SoundManager.Inst.PauseAudio(AudioSource);
            }
            else
            {
                MoveToNextDialogue(GetCurrentID());
            }
        }
    }

    private void ShowDialogue(string id)
    {
        var data = GameDataManager.Inst.GetDialogueData(id);
        string speaker = data.Speaker;

        if (speaker != string.Empty)
        {
            SetCharacterName(speaker);
        }
        else
        {
            Image_Speaker.gameObject.SetActive(false);
        }

        CancelTypingRoutine();
        _typingToken = new CancellationTokenSource();

        Typing(id, _typingToken.Token).Forget();

        VisualNovelManager.Inst.OnChangeBaseUI?.Invoke(id);

        SetBGM(data.BGM);
        SetSFX(data.SFX);
    }

    private void MoveToNextDialogue(string id)
    {
        string nextID = GameDataManager.Inst.GetDialogueData(id).NextID;

        bool isMoved = VisualNovelManager.Inst.MoveToContent(nextID);

        if (!isMoved)
        {
            VisualNovelManager.Inst.SetCurrentDialogueID(nextID);
            ShowDialogue(GetCurrentID());
        }
    }

    private void SetCharacterName(string speakerID)
    {
        string speakerName = GameDataManager.Inst.GetCharacterData(speakerID).Name;

        if (speakerName == "{Player.Name}")
        {
            speakerName = GameManager.Inst.GetPlayerName();
        }

        Image_Speaker.gameObject.SetActive(true);
        Text_Speaker.text = speakerName;
    }

    private void SkipDialogue()
    {
        var data = GameDataManager.Inst.DialogueDataList;
        string nextID = GameDataManager.Inst.GetDialogueData(GetCurrentID()).NextID;

        if (!nextID.Contains("Episode"))
        {
            return;
        }

        foreach (var d in data)
        {
            if (!nextID.Contains("Episode"))
            {
                break;
            }

            // 월권
            VisualNovelManager.Inst.SetCurrentDialogueID(nextID);

            // 캐싱 필요
            nextID = GameDataManager.Inst.GetDialogueData(GetCurrentID()).NextID;
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

        SoundManager.Inst.SetTypingAndPlay(AudioSource).Forget();

        string content = GameDataManager.Inst.GetDialogueData(id).Content;
        Text_Dialogue.maxVisibleCharacters = 0;
        Text_Dialogue.text = content;
        Image_NextArrow.gameObject.SetActive(false);

        // while 권장 X (while보다는 for문으로)
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
        SoundManager.Inst.PauseAudio(AudioSource);
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
        return VisualNovelManager.Inst.GetCurrentDialogueID();
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
            SoundManager.Inst.SetBGMAndPlay($"Audio/{bgm}").Forget();
        }
    }

    private void SetSFX(string sfx)
    {
        if (sfx != string.Empty)
        {
            SoundManager.Inst.SetSFXAndPlay($"Audio/{sfx}").Forget();
        }
    }
}