using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 화면 밝기 조절용 오버레이 이미지
/// </summary>

public class OverlayScreen : MonoBehaviour
{
    [SerializeField] private Image Image_Overlay;

    private void Start()
    {
        ApplyBrightness(PlayerPrefs.GetFloat("Brightness", 0f));
        GameManager.Inst.OnChangeBrightness = (value) => ApplyBrightness(value);
    }

    public void ApplyBrightness(float value)
    {
        float alpha = Mathf.Clamp(value, 0.0f, 0.8f);
        Image_Overlay.color = new Color(0, 0, 0, alpha);
    }
}
