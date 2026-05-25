using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class VisualNovelUI : UIBase
{
    [SerializeField] private Image Image_Background;
    [SerializeField] private Image Image_Character;

    private void Awake()
    {
        VisualNovelManager.Inst.OnChangeBaseUI += (id) => ChangeBackgroundImage(id).Forget();
        VisualNovelManager.Inst.OnChangeBaseUI += (id) => ChangeSpeakerCharacter(id).Forget();
    }

    private async UniTask ChangeBackgroundImage(string id)
    {
        string background = GameDataManager.Inst.GetDialogueData(id).Background;
        string path = $"Background/{background}";

        await GameUtil.LoadSpriteAndSet(path, Image_Background);
    }

    private async UniTask ChangeSpeakerCharacter(string id)
    {
        string characterFacial = GameDataManager.Inst.GetDialogueData(id).Facial;

        if (characterFacial == string.Empty)
        {
            Image_Character.gameObject.SetActive(false);
        }
        else
        {
            Image_Character.gameObject.SetActive(true);

            string path = $"Character/{characterFacial}";

            await GameUtil.LoadSpriteAndSet(path, Image_Character);
        }
    }
}