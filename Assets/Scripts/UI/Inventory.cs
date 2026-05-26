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
        List<ItemModel> items = GameManager.Inst.GetInventory();

        Debug.Log($"{items.Count}, {_inventory.Count}");

        for (int i = _inventory.Count; i < items.Count; i++)
        {
            string path = "Prefabs/UI/InventorySlot";

            GameObject prefab = await ResourceManager.Inst.InstantiatePrefab(path, SlotParent);

            InventorySlot inventorySlot = prefab.GetComponent<InventorySlot>();

            inventorySlot.SetSlotInfo(items[i].ItemID);
            inventorySlot.SetItemCount(items[i].ItemCount);

            _inventory.Add(items[i].ItemID, inventorySlot);
            Debug.Log(items[i].ItemID);
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
            _inventory.Remove(id);
        }
    }
}