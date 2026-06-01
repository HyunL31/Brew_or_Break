using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 아이템 설명창 UI
/// </summary>

public class ItemDescription : UIBase
{
    [SerializeField] TextMeshProUGUI Text_ItemName;
    [SerializeField] TextMeshProUGUI Text_ItemDescription;

    private Dictionary<string, Ingredient> _data;

    private void Awake()
    {
        _data = GameDataManager.Inst.IngredientDataList;
    }

    // 아이템 슬롯 바로 옆으로 이동
    public void SetPosition(RectTransform rect)
    {
        Vector3 targetPos = rect.position + new Vector3(rect.rect.width + 100f, 0, 0);

        this.transform.position = targetPos;
    }

    public void SetItemInfo(string id)
    {
        string name = _data[id].Name;
        string decription = _data[id].Description;

        Text_ItemName.text = name;
        Text_ItemDescription.text = decription;
    }
}
