using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public static class GameUtil
{
    // 데이터 드리븐 로드
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
        GameDataManager.Inst.LoadSkillData("Skill");
    }

    // 스프라이트 로드 & 적용 (이미지)
    public static async UniTask<Sprite> LoadSpriteAndSet(string path, Image imageObject)
    {
        Sprite sprite = await ResourceManager.Inst.LoadSprite(path);

        if (sprite != null)
        {
            imageObject.sprite = sprite;
        }

        return sprite;
    }

    // 텍스처 로드 & 적용 (마우스 커서 등)
    public static async UniTask<Sprite> LoadTextureAndSet(string path, SpriteRenderer spriteRender)
    {
        Sprite sprite = await ResourceManager.Inst.LoadSprite(path);

        if (sprite != null)
        {
            spriteRender.sprite = sprite;
        }

        return sprite;
    }

    // 사운드 로드 & 적용
    public static async UniTask<AudioClip> LoadSoundAndSet(string path, AudioSource audio)
    {
        AudioClip audioClip = await ResourceManager.Inst.LoadAsset<AudioClip>(path);

        if (audioClip != null)
        {
            audio.clip = audioClip;
        }

        return audioClip;
    }

    // 스프라이트를 로드만 하는 메서드
    public static async UniTask<Sprite> LoadSpriteOnly(string path)
    {
        return await ResourceManager.Inst.LoadSprite(path);
    }
}
