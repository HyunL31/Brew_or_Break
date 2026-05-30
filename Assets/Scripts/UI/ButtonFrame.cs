using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonFrame : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject Frame;

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
