using TMPro;
using UnityEngine;

public class PlayerBubble : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Text_Bubble;
    [SerializeField] private RectTransform Rect;

    public void SetBubbleText(string text, Transform playerPos)
    {
        CollectingManager.Inst.SetHUDPos(playerPos, Rect, 2f);

        Text_Bubble.text = text;
    }
}
