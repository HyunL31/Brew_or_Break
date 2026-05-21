using UnityEngine;
using UnityEngine.UI;

public static class GameUtil
{
    public static void LoadFullData()
    {
        GameDataManager.Inst.LoadCharacterData("Character");
        GameDataManager.Inst.LoadChoiceData("Choice");
        GameDataManager.Inst.LoadClueData("Clue");
        GameDataManager.Inst.LoadCraftData("Craft");
        GameDataManager.Inst.LoadDialogueData("Dialogue");
        GameDataManager.Inst.LoadIngredientData("Ingredient");
        GameDataManager.Inst.LoadMonsterData("Monster");
        GameDataManager.Inst.LoadPotionData("Potion");
        GameDataManager.Inst.LoadResultData("Result");
    }

    public static void LoadSpriteAndSet(string path, Image imageObject)
    {
        ResourceManager.Inst.LoadSprite(path, (sprite) =>
        {
            imageObject.sprite = sprite;
        });
    }

    public static AudioClip LoadSoundAndSet(string clipName, AudioSource audio)
    {
        AudioClip audioClip = null;

        string path = $"Audio/{clipName}";
        ResourceManager.Inst.LoadAsset<AudioClip>(path, (clip) =>
        {
            audio.clip = clip;
            audioClip = clip;
        });

        return audioClip;
    }
}

public static class MaxStoreStatus
{
    public static int MaxLevel = 10;
    public static int MaxReputation = 500;
    public static int MaxCompensation = 10000;
}
