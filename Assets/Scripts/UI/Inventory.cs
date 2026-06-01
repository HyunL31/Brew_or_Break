using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 인벤토리 UI
/// </summary>

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
    }

    private void OnEnable()
    {
        GameManager.Inst.OnSetInventory = (id, count) => { UpdateInventory(id, count).Forget(); };

        SetInventory().Forget();
    }

    private void OnDisable()
    {
        GameManager.Inst.OnSetInventory = null;
    }

    private async UniTask SetInventory()
    {
        List<ItemModel> items = GameManager.Inst.PlayerModel.Inventory;

        if (items.Count <= 0)
        {
            return;
        }

        foreach (ItemModel item in items)
        {
            await CreateSlot(item.ItemID, item.ItemCount);
        }
    }
    
    // 아이템 개수 변화 시 삭제 및 생성
    private async UniTaskVoid UpdateInventory(string id, int count)
    {
        if (count <= 0)
        {
            if (_inventory.ContainsKey(id))
            {
                if (_inventory[id] != null)
                {
                    Destroy(_inventory[id].gameObject);
                }

                _inventory.Remove(id);
            }

            return;
        }

        await CreateSlot(id, count);
    }

    // 슬롯 생성
    private async UniTask CreateSlot(string id, int count)
    {
        if (_inventory.ContainsKey(id))
        {
            _inventory[id].SetItemCount(count);
        }
        else
        {
            if (_inventory.ContainsKey(id) && _inventory[id] == null)
            {
                await UniTask.WaitUntil(() => _inventory.ContainsKey(id) && _inventory[id] != null);
                _inventory[id].SetItemCount(count);
                return;
            }
            string path = "Prefabs/UI/InventorySlot";

            _inventory.Add(id, null);

            GameObject prefab = await ResourceManager.Inst.InstantiatePrefab(path, SlotParent);
            InventorySlot inventorySlot = prefab.GetComponent<InventorySlot>();

            inventorySlot.SetSlotInfo(id);
            inventorySlot.SetItemCount(count);

            _inventory[id] = inventorySlot;
        }
    }
}