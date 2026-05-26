using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : UIBase
{
    [Header("스킬")]
    [SerializeField] private Transform SkillButtonRoot;
    [SerializeField] private Button SkillButtonPrefab;

    [Header("기타")]
    [SerializeField] private Button Button_Inventory;
    [SerializeField] private Image Image_Stamina;
    
    private List<SkillButton> _skills = new List<SkillButton>();

    private void Awake()
    {
        Button_Inventory.onClick.AddListener(UIManager.Inst.OpenInventory);
        CollectingManager.Inst.OnChangeStamina = UpdateStamina;
    }

    private void OnEnable()
    {
        Image_Stamina.fillAmount = 1;
        Image_Stamina.color = Color.green;
    }

    private void Start()
    {
        InitSkillButton();
    }

    private void InitSkillButton()
    {
        int playerLevel = StoreManager.Inst.GetStoreLevel();
        var datas = GameDataManager.Inst.SkillDataList.Values;

        foreach (var data in datas)
        {
            if (data.Level <= playerLevel)
            {
                Button button = Instantiate(SkillButtonPrefab, SkillButtonRoot);
                
                SkillButton skillButton = button.GetComponent<SkillButton>();
                skillButton.SetSkillInfo(data.ID);

                _skills.Add(skillButton);
            }
            else
            {
                break;
            }
        }
    }

    private void UpdateStamina(float stamina, float maxStamina)
    {
        Image_Stamina.fillAmount = stamina / maxStamina;

        if(Image_Stamina.fillAmount >= 0.6f)
        {
            Image_Stamina.color = Color.green;
        }
        else if (Image_Stamina.fillAmount >= 0.3f)
        {
            Image_Stamina.color = Color.yellow;
        }
        else if (Image_Stamina.fillAmount > 0)
        {
            Image_Stamina.color = Color.red;
        }
    }
}
