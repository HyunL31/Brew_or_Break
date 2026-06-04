using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 사냥 콘텐츠 HP 바
/// </summary>

public class HPBar : MonoBehaviour
{
    [SerializeField] private Image Image_HP;
    [SerializeField] private float Yoffset = 1f;
    [SerializeField] private RectTransform _rect;

    private Transform _target;

    private void LateUpdate()
    {
        CollectingManager.Inst.SetHUDPos(_target, _rect, Yoffset);
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    // 남은 HP에 따라 이미지 색 변경
    public void UpdateHPBar(float currentHP, float maxHP)
    {
        if (Image_HP != null)
        {
            Image_HP.fillAmount = currentHP / maxHP;
        }

        if (Image_HP.fillAmount >= 0.6)
        {
            Image_HP.color = Color.green;
        }
        else if (Image_HP.fillAmount >= 0.3)
        {
            Image_HP.color = Color.yellow;
        }
        else if (Image_HP.fillAmount > 0)
        {
            Image_HP.color = Color.red;
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}
