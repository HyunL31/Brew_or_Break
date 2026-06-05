using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 마법약 제작 UI 컴포넌트
/// </summary>

public class CraftUI : UIBase
{
    [Header("냄비")]
    [SerializeField] private Button Button_Confirm;
    [SerializeField] private Button Button_Next;
    [SerializeField] private Image Image_Fire;
    [SerializeField] private Image Image_Smoke;
    [SerializeField] private Image Image_Pot;
    [SerializeField] private Animator Anim;

    [Header("산도")]
    [SerializeField] private TextMeshProUGUI Text_Acidity;
    [SerializeField] private Image Image_Litmus;
    [SerializeField] private Slider Slider_Acidity;

    [SerializeField] private GameObject Stopwatch;

    private int _randomAcidity = 0;
    private List<string> _addedItem = new List<string>();
    private string _potionID = string.Empty;
    private Dictionary<string, Potion> _potionData;
    private Dictionary<string, Craft> _craftData;
    private Dictionary<string, Ingredient> _ingredientData;

    private void Awake()
    {
        Slider_Acidity.minValue = 0;
        Slider_Acidity.maxValue = 14;

        Button_Confirm.onClick.AddListener(OnClickConfirm);
        Button_Next.onClick.AddListener(OnClickNext);
        Slider_Acidity.onValueChanged.AddListener((value) => SetLitmusColor(value));

        VisualNovelManager.Inst.OnDropItem = (itemID) => AddItem(itemID);
        VisualNovelManager.Inst.OnEndTimer = ShowPotion;
        VisualNovelManager.Inst.OnStartTimer = (play) => Stopwatch.SetActive(play);
        Debug.Log("연결 완");
    }

    private void OnEnable()
    {
        Slider_Acidity.value = 0;

        Image_Fire.gameObject.SetActive(true);
        Image_Fire.color = Color.white;
        Image_Smoke.gameObject.SetActive(true);
        Image_Smoke.color = Color.white;
        Button_Next.gameObject.SetActive(false);

        Text_Acidity.text = SetRandomAcidity();

        Image_Litmus.color = Color.red;

        string path = "Icon/Pot2";
        GameUtil.LoadSpriteAndSet(path, Image_Pot).Forget();

        Anim.gameObject.SetActive(false);
    }

    private void Start()
    {
        _potionData = GameDataManager.Inst.PotionDataList;
        _craftData = GameDataManager.Inst.CraftDataList;
        _ingredientData = GameDataManager.Inst.IngredientDataList;
    }

    private string SetRandomAcidity()
    {
        int random = UnityEngine.Random.Range(0, 15);
        _randomAcidity = random;

        return $"pH {random}";
    }

    private void SetLitmusColor(float value)
    {
        // 빨강 -> 파랑이 슬라이더 값에 따라 균일하게 분포하도록 만드는 정규화 공식
        float normalValue = (value - Slider_Acidity.minValue) / (Slider_Acidity.maxValue - Slider_Acidity.minValue);

        Color color = Color.Lerp(Color.red, Color.blue, normalValue);
        Image_Litmus.color = color;
    }

    private void OnClickConfirm()
    {
        ShowPotion();
        _addedItem.Clear();
    }

    private void OnClickNext()
    {
        UIManager.Inst.OpenDialogueUI();
        UIManager.Inst.CloseCraftUI();
    }

    private void ShowPotion()
    {
        Image_Fire.gameObject.SetActive(false);
        Image_Smoke.gameObject.SetActive(false);

        _potionID = GetMadePotionID();
        string path = $"Icon/Potion[{_potionID}]";

        Anim.gameObject.SetActive(true);

        string effect = string.Empty;
        if (_potionID == "Mess")
        {
            effect = "Fail";
        }
        else
        {
            effect = "Success";
        }

        Stopwatch.SetActive(false);
        PlayPotionEffect(effect).Forget();

        GameUtil.LoadSpriteAndSet(path, Image_Pot).Forget();
        GetReturnID(_potionID);

        Button_Next.gameObject.SetActive(true);
    }

    private bool CheckAcidityValue()
    {
        // -1 ~ +1까지의 범위 체크
        if (Mathf.Abs(Slider_Acidity.value - _randomAcidity) <= 1f)
        {
            return true;
        }
        
        return false;
    }

    private string GetMadePotionID()
    {
        if (!CheckAcidityValue())
        {
            SoundManager.Inst.OnSFX?.Invoke("Audio/Fail");
            return "Mess";
        }

        bool isRight = false;

        foreach (var item in _potionData.Values)
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
                SoundManager.Inst.OnSFX?.Invoke("Audio/Success");
                return item.ID;
            }
        }

        SoundManager.Inst.OnSFX?.Invoke("Audio/Fail");
        return "Mess";
    }

    // 아이템 추가 시 마법약 색 변화
    private void AddItem(string id)
    {
        List<float> RGB = _ingredientData[id].RGB;

        Color color = new Color(RGB[0], RGB[1], RGB[2]);

        Image_Fire.color = color;
        Image_Smoke.color = color;

        _addedItem.Add(id);

        GameManager.Inst.UseItem(id);
    }

    private void GetReturnID(string potionID)
    {
        string id = VisualNovelManager.Inst.CurrentDialogueID;

        List<string> potions = _craftData[id].PotionID;
        string returnID = string.Empty;

        for (int i = 0; i < potions.Count; i++)
        {
            if (potions[i] == potionID)
            {
                returnID = _craftData[id].SuccessID[i];
                break;
            }
        }

        if (returnID == string.Empty)
        {
            returnID = _craftData[id].FailID;
        }

        VisualNovelManager.Inst.OnSetDialogueID(returnID);
    }

    // 마법약 이펙트
    private async UniTask PlayPotionEffect(string effect)
    {
        Anim.SetTrigger(effect);

        await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: this.GetCancellationTokenOnDestroy());

        Anim.gameObject.SetActive(false);
    }
}