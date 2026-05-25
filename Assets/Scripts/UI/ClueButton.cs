using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClueButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string ClueID;
    [SerializeField] private Button Button_Clue;

    private void Awake()
    {
        Button_Clue.onClick.AddListener(SetReturnID);
        Button_Clue.onClick.AddListener(() => OnClickClue().Forget());
    }

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
        this.gameObject.SetActive(false);

        // [TODO] 점수 계산
    }

    private void SetReturnID()
    {
        string returnID = GameDataManager.Inst.GetClueData(ClueID).ReturnID;

        VisualNovelManager.Inst.SetCurrentDialogueID(returnID);
    }
}
