using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogSlot : MonoBehaviour
{
    [SerializeField] private Image Image_Character;
    [SerializeField] private TextMeshProUGUI Text_Dialogue;

    public void InitSlot(string characterID, string currentID, bool isSameSpeaker)
    {
        if (Image_Character != null)
        {
            Image_Character.gameObject.SetActive(!isSameSpeaker);

            string path = $"Icon/Portrait[{characterID}]";
            GameUtil.LoadSpriteAndSet(path, Image_Character).Forget();
        }

        string content = GameDataManager.Inst.GetDialogueData(currentID).Content;
        Text_Dialogue.text = content;
    }
}