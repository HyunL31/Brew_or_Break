using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 인벤토리 UI
/// </summary>

public class Inventory : UIBase
{
    [SerializeField] Button Button_Close;
    [SerializeField] Transform SlotParent;
    [SerializeField] TextMeshProUGUI Text_EmptyInfo;

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
        GameManager.Inst.OnSetInventory -= (id, count) => { UpdateInventory(id, count).Forget(); };
        UIManager.Inst.CloseItemDescription();
    }

    private async UniTask SetInventory()
    {
        List<ItemModel> items = GameManager.Inst.PlayerModel.Inventory;

        // 인벤토리 슬롯 초기화
        foreach (InventorySlot slot in _inventory.Values)
        {
            if (slot != null)
            {
                slot.gameObject.SetActive(false);
            }
        }

        if (items == null || items.Count <= 0)
        {
            Text_EmptyInfo.gameObject.SetActive(true);
            return;
        }

        foreach (ItemModel item in items)
        {
            if (item.ItemCount <= 0)
            {
                continue;
            }

            await CreateSlot(item.ItemID, item.ItemCount);
        }
    }
    
    // 아이템 개수 변화 시 삭제 및 생성
    private async UniTaskVoid UpdateInventory(string id, int count)
    {
        if (count <= 0)
        {
            if (_inventory.ContainsKey(id) && _inventory[id] != null)
            {
                _inventory[id].gameObject.SetActive(false);
            }

            bool hasActiveSlot = false;
            foreach (InventorySlot slot in _inventory.Values)
            {
                if (slot != null && slot.gameObject.activeSelf)
                {
                    hasActiveSlot = true;
                    break;
                }
            }

            if (!hasActiveSlot)
            {
                Text_EmptyInfo.gameObject.SetActive(true);
            }

            return;
        }

        await CreateSlot(id, count);
        Text_EmptyInfo.gameObject.SetActive(false);
    }

    // 슬롯 생성
    private async UniTask CreateSlot(string id, int count)
    {
        if (_inventory.ContainsKey(id) && _inventory[id] != null)
        {
            _inventory[id].gameObject.SetActive(true);
            _inventory[id].SetItemCount(count);
        }
        else
        {
            if (_inventory.ContainsKey(id))
            {
                return;
            }

            _inventory.Add(id, null);

            string path = "Prefabs/UI/InventorySlot";

            GameObject prefab = await ResourceManager.Inst.InstantiatePrefab(path, SlotParent);
            InventorySlot inventorySlot = prefab.GetComponent<InventorySlot>();

            inventorySlot.SetSlotInfo(id);
            inventorySlot.SetItemCount(count);

            _inventory[id] = inventorySlot;
        }
    }
}