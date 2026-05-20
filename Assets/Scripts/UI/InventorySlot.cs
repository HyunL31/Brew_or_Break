using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image Image_Item;
    [SerializeField] TextMeshProUGUI Text_ItemCount;
    [SerializeField] RectTransform Rect;

    private string _itmeID;

    public void SetSlotInfo(string id)
    {
        _itmeID = id;

        string path = $"Icon/Item[{id}]";

        GameUtil.LoadSpriteAndSet(path, Image_Item);
    }

    public void SetItemCount(int count)
    {
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
}
