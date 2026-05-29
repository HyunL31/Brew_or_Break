using System;
using System.Collections.Generic;

[Serializable]
public class PlayerModel
{
    public string PlayerName;
    public string StoreName;
    public int Day;
    public bool IsComplete;
    public StoreModel Store = new StoreModel();
    public List<ItemModel> Inventory = new List<ItemModel>();
}

[Serializable]
public class ItemModel
{
    public string ItemID;
    public int ItemCount;
}

[Serializable]
public class StoreModel
{
    public int Level;
    public int Gold;
    public int Reputation;
    public int Compensation;
}
