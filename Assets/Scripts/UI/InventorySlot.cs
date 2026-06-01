using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 인벤토리 슬롯 전용 컴포넌트
/// </summary>

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image Image_Item;
    [SerializeField] private TextMeshProUGUI Text_ItemCount;
    [SerializeField] private RectTransform Rect;

    public string ItemID {  get; private set; } = string.Empty;
    private GameObject _drawItem = null;

    public void SetSlotInfo(string id)
    {
        ItemID = id;

        string path = $"Icon/Item[{id}]";

        GameUtil.LoadSpriteAndSet(path, Image_Item).Forget();
    }

    public void SetItemCount(int count)
    {
        Text_ItemCount.text = count.ToString();
    }

    // 마우스 호버 시 아이템 설명창 열기
    public void OnPointerEnter(PointerEventData eventData)
    {
        UIBase uiBase = UIManager.Inst.OpenItemDescription();

        if (uiBase is ItemDescription itemDescription)
        {
            itemDescription.SetPosition(Rect);
            itemDescription.SetItemInfo(ItemID);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Inst.CloseItemDescription();
    }

    // 아이템 추가할 때 드래그 (Craft Content 진행 시에만)
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!UIManager.Inst.IsOpenedUI(UIType.CraftUI))
        {
            return;
        }

        UIBase uiBase = UIManager.Inst.OpenDrawItem();

        if (uiBase is DrawItem drawItem)
        {
            drawItem.SetItemImage(ItemID);
        }

        _drawItem = uiBase.gameObject;
        _drawItem.transform.position = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!UIManager.Inst.IsOpenedUI(UIType.CraftUI))
        {
            return;
        }

        _drawItem.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!UIManager.Inst.IsOpenedUI(UIType.CraftUI))
        {
            return;
        }

        UIManager.Inst.CloseDrawItem();
        _drawItem = null;
    }
}