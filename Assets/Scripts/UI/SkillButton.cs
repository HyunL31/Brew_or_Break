using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum SkillType
{
    None,
    Basic,
    Projectile,
    Overlap
}

/// <summary>
/// 스킬 유형별 버튼
/// </summary>

public class SkillButton : MonoBehaviour
{
    [SerializeField] private Button Button_Skill;
    [SerializeField] private Image Image_Skill;
    [SerializeField] private TextMeshProUGUI Text_Skill;
    [SerializeField] private TextMeshProUGUI Text_Key;

    private string _skillID;
    private int _ATK;
    private float _stamina;
    private Dictionary<string, Skill> _data;
    private KeyCode _mainKeyCode;
    private KeyCode _keypadCode;

    private void Awake()
    {
        _data = GameDataManager.Inst.SkillDataList;
    }

    private void Start()
    {
        RegistClickEvent(_skillID);

        _ATK = _data[_skillID].ATK;
        _stamina = _data[_skillID].Stamina;
    }

    private void Update()
    {
        if (Input.GetKeyDown(_mainKeyCode) || Input.GetKeyDown(_keypadCode))
        {
            Button_Skill.onClick.Invoke();
        }
    }

    public void SetSkillInfo(string id)
    {
        _skillID = id;

        Text_Skill.text = _data[id].SkillName;
        Text_Key.text = $"{_data[id].Level}";

        string path = $"Icon/Skill[{id}]";
        GameUtil.LoadSpriteAndSet(path, Image_Skill).Forget();
    }

    // 버튼 이벤트 등록
    private void RegistClickEvent(string id)
    {
        string skillType = _data[id].Type;
        SkillType type = Enum.Parse<SkillType>(skillType);

        switch (type)
        {
            case SkillType.Basic:
                Button_Skill.onClick.AddListener(OnClickBasicSkill);
                break;

            case SkillType.Projectile:
                Button_Skill.onClick.AddListener(OnClickProjectileSkill);
                break;

            case SkillType.Overlap:
                Button_Skill.onClick.AddListener(OnClickOverlapSkill);
                break;
        }

        Debug.Log(_skillID);
        _mainKeyCode = KeyCode.Alpha0 + _data[_skillID].Level;
        _keypadCode = KeyCode.Keypad0 + _data[_skillID].Level;
    }

    // 스킬 사용
    private void OnClickBasicSkill()
    {
        CollectingManager.Inst.GetPlayer().UseBasicSkill(_ATK, _stamina);
    }

    private void OnClickProjectileSkill()
    {
        string animType = _data[_skillID].Anim;
        Enum.TryParse(animType, out ProjectileType projectileType);

        CollectingManager.Inst.GetPlayer().UseProjectileSkill(projectileType, _ATK, _stamina);
    }

    private void OnClickOverlapSkill()
    {
        string animType = _data[_skillID].Anim;
        Enum.TryParse(animType, out OverlapType overlapType);

        CollectingManager.Inst.GetPlayer().UseOverlapSkill(overlapType, _ATK, _stamina);
    }
}
