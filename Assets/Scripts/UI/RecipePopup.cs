using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipePopup : UIBase
{
    [Header("버튼")]
    [SerializeField] Button Button_Left;
    [SerializeField] Button Button_Right;
    [SerializeField] Button Button_Confirm;

    [Header("콘텐츠")]
    [SerializeField] Image Image_Potion;
    [SerializeField] TextMeshProUGUI Text_PotionName;
    [SerializeField] TextMeshProUGUI Text_PotionDescription;
    [SerializeField] Transform ItemSlotParent;

    private int _currentIndex = 1;
    private int _maxPotionCount;
    private List<GameObject> _itemSlots = new List<GameObject>();

    private void Awake()
    {
        _maxPotionCount = GameDataManager.Inst.PotionDataList.Count;

        Button_Left.onClick.AddListener(OnClickLeftArrow);
        Button_Right.onClick.AddListener(OnClickRightArrow);
        Button_Confirm.onClick.AddListener(OnClickConfirm);

        SetPotionInfo(_currentIndex).Forget();
    }

    private void OnEnable()
    {
        _currentIndex = 1;
    }

    private void OnClickLeftArrow()
    {
        if (_currentIndex < 1)
        {
            return;
        }

        _currentIndex--;
        Button_Right.gameObject.SetActive(true);

        if (_currentIndex == 1)
        {
            Button_Left.gameObject.SetActive(false);
        }

        SetPotionInfo(_currentIndex).Forget();
    }

    private void OnClickRightArrow()
    {
        if (_currentIndex >= _maxPotionCount)
        {
            return;
        }

        _currentIndex++;
        Button_Left.gameObject.SetActive(true);

        if (_currentIndex == _maxPotionCount)
        {
            Button_Right.gameObject.SetActive(false);
        }

        SetPotionInfo(_currentIndex).Forget();
    }

    private async UniTask SetPotionInfo(int index)
    {
        string potionID = GetPotionID(index);
        string path = $"Icon/Potion[{potionID}]";

        await GameUtil.LoadSpriteAndSet(path, Image_Potion);

        var data = GameDataManager.Inst.GetPotionData(potionID);
        
        Text_PotionName.text = data.Name;
        Text_PotionDescription.text = data.Description;

        await SetPotionIngredient(index);
    }

    private async UniTask SetPotionIngredient(int index)
    {
        foreach(GameObject slot in _itemSlots)
        {
            Destroy(slot);
        }

        _itemSlots.Clear();

        string potionID = GetPotionID(index);
        string path = "Prefabs/UI/InventorySlot";

        List<string> ingredient = GameDataManager.Inst.GetPotionData(potionID).Ingredient;

        foreach(string item in ingredient)
        {
            GameObject prefab = await ResourceManager.Inst.InstantiatePrefab(path, ItemSlotParent);

            InventorySlot slot = prefab.GetComponent<InventorySlot>();
            slot.SetSlotInfo(item);

            _itemSlots.Add(prefab);
        }
    }

    private void OnClickConfirm()
    {
        UIManager.Inst.CloseRecipePopup();
    }

    private string GetPotionID(int index)
    {
        string potionID = string.Empty;

        if (index < 10)
        {
            potionID = $"Potion_0{index}";
        }
        else
        {
            potionID = $"Potion_{index}";
        }

        return potionID;
    }
}
