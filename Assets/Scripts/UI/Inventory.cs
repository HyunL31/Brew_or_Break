using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : UIBase
{
    [SerializeField] Button Button_Close;
    [SerializeField] Transform SlotParent;
    
    private List<GameObject> _itemSlots = new List<GameObject>();

    private void Awake()
    {
        Button_Close.onClick.AddListener(UIManager.Inst.CloseInventory);
        SetInventory();
    }

    private void SetInventory()
    {
        List<ItemModel> items = GameManager.Inst.GetInventory();

        foreach (ItemModel item in items)
        {
            string path = "Prefabs/UI/InventorySlot";

            ResourceManager.Inst.InstantiatePrefab(path, SlotParent, (prefab) =>
            {
                InventorySlot inventorySlot = prefab.GetComponent<InventorySlot>();

                inventorySlot.SetSlotInfo(item.ItemID);
                inventorySlot.SetItemCount(item.ItemCount);
            });
        }
    }
}