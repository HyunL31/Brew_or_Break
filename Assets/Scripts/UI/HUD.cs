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
    
    private List<SkillButton> _skills = new List<SkillButton>();

    private void Awake()
    {
        Button_Inventory.onClick.AddListener(UIManager.Inst.OpenInventory);
    }

    private void Start()
    {
        InitSkillButton();
    }

    private void InitSkillButton()
    {
        int playerLevel = StoreManager.Inst.GetStoreLevel();
        Debug.Log(playerLevel);
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
}
