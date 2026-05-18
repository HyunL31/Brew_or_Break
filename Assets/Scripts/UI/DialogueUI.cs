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

    private string _currentID;
    private bool _isTyping = false;
    private WaitForSeconds _typingWaitTime;
    private Coroutine _typingEffect;

    private void Awake()
    {
        _typingWaitTime = new WaitForSeconds(0.03f);
        _currentID = "Episode_00_01";
    }

    private void OnEnable()
    {
        ShowDialogue(_currentID);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MoveToNextDialogue(_currentID);
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

        _typingEffect = StartCoroutine(Typing(_currentID));

        ChangeSpeakerCharacter(_currentID);
        ChangeBackgroundImage(_currentID);
    }

    private void MoveToNextDialogue(string id)
    {
        string nextID = GameDataManager.Inst.GetDialogueData(id).NextID;
        _currentID = nextID;

        ShowDialogue(_currentID);
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

    private IEnumerator Typing(string id)
    {
        _isTyping = true;
        Text_Dialogue.text = string.Empty;

        string content = GameDataManager.Inst.GetDialogueData(id).Content;

        for (int i = 0; i < content.Length; i++)
        {
            Text_Dialogue.text += content[i];

            yield return _typingWaitTime;
        }

        yield return null;
    }
}