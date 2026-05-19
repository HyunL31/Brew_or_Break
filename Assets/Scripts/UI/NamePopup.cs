using UnityEngine;
using UnityEngine.UI;

public class NamePopup : UIBase
{
    [Header("이름 입력")]
    [SerializeField] private InputField PlayerName;
    [SerializeField] private InputField StoreName;
    [SerializeField] private Button Button_Confirm;

    [Header("경고 팝업")]
    [SerializeField] private GameObject AlertPopup;
    [SerializeField] private Button Button_Alert;

    private void Awake()
    {
        
    }
}
