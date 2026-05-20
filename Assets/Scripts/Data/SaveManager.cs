using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Inst;

    private void Awake()
    {
        Inst = this;
    }

    private string GetPath()
    {
        return Path.Combine(Application.persistentDataPath, "BrewOrBreak.json");
    }

    public void RequestSaveData(PlayerModel data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetPath(), json);
    }

    public PlayerModel RequestLoadData()
    {
        string path = GetPath();

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerModel data = JsonUtility.FromJson<PlayerModel>(json);

            return data;
        }
        else
        {
            var playerData = RequestLoadDefaultData();
            return playerData;
        }
    }

    public PlayerModel RequestLoadDefaultData()
    {
        var playerData = GetDefaultData();
        RequestSaveData(playerData);
        return playerData;
    }

    public PlayerModel GetDefaultData()
    {
        var newPlayerData = new PlayerModel();
        newPlayerData.PlayerName = "";
        newPlayerData.StoreName = "";
        newPlayerData.Day = 0;
        newPlayerData.StoreLevel = 0;
        newPlayerData.Gold = 0;
        newPlayerData.Reputation = 0;
        newPlayerData.Compensation = 1000;

        newPlayerData.Inventory.Add(AddDefaultItem("Item_01"));
        newPlayerData.Inventory.Add(AddDefaultItem("Item_02"));
        newPlayerData.Inventory.Add(AddDefaultItem("Item_04"));
        newPlayerData.Inventory.Add(AddDefaultItem("Item_06"));
        newPlayerData.Inventory.Add(AddDefaultItem("Item_08"));

        return newPlayerData;
    }

    private ItemModel AddDefaultItem(string itemID)
    {
        var item = new ItemModel();
        item.ItemID = itemID;
        item.ItemCount = 1;

        return item;
    }
}
