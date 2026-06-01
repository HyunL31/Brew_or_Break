using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 버튼 호버 사운드 & 프레임 활성화
/// </summary>

public class ButtonFrame : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject Frame;

    private void OnEnable()
    {
        if (Frame != null)
        {
            Frame.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Inst.SetSFXAndPlay("Audio/Button").Forget();

        if (Frame != null )
        {
            Frame.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Frame != null )
        {
            Frame.SetActive(false);
        }
    }
}
