using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClueSlot : MonoBehaviour
{
    [SerializeField] private Image Image_Clue;
    [SerializeField] private TextMeshProUGUI Text_Name;
    [SerializeField] private TextMeshProUGUI Text_Description;

    private Dictionary<string, Clue> _data;

    private void Awake()
    {
        _data = GameDataManager.Inst.ClueDataList;
    }

    public void SetClueInfo(string id)
    {
        string name = _data[id].Name;
        string description = _data[id].Description;
        string path = $"{_data[id].Path}";

        GameUtil.LoadSpriteAndSet(path, Image_Clue).Forget();
        Text_Name.text = name;
        Text_Description.text = description;
    }
}
