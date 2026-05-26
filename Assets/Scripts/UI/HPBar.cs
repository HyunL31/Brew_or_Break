using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField] private Image Image_HP;
    [SerializeField] private float Yoffset = 1f;
    [SerializeField] private RectTransform _rect;

    private Transform _target;
    private Camera _main;

    private void Awake()
    {
        _main = Camera.main;
    }

    private void LateUpdate()
    {
        SetPosition();
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void SetPosition()
    {
        if (_target == null || _main == null)
        {
            return;
        }

        Vector3 targetPos = _target.position + new Vector3(0, Yoffset, 0);
        Vector3 screenPos = _main.WorldToScreenPoint(targetPos);

        _rect.transform.position = screenPos;
    }

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
