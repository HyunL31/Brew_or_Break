using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] Image Image_Item;
    [SerializeField] TextMeshProUGUI Text_ItemCount;
    [SerializeField] RectTransform Rect;

    private string _itmeID = string.Empty;
    private int _itemCount = 0;
    private GameObject _drawItem = null;

    public void SetSlotInfo(string id)
    {
        _itmeID = id;

        string path = $"Icon/Item[{id}]";

        GameUtil.LoadSpriteAndSet(path, Image_Item).Forget();
    }

    public void SetItemCount(int count)
    {
        _itemCount = count;
        Text_ItemCount.text = count.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIBase uiBase = UIManager.Inst.OpenItemDescription();

        if (uiBase is ItemDescription itemDescription)
        {
            itemDescription.SetPosition(Rect);
            itemDescription.SetItemInfo(_itmeID);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Inst.CloseItemDescription();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!UIManager.Inst.IsOpenedUI(UIType.CraftUI))
        {
            return;
        }

        UIBase uiBase = UIManager.Inst.OpenDrawItem();

        if (uiBase is DrawItem drawItem)
        {
            drawItem.SetItemImage(_itmeID);
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

        SoundManager.Inst.OnSFX?.Invoke("Audio/Plop");
        UIManager.Inst.CloseDrawItem();
        _drawItem = null;

        VisualNovelManager.Inst.OnDropItem?.Invoke(_itmeID);
    }
}