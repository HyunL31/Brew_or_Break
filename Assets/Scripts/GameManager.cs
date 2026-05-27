using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst;

    public PlayerModel PlayerModel { get; private set; } = new PlayerModel();

    public Action<string, int> OnSetInventory;

    private void Awake()
    {
        Inst = this;
    }

    public void SaveData()
    {
        SaveManager.Inst.RequestSaveData(PlayerModel);
    }

    public void LoadData()
    {
        PlayerModel = SaveManager.Inst.RequestLoadData();
    }

    public void LoadDefaultData()
    {
        PlayerModel = SaveManager.Inst.RequestLoadDefaultData();
    }

    public void UseItem(string id)
    {
        ItemModel target = null;

        foreach (ItemModel item in PlayerModel.Inventory)
        {
            if (item.ItemID.Contains(id))
            {
                target = item;
                break;
            }
        }

        if (target != null)
        {
            target.ItemCount--;

            if (target.ItemCount <= 0)
            {
                PlayerModel.Inventory.Remove(target);
                OnSetInventory?.Invoke(id, 0);
            }
            else
            {
                OnSetInventory?.Invoke(id, target.ItemCount);
            }
        }
    }

    public int AddItem(string id)
    {
        foreach (ItemModel item in PlayerModel.Inventory)
        {
            if (item.ItemID.Contains(id))
            {
                item.ItemCount++;
                OnSetInventory?.Invoke(id, item.ItemCount);
                return item.ItemCount;
            }
        }

        PlayerModel.Inventory.Add(SaveManager.Inst.AddDefaultItem(id));
        OnSetInventory?.Invoke(id, 1);
        return 1;
    }

    public void SetDay()
    {
        PlayerModel.Day++;
    }

    public void SetName(string player, string store)
    {
        PlayerModel.PlayerName = player;
        PlayerModel.StoreName = store;
    }
}
