using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 마법약 제작 콘텐츠 레시피 UI
/// </summary>

public class RecipePopup : UIBase
{
    [Header("버튼")]
    [SerializeField] Button Button_Left;
    [SerializeField] Button Button_Right;
    [SerializeField] Button Button_Confirm;

    [Header("레시피")]
    [SerializeField] GameObject RecipePanel;
    [SerializeField] Button Button_Recipe;
    [SerializeField] Image Image_Potion;
    [SerializeField] TextMeshProUGUI Text_PotionName;
    [SerializeField] TextMeshProUGUI Text_PotionDescription;
    [SerializeField] Transform ItemSlotParent;

    [Header("튜토리얼")]
    [SerializeField] GameObject TutorialPanel;
    [SerializeField] Image Image_Player;
    [SerializeField] Button Button_Tutorial;

    private int _currentIndex = 1;
    private int _maxPotionCount;
    private List<GameObject> _itemSlots = new List<GameObject>();
    private Dictionary<string, Potion> _data;

    private void Awake()
    {
        _data = GameDataManager.Inst.PotionDataList;
        _maxPotionCount = _data.Count;

        Button_Left.onClick.AddListener(OnClickLeftArrow);
        Button_Right.onClick.AddListener(OnClickRightArrow);
        Button_Confirm.onClick.AddListener(OnClickConfirm);
        Button_Recipe.onClick.AddListener(OnClickRecipe);
        Button_Tutorial.onClick.AddListener(OnClickTutorial);
    }

    private void OnEnable()
    {
        _currentIndex = 1;

        SetPotionInfo(_currentIndex).Forget();

        OnClickRecipe();

        GameUtil.LoadSpriteAndSet($"Character/{GameManager.Inst.PlayerModel.Gender}/Player_01_02", Image_Player).Forget();
    }

    // 화살표 버튼 관리
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

    // 마법약 레시피 설정
    private async UniTask SetPotionInfo(int index)
    {
        string potionID = GetPotionID(index);
        string path = $"Icon/Potion[{potionID}]";

        await GameUtil.LoadSpriteAndSet(path, Image_Potion);

        Text_PotionName.text = _data[potionID].Name;
        Text_PotionDescription.text = _data[potionID].Description;

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

        List<string> ingredient = _data[potionID].Ingredient;

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
        VisualNovelManager.Inst.OnStartTimer?.Invoke(true);

        UIManager.Inst.CloseRecipePopup();
    }

    private void OnClickRecipe()
    {
        Button_Tutorial.image.color = Color.grey;
        Button_Recipe.image.color = Color.white;

        RecipePanel.SetActive(true);
        TutorialPanel.SetActive(false);

        Button_Left.gameObject.SetActive(true);
        Button_Right.gameObject.SetActive(true);
    }

    private void OnClickTutorial()
    {
        Button_Tutorial.image.color = Color.white;
        Button_Recipe.image.color = Color.grey;

        RecipePanel.SetActive(false);
        TutorialPanel.SetActive(true);

        Button_Left.gameObject.SetActive(false);
        Button_Right.gameObject.SetActive(false);
    }

    private string GetPotionID(int index)
    {
        string potionID = $"Potion_{index}";

        return potionID;
    }
}
