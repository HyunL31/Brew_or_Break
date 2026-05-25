using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClueSlot : MonoBehaviour
{
    [SerializeField] private Image Image_Clue;
    [SerializeField] private TextMeshProUGUI Text_Name;
    [SerializeField] private TextMeshProUGUI Text_Description;

    public void SetClueInfo(string id)
    {
        var data = GameDataManager.Inst.GetClueData(id);
        string name = data.Name;
        string description = data.Description;
        string path = $"{data.Path}";

        GameUtil.LoadSpriteAndSet(path, Image_Clue).Forget();
        Text_Name.text = name;
        Text_Description.text = description;
    }
}
