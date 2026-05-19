using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClueButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string ClueID;
    [SerializeField] private Button Button_Clue;

    [SerializeField] private ClueUI ClueUI;

    private void Awake()
    {
        Button_Clue.onClick.AddListener(OnClickClue);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetCursorImage("ClueCursor");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetCursorImage("DefaultCursor");
    }

    private void SetCursorImage(string path)
    {
        ResourceManager.Inst.LoadAsset<Texture2D>(path, (cursor) =>
        {
            Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
        });
    }

    private void OnClickClue()
    {
        ClueUI.OnClickClueButton?.Invoke(ClueID);

        // [TODO] 점수 계산
    }
}
