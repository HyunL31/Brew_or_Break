using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 대화 로그 슬롯
/// </summary>

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

            if (characterID.Contains("Player"))
            {
                if (GameManager.Inst.PlayerModel.Gender == "Girl")
                {
                    path = $"Icon/Portrait[Girl_{characterID}]";
                }
                else
                {
                    path = $"Icon/Portrait[Boy_{characterID}]";
                }
            }

            Debug.Log(path);
            GameUtil.LoadSpriteAndSet(path, Image_Character).Forget();
        }

        string content = GameDataManager.Inst.GetDialogueData(currentID).Content;
        Text_Dialogue.text = content;
    }
}