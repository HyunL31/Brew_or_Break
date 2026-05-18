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

    private string _currentID;

    private void Awake()
    {
        _currentID = "Episode_00_01";
    }

    private void OnEnable()
    {
        ShowDialogue(_currentID);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {

        }
    }

    private void ShowDialogue(string id)
    {

    }
}
