using UnityEngine;
using UnityEngine.UI;

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
