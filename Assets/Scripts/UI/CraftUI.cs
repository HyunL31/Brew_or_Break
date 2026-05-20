using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftUI : UIBase
{
    [Header("냄비")]
    [SerializeField] private Button Button_Confirm;
    [SerializeField] private Image Image_Fire;
    [SerializeField] private Image Image_Smoke;
    [SerializeField] private Image Image_Pot;

    [Header("산도")]
    [SerializeField] private TextMeshProUGUI Text_Acidity;
    [SerializeField] private Image Image_Litmus;
    [SerializeField] private Slider Slider_Acidity;

    private int _randomAcidity;
    private List<string> _addedItem = new List<string>();

    private void Awake()
    {
        Slider_Acidity.minValue = 0;
        Slider_Acidity.maxValue = 14;

        Button_Confirm.onClick.AddListener(OnClickConfirm);
        Slider_Acidity.onValueChanged.AddListener((value) => SetLitmusColor(value));
        VisualNovelManager.Inst.OnDropItem = (itemID) => AddItem(itemID);
    }

    private void OnEnable()
    {
        Text_Acidity.text = SetRandomAcidity();

        Image_Litmus.color = Color.red;

        string path = "Icon/Pot2";
        GameUtil.LoadSpriteAndSet(path, Image_Pot);
    }

    private string SetRandomAcidity()
    {
        int random = Random.Range(0, 15);
        _randomAcidity = random;

        return $"pH {random}";
    }

    private void SetLitmusColor(float value)
    {
        float normalValue = (value - Slider_Acidity.minValue) / (Slider_Acidity.maxValue - Slider_Acidity.minValue);

        Color color = Color.Lerp(Color.red, Color.blue, normalValue);
        Image_Litmus.color = color;
    }

    private void OnClickConfirm()
    {
        ShowPotion();
        _addedItem.Clear();
    }

    private void ShowPotion()
    {
        Image_Fire.gameObject.SetActive(false);
        Image_Smoke.gameObject.SetActive(false);

        string path = $"Icon/Potion[{GetMadePotionID()}]";

        GameUtil.LoadSpriteAndSet(path, Image_Pot);
    }

    private bool CheckAcidityValue()
    {
        if (Slider_Acidity.value <= _randomAcidity + 1f && Slider_Acidity.value >= _randomAcidity - 1f)
        {
            return true;
        }
        
        return false;
    }

    private string GetMadePotionID()
    {
        if (!CheckAcidityValue())
        {
            return "Mess";
        }

        var data = GameDataManager.Inst.PotionDataList.Values;
        bool isRight = false;

        foreach (var item in data)
        {
            List<string> items = item.Ingredient;

            if (items.Count != _addedItem.Count)
            {
                isRight = false;
                continue;
            }

            foreach (string addedItem in _addedItem)
            {
                if (items.Contains(addedItem))
                {
                    isRight = true;
                }
                else
                {
                    isRight = false;
                    break;
                }
            }

            if (isRight)
            {
                return item.ID;
            }
        }

        return "Mess";
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