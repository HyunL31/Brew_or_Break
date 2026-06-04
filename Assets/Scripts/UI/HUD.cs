using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 사냥 콘텐츠 UI
/// </summary>

public class HUD : UIBase
{
    [Header("스킬")]
    [SerializeField] private Transform SkillButtonRoot;
    [SerializeField] private Button SkillButtonPrefab;

    [Header("기타")]
    [SerializeField] private Button Button_Inventory;
    [SerializeField] private Button Button_EndCollecting;
    [SerializeField] private Image Image_Player;
    [SerializeField] private Image Image_Stamina;
    [SerializeField] private GameObject ItemKeyInfo;
    
    private Dictionary<string, SkillButton> _skills = new Dictionary<string, SkillButton>();
    private Dictionary<string, Skill> _data;

    private void Awake()
    {
        _data = GameDataManager.Inst.SkillDataList;

        Button_Inventory.onClick.AddListener(UIManager.Inst.OpenInventory);
        Button_EndCollecting.onClick.AddListener(EndCollecting);
        CollectingManager.Inst.OnChangeStamina = UpdateStamina;
        CollectingManager.Inst.OnEnterItem = (value) => ItemKeyInfo.SetActive(value);

        ItemKeyInfo.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            UIManager.Inst.OpenInventory();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            EndCollecting();
        }
    }

    private void OnEnable()
    {
        InitSkillButton();
        SetHUDImage();
    }

    // 플레이어 레벨에 따른 스킬 버튼 생성
    private void InitSkillButton()
    {
        int playerLevel = StoreManager.Inst.StoreModel.Level;

        foreach (var data in _data.Values)
        {
            if (data.Level <= playerLevel)
            {
                if (!_skills.ContainsKey(data.SkillName))
                {
                    Button button = Instantiate(SkillButtonPrefab, SkillButtonRoot);

                    SkillButton skillButton = button.GetComponent<SkillButton>();
                    skillButton.SetSkillInfo(data.ID);

                    _skills.Add(data.SkillName, skillButton);
                }
            }
            else
            {
                break;
            }
        }
    }

    // 플레이어 캐릭터 이미지 적용
    private void SetHUDImage()
    {
        Image_Stamina.fillAmount = 1;
        Image_Stamina.color = Color.green;

        string path = "Icon/Portrait[Girl_Player_01_04]";

        if (GameManager.Inst.PlayerModel.Gender == "Boy")
        {
            path = "Icon/Portrait[Boy_Player_01_04]";
        }

        GameUtil.LoadSpriteAndSet(path, Image_Player).Forget();
    }

    // 스태미나 UI 색상 변경
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

    private void EndCollecting()
    {
        CollectingManager.Inst.OnEndCollecting?.Invoke();

        GameManager.Inst.SetDay();
        UIManager.Inst.OpenLobbyUI();
        UIManager.Inst.CloseHUD();
    }
}
