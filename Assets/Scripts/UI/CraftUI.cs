using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftUI : UIBase
{
    [SerializeField] private Button Button_Confirm;
    [SerializeField] private Image Image_Fire;
    [SerializeField] private Image Image_Smoke;

    private List<string> _addedItem = new List<string>();

    private void Awake()
    {
        Button_Confirm.onClick.AddListener(OnClickConfirm);
        VisualNovelManager.Inst.OnDropItem = (itemID) => AddItem(itemID);
    }

    private void OnClickConfirm()
    {

    }

    private void CheckAnswer()
    {
        Image_Fire.gameObject.SetActive(false);
        Image_Smoke.gameObject.SetActive(false);
    }

    private void AddItem(string id)
    {
        List<float> RGB = GameDataManager.Inst.GetIngredientData(id).RGB;

        Color color = new Color(RGB[0], RGB[1], RGB[2]);

        Image_Fire.color = color;
        Image_Smoke.color = color;

        _addedItem.Add(id);
    }
}