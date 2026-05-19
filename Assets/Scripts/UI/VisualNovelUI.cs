using UnityEngine;
using UnityEngine.UI;

public class VisualNovelUI : UIBase
{
    [SerializeField] private Image Image_Background;
    [SerializeField] private Image Image_Character;

    private void Awake()
    {
        VisualNovelManager.Inst.OnChangeBaseUI += ChangeBackgroundImage;
        VisualNovelManager.Inst.OnChangeBaseUI += ChangeSpeakerCharacter;
    }

    private void ChangeBackgroundImage(string id)
    {
        string background = GameDataManager.Inst.GetDialogueData(id).Background;
        string path = $"Background/{background}";

        GameUtil.LoadSpriteAndSet(path, Image_Background);
    }

    private void ChangeSpeakerCharacter(string id)
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

            GameUtil.LoadSpriteAndSet(path, Image_Character);
        }
    }
}