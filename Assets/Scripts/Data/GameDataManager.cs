using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Inst;

    private void Awake()
    {
        Inst = this;

        GameUtil.LoadFullData();
    }

    [Serializable]
    private class SerializationWrapper<T>
    {
        public List<T> data;
    }

    public Dictionary<string, Character> CharacterDataList {  get; private set; } = new Dictionary<string, Character>();
    public Dictionary<string, Choice> ChoiceDataList { get; private set; } = new Dictionary<string, Choice>();
    public Dictionary<string, Clue> ClueDataList { get; private set; } = new Dictionary<string, Clue>();
    public Dictionary<string, Craft> CraftDataList { get; private set; } = new Dictionary<string, Craft>();
    public Dictionary<string, Dialogue> DialogueDataList { get; private set;} = new Dictionary<string, Dialogue>();
    public Dictionary<string, Ingredient> IngredientDataList { get; private set; } = new Dictionary<string, Ingredient>();
    public Dictionary<string, Monster> MonsterDataList { get; private set; } = new Dictionary<string, Monster>();
    public Dictionary<string, Potion> PotionDataList { get; private set; } = new Dictionary<string, Potion>();
    public Dictionary<string, Result> ResultDataList { get; private set; } = new Dictionary<string, Result>();

    private Dictionary<string, T> LoadData<T>(string tableName) where T : GameDataBase
    {
        string jsonPath = $"JsonOutput/{tableName}";

        TextAsset textAsset = Resources.Load<TextAsset>(jsonPath);

        if (textAsset == null)
        {
            Debug.Log($"Json 파일이 없음 ({tableName})");
            return new Dictionary<string, T>();
        }

        try
        {
            string jsonString = textAsset.text;

            string wrappedJson = "{\"data\":" + jsonString + "}";
            SerializationWrapper<T> wrapper = JsonUtility.FromJson<SerializationWrapper<T>>(wrappedJson);

            if (wrapper != null && wrapper.data != null)
            {
                Debug.Log($"{typeof(T).Name} 데이터를 {wrapper.data.Count}개 로드했습니다.");
                return wrapper.data.ToDictionary(data => data.ID.ToString());
            }
        }
        catch (Exception e)
        {
            Debug.Log($"오류오류 {e.Message}");
        }

        return new Dictionary<string, T>();
    }

    public void LoadCharacterData(string tableName)
    {
        CharacterDataList = LoadData<Character>(tableName);
    }

    public void LoadChoiceData(string tableName)
    {
        ChoiceDataList = LoadData<Choice>(tableName);
    }

    public void LoadClueData(string tableName)
    {
        ClueDataList = LoadData<Clue>(tableName);
    }

    public void LoadCraftData(string tableName)
    {
        CraftDataList = LoadData<Craft>(tableName);
    }

    public void LoadDialogueData(string tableName)
    {
        DialogueDataList = LoadData<Dialogue>(tableName);
    }

    public void LoadIngredientData(string tableName)
    {
        IngredientDataList = LoadData<Ingredient>(tableName);
    }

    public void LoadMonsterData(string tableName)
    {
        MonsterDataList = LoadData<Monster>(tableName);
    }

    public void LoadPotionData(string tableName)
    {
        PotionDataList = LoadData<Potion>(tableName);
    }

    public void LoadResultData(string tableName)
    {
        ResultDataList = LoadData<Result>(tableName);
    }

    public Character GetCharacterData(string id)
    {
        if (CharacterDataList == null || string.IsNullOrEmpty(id))
        {
            return null;
        }

        return CharacterDataList.TryGetValue(id, out var data) ? data : null;
    }

    public Choice GetChoiceData(string id)
    {
        if (ChoiceDataList == null || string.IsNullOrEmpty(id))
        {
            return null;
        }

        return ChoiceDataList.TryGetValue(id, out var data) ? data : null;
    }

    public Clue GetClueData(string id)
    {
        if (ClueDataList == null || string.IsNullOrEmpty(id))
        {
            return null;
        }

        return ClueDataList.TryGetValue(id, out var data) ? data : null;
    }

    public Craft GetCraftData(string id)
    {
        if (CraftDataList == null || string.IsNullOrEmpty(id))
        {
            return null;
        }

        return CraftDataList.TryGetValue(id, out var data) ? data : null;
    }

    public Dialogue GetDialogueData(string id)
    {
        if (DialogueDataList == null || string.IsNullOrEmpty(id))
        {
            return null;
        }

        return DialogueDataList.TryGetValue(id, out var data) ? data : null;
    }

    public Ingredient GetIngredientData(string id)
    {
        if (IngredientDataList == null || string.IsNullOrEmpty(id))
        {
            return null;
        }

        return IngredientDataList.TryGetValue(id, out var data) ? data : null;
    }

    public Monster GetMonsterData(string id)
    {
        if (MonsterDataList == null || string.IsNullOrEmpty(id))
        {
            return null;
        }

        return MonsterDataList.TryGetValue(id, out var data) ? data : null;
    }

    public Potion GetPotionData(string id)
    {
        if (PotionDataList == null || string.IsNullOrEmpty(id))
        {
            return null;
        }

        return PotionDataList.TryGetValue(id, out var data) ? data : null;
    }

    public Result GetResultData(string id)
    {
        if (ResultDataList == null || string.IsNullOrEmpty(id))
        {
            return null;
        }

        return ResultDataList.TryGetValue(id, out var data) ? data : null;
    }
}