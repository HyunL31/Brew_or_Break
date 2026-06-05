using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 매니저
/// </summary>

public class GameManager : MonoBehaviour
{
    public static GameManager Inst;

    public PlayerModel PlayerModel { get; private set; } = new PlayerModel();
    public int CurrentSlotIndex { get; private set; } = 0;
    public HashSet<int> SlotIndex { get; private set; } = new HashSet<int>();
    public bool IsOpeningStore { get; private set; } = true;

    public Action<string, int> OnSetInventory;
    public Action<float> OnChangeBrightness;
    public Action<int> OnStartGame;

    private void Awake()
    {
        Inst = this;

        // 모든 저장 파일 로드
        for (int i = 0; i < 100; i++)
        {
            if (SaveManager.Inst.HasSaveFile(i))
            {
                SlotIndex.Add(i);
            }
        }

        OnStartGame += (index) =>
        {
            SetCurrentSaveIndex(index);
            
            if (SaveManager.Inst.HasSaveFile(index))
            {
                LoadData(index);
            }
            else
            {
                LoadDefaultData();
                SaveData();
            }

            StoreManager.Inst.StoreInit();
        };
    }

    // 저장
    public void SaveData()
    {
        SaveManager.Inst.RequestSaveData(CurrentSlotIndex, PlayerModel);
        SlotIndex.Add(CurrentSlotIndex);
    }

    public void LoadData(int index)
    {
        PlayerModel = SaveManager.Inst.RequestLoadData(index);
    }

    public void LoadDefaultData()
    {
        PlayerModel = SaveManager.Inst.GetDefaultData();
    }

    // 아이템 사용 및 획득
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

    // 플레이어 정보 설정
    public void SetDay()
    {
        PlayerModel.Day++;
        SaveData();
    }

    public void SetName(string player, string store)
    {
        PlayerModel.PlayerName = player;
        PlayerModel.StoreName = store;
    }

    // 저장 파일 인덱스 설정
    public void SetCurrentSaveIndex(int index)
    {
        CurrentSlotIndex = index;
    }

    public int GetEmptySlotIndex()
    {
        for (int i = 0; i < 100; i++)
        {
            if (!SlotIndex.Contains(i))
            {
                return i;
            }
        }

        return 0;
    }

    public void IsOpenStore(bool isLobby)
    {
        IsOpeningStore = isLobby;
    }
}
