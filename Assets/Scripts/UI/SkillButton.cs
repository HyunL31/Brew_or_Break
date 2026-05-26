using Cysharp.Threading.Tasks;
using System;
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

public class SkillButton : MonoBehaviour
{
    [SerializeField] private Button Button_Skill;
    [SerializeField] private Image Image_Skill;
    [SerializeField] private TextMeshProUGUI Text_Skill;

    private string _skillID;
    private int _ATK;
    private float _stamina;

    private void Start()
    {
        RegistClickEvent(_skillID);

        _ATK = CollectingManager.Inst.SetSkillATK(_skillID);
        _stamina = GameDataManager.Inst.GetSkillData(_skillID).Stamina;
    }

    public void SetSkillInfo(string id)
    {
        _skillID = id;

        Text_Skill.text = GameDataManager.Inst.GetSkillData(id).SkillName;

        string path = $"Icon/Skill[{id}]";
        GameUtil.LoadSpriteAndSet(path, Image_Skill).Forget();
    }

    private void RegistClickEvent(string id)
    {
        string skillType = GameDataManager.Inst.GetSkillData(id).Type;
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
    }

    private void OnClickBasicSkill()
    {
        CollectingManager.Inst.GetPlayer().UseBasicSkill(_ATK, _stamina);
    }

    private void OnClickProjectileSkill()
    {
        string animType = GameDataManager.Inst.GetSkillData(_skillID).Anim;
        Enum.TryParse(animType, out ProjectileType projectileType);

        CollectingManager.Inst.GetPlayer().UseProjectileSkill(projectileType, _ATK, _stamina);
    }

    private void OnClickOverlapSkill()
    {
        string animType = GameDataManager.Inst.GetSkillData(_skillID).Anim;
        Enum.TryParse(animType, out OverlapType overlapType);

        CollectingManager.Inst.GetPlayer().UseOverlapSkill(overlapType, _ATK, _stamina);
    }
}
