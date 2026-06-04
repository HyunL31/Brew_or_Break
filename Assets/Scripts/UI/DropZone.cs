using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 아이템 드랍 존 구현 (마법약 솥에 넣어야 아이템이 들어갈 수 있도록)
/// </summary>

public class DropZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        SoundManager.Inst.OnSFX?.Invoke("Audio/Plop");

        GameObject dragItem = eventData.pointerDrag;
        InventorySlot slot = dragItem.GetComponent<InventorySlot>();
        UIManager.Inst.CloseDrawItem();

        if (slot.ItemID != null)
        {
            VisualNovelManager.Inst.OnDropItem?.Invoke(slot.ItemID);
        }
    }
}