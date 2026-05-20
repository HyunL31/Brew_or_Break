using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerModel
{
    public string PlayerName;
    public string StoreName;
    public int Day;
    public int StoreLevel;
    public int Gold;
    public int Reputation;
    public int Compensation;
    public List<ItemModel> Inventory = new List<ItemModel>();
}

[Serializable]
public class ItemModel
{
    public string ItemID;
    public int ItemCount;
}
