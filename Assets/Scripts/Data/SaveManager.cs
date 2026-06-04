using System;
using System.IO;
using UnityEngine;

/// <summary>
/// 저장 매니저
/// </summary>

public class SaveManager : MonoBehaviour
{
    public static SaveManager Inst;

    public Action OnSaveClear;

    private void Awake()
    {
        Inst = this;
    }

    private string GetPath(int slotIndex)
    {
        return Path.Combine(Application.persistentDataPath, $"BrewOrBreak{slotIndex}.json");
    }

    public void RequestSaveData(int slotIndex, PlayerModel data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetPath(slotIndex), json);
    }

    public PlayerModel RequestLoadData(int slotIndex)
    {
        string path = GetPath(slotIndex);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerModel data = JsonUtility.FromJson<PlayerModel>(json);

            return data;
        }
        else
        {
            var playerData = GetDefaultData();
            return playerData;
        }
    }

    public PlayerModel GetDefaultData()
    {
        var newPlayerData = new PlayerModel();
        newPlayerData.PlayerName = "";
        newPlayerData.StoreName = "";
        newPlayerData.Day = 0;
        newPlayerData.Gender = "Girl";
        newPlayerData.IsComplete = false;
        newPlayerData.EndingID = "";

        newPlayerData.Store.Level = 1;
        newPlayerData.Store.Gold = 0;
        newPlayerData.Store.Reputation = 0;
        newPlayerData.Store.Compensation = 6000;

        newPlayerData.Inventory.Add(AddDefaultItem("Item_01"));
        newPlayerData.Inventory.Add(AddDefaultItem("Item_02"));
        newPlayerData.Inventory.Add(AddDefaultItem("Item_04"));
        newPlayerData.Inventory.Add(AddDefaultItem("Item_06"));
        newPlayerData.Inventory.Add(AddDefaultItem("Item_08"));

        return newPlayerData;
    }

    public ItemModel AddDefaultItem(string itemID)
    {
        var item = new ItemModel();
        item.ItemID = itemID;
        item.ItemCount = 1;

        return item;
    }

    public bool HasSaveFile(int slotIndex)
    {
        return File.Exists(GetPath(slotIndex));
    }
}
