using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonFrame : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject Frame;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Frame.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Frame.SetActive(false);
    }
}
