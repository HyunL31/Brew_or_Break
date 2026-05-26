using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst;

    private PlayerModel _playerModel = new PlayerModel();
    public Action<string> OnSetInventory;

    private void Awake()
    {
        Inst = this;
    }

    public void SaveData()
    {
        SaveManager.Inst.RequestSaveData(_playerModel);
    }

    public void LoadData()
    {
        _playerModel = SaveManager.Inst.RequestLoadData();
    }

    public void LoadDefaultData()
    {
        _playerModel = SaveManager.Inst.RequestLoadDefaultData();
    }

    public List<ItemModel> GetInventory()
    {
        return _playerModel.Inventory;
    }

    public void UseItem(string id)
    {
        ItemModel target = null;

        foreach (ItemModel item in _playerModel.Inventory)
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
                OnSetInventory?.Invoke(id);
                _playerModel.Inventory.Remove(target);
            }
        }
    }

    public int AddItem(string id)
    {
        foreach (ItemModel item in _playerModel.Inventory)
        {
            if (item.ItemID.Contains(id))
            {
                item.ItemCount++;
                OnSetInventory?.Invoke(id);
                return item.ItemCount;
            }
        }

        _playerModel.Inventory.Add(SaveManager.Inst.AddDefaultItem(id));
        OnSetInventory?.Invoke(id);
        return 1;
    }

    public int GetDay()
    {
        return _playerModel.Day;
    }

    public void SetDay()
    {
        _playerModel.Day++;
    }

    public string GetPlayerName()
    {
        return _playerModel.PlayerName;
    }

    public string GetStoreName()
    {
        return _playerModel.StoreName;
    }

    public void SetName(string player, string store)
    {
        _playerModel.PlayerName = player;
        _playerModel.StoreName = store;
    }

    public StoreModel GetStoreModel()
    {
        return _playerModel.Store;
    }
}
