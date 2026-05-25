using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : UIBase
{
    [SerializeField] Button Button_Close;
    [SerializeField] Transform SlotParent;

    private Dictionary<string, InventorySlot> _inventory = new Dictionary<string, InventorySlot>();

    private void Awake()
    {
        if (Button_Close != null)
        {
            Button_Close.onClick.AddListener(UIManager.Inst.CloseInventory);
        }

        GameManager.Inst.OnSetInventory = (id) => ResetInventory(id);
    }

    private void OnEnable()
    {
        SetInventory().Forget();
    }

    private async UniTask SetInventory()
    {
        _inventory.Clear();

        List<ItemModel> items = GameManager.Inst.GetInventory();

        foreach (ItemModel item in items)
        {
            string path = "Prefabs/UI/InventorySlot";

            GameObject prefab = await ResourceManager.Inst.InstantiatePrefab(path, SlotParent);

            InventorySlot inventorySlot = prefab.GetComponent<InventorySlot>();

            inventorySlot.SetSlotInfo(item.ItemID);
            inventorySlot.SetItemCount(item.ItemCount);

            _inventory.Add(item.ItemID, inventorySlot);
        }
    }

    private void ResetInventory(string id)
    {
        ItemModel itemModel = null;

        foreach (ItemModel item in GameManager.Inst.GetInventory())
        {
            if (item.ItemID == id)
            {
                itemModel = item;
                break;
            }
        }

        if (itemModel != null && itemModel.ItemCount <= 0)
        {
            Destroy(_inventory[id].gameObject);
        }
    }
}