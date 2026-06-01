using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 단서 버튼
/// </summary>

public class ClueButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string ClueID;
    [SerializeField] private Button Button_Clue;

    private Dictionary<string, Clue> _data;

    private void Awake()
    {
        Button_Clue.onClick.AddListener(SetReturnID);
        Button_Clue.onClick.AddListener(() => OnClickClue().Forget());
    }

    private void Start()
    {
        _data = GameDataManager.Inst.ClueDataList;
    }

    // 마우스 커서 이미지 변경
    public void OnPointerEnter(PointerEventData eventData)
    {
        SetCursorImage("ClueCursor").Forget();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetCursorImage("DefaultCursor").Forget();
    }

    private async UniTask SetCursorImage(string path)
    {
        Texture2D cursor = await ResourceManager.Inst.LoadAsset<Texture2D>(path);

        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
    }

    private async UniTask OnClickClue()
    {
        VisualNovelManager.Inst.OnClickClueButton?.Invoke(ClueID);

        await SetCursorImage("DefaultCursor");

        int point = _data[ClueID].Point;
        StoreManager.Inst.SetCluePoint(point);

        this.gameObject.SetActive(false);
    }

    private void SetReturnID()
    {
        string returnID = _data[ClueID].ReturnID;

        VisualNovelManager.Inst.OnSetDialogueID(returnID);
    }
}
