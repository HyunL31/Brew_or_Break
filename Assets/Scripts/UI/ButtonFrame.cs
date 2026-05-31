using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
