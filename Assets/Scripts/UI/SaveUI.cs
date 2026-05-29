using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveUI : UIBase
{
    [SerializeField] private Button Button_Close;
    [SerializeField] private Transform SlotParent;

    private HashSet<int> _slotID = new HashSet<int>();

    private void Awake()
    {
        Button_Close.onClick.AddListener(OnClickClose);
    }

    private void OnEnable()
    {
        foreach (Transform child in SlotParent)
        {
            Destroy(child.gameObject);
        }

        _slotID.Clear();

        RefreshSlot().Forget();
    }

    private async UniTask RefreshSlot()
    {
        foreach (int i in GameManager.Inst.SlotIndex)
        {
            if (_slotID.Contains(i))
            {
                continue;
            }

            GameObject prefab = await ResourceManager.Inst.InstantiatePrefab("Prefabs/UI/SaveSlot", SlotParent);
            prefab.GetComponent<SaveSlot>().InitSlot(i);
            _slotID.Add(i);
        }
    }

    private void OnClickClose()
    {
        UIManager.Inst.CloseSaveUI();
    }
}
