using UnityEngine;

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
}
