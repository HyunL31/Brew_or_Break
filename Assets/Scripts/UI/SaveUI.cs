using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 저장 파일 로드 UI
/// </summary>

public class SaveUI : UIBase
{
    [SerializeField] private Button Button_Close;
    [SerializeField] private Transform SlotParent;
    [SerializeField] private GameObject InfoText;

    private HashSet<int> _slotID = new HashSet<int>();

    private void Awake()
    {
        Button_Close.onClick.AddListener(OnClickClose);

        SaveManager.Inst.OnSaveClear = ClearSaveSlot;
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
        ClearSaveSlot();

        foreach (int i in GameManager.Inst.SlotIndex)
        {
            if (_slotID.Contains(i))
            {
                continue;
            }

            GameObject prefab = await ResourceManager.Inst.InstantiatePrefab("Prefabs/UI/SaveSlot", SlotParent);
            SaveSlot saveSlot = prefab.GetComponent<SaveSlot>();
            saveSlot.InitSlot(i);

            _slotID.Add(i);
        }
    }

    private void ClearSaveSlot()
    {
        if (GameManager.Inst.SlotIndex.Count == 0)
        {
            InfoText.SetActive(true);
        }
        else
        {
            InfoText.SetActive(false);
        }
    }

    private void OnClickClose()
    {
        UIManager.Inst.CloseSaveUI();
    }
}
