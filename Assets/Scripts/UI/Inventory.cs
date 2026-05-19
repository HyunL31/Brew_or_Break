using UnityEngine;
using UnityEngine.UI;

public class Inventory : UIBase
{
    [SerializeField] Button Button_Close;

    private void Awake()
    {
        Button_Close.onClick.AddListener(UIManager.Inst.CloseInventory);
    }
}