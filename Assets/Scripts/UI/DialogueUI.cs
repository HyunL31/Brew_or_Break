using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("버튼")]
    [SerializeField] private Button Button_Return;
    [SerializeField] private Button Button_Log;
    [SerializeField] private Toggle Toggle_Auto;
    [SerializeField] private Button Button_Skip;

    [Header("텍스트")]
    [SerializeField] private TextMeshProUGUI Text_Dialogue;
    [SerializeField] private TextMeshProUGUI Text_Speaker;

    [Header("이미지")]
    [SerializeField] private Image Image_Background;
    [SerializeField] private Image Image_Character;
    [SerializeField] private Image Image_NextArrow;
    [SerializeField] private Image Image_Speaker;

    [Header("패널")]
    [SerializeField] private GameObject LobbyUI;

    private bool _isTyping = false;
    private WaitForSeconds _typingWaitTime;
    private Coroutine _typingEffect;

    private void Awake()
    {
        _typingWaitTime = new WaitForSeconds(0.03f);

        Button_Skip.onClick.AddListener(SkipDialogue);
    }

    private void OnEnable()
    {
        ShowDialogue(GetCurrentID());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_isTyping)
            {
                _isTyping = false;
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
            Image_Speaker.gameObject.SetActive(true);
            Text_Speaker.text = speaker;
        }
        else
        {
            Image_Speaker.gameObject.SetActive(false);
        }

        if (_typingEffect != null)
        {
            StopCoroutine(_typingEffect);
        }

        _typingEffect = StartCoroutine(Typing(id));

        ChangeSpeakerCharacter(id);
        ChangeBackgroundImage(id);
    }

    private void MoveToNextDialogue(string id)
    {
        string nextID = GameDataManager.Inst.GetDialogueData(id).NextID;

        if (nextID == "Lobby")
        {
            GameManager.Inst.AddDay();
            LobbyUI.SetActive(true);
            this.gameObject.SetActive(false);

            return;
        }

        GameManager.Inst.SetCurrentDialogueID(nextID);
        ShowDialogue(GetCurrentID());
    }

    private void ChangeBackgroundImage(string id)
    {
        string background = GameDataManager.Inst.GetDialogueData(id).Background;

        Image_Background.sprite = Resources.Load<Sprite>($"Background/{background}");
    }

    private void ChangeSpeakerCharacter(string id)
    {
        string characterFacial = GameDataManager.Inst.GetDialogueData(id).Facial;

        if (characterFacial == string.Empty)
        {
            Image_Character.gameObject.SetActive(false);
        }
        else
        {
            Image_Character.gameObject.SetActive(true);
            Image_Character.sprite = Resources.Load<Sprite>($"Character/{characterFacial}");
        }
    }

    private void SkipDialogue()
    {
        string nextID = GameDataManager.Inst.GetDialogueData(GetCurrentID()).NextID;

        if (!nextID.Contains("Episode"))
        {
            return;
        }

        while (nextID.Contains("Episode"))
        {
            GameManager.Inst.SetCurrentDialogueID(nextID);

            nextID = GameDataManager.Inst.GetDialogueData(GetCurrentID()).NextID;
        }

        ShowDialogue(GetCurrentID());
    }

    private IEnumerator Typing(string id)
    {
        _isTyping = true;
        Text_Dialogue.text = string.Empty;
        Image_NextArrow.gameObject.SetActive(false);

        string content = GameDataManager.Inst.GetDialogueData(id).Content;

        for (int i = 0; i < content.Length; i++)
        {
            if (!_isTyping)
            {
                break;
            }

            Text_Dialogue.text += content[i];

            yield return _typingWaitTime;
        }

        Text_Dialogue.text = content;

        _isTyping = false;
        Image_NextArrow.gameObject.SetActive(true);

        yield return null;
    }

    private string GetCurrentID()
    {
        return GameManager.Inst.GetCurrentDialogueID();
    }
}