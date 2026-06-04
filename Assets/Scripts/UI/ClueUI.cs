using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 단서 콘텐츠 UI
/// </summary>

public class ClueUI : UIBase
{
    [SerializeField] private List<GameObject> ClueButtons = new List<GameObject>();

    [SerializeField] private Transform SlotParent;
    [SerializeField] private Button Button_Confirm;
    [SerializeField] private GameObject EmptyInfo;

    private List<GameObject> _clueSlots = new List<GameObject>();

    private void OnEnable()
    {
        VisualNovelManager.Inst.OnClickClueButton = (id) => SetClueSlot(id).Forget();
        Button_Confirm.onClick.AddListener(OnClickConfirm);
        ActiveClueParent(GameManager.Inst.PlayerModel.Day);

        EmptyInfo.gameObject.SetActive(true);
    }

    private void ActiveClueParent(int day)
    {
        ClueButtons[day - 1].SetActive(true);
        
        foreach (Transform child in ClueButtons[day - 1].transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    private async UniTask SetClueSlot(string id)
    {
        EmptyInfo.gameObject.SetActive(false);

        string slotPath = "Prefabs/UI/ClueSlot";

        GameObject prefab = await ResourceManager.Inst.InstantiatePrefab(slotPath, SlotParent);

        _clueSlots.Add(prefab);

        ClueSlot clueSlot = prefab.GetComponent<ClueSlot>();
        clueSlot.SetClueInfo(id);
    }

    private void OnClickConfirm()
    {
        if (_clueSlots.Count <= 0)
        {
            UIManager.Inst.OpenConfirmPopup("아직 단서를 찾지 못했습니다.\n의심되는 부분을 클릭해주세요.");
            return;
        }

        foreach (GameObject clueSlot in _clueSlots)
        {
            Destroy(clueSlot);
        }

        _clueSlots.Clear();

        ClueButtons[GameManager.Inst.PlayerModel.Day - 1].SetActive(false);

        UIManager.Inst.OpenDialogueUI();
        UIManager.Inst.CloseClueUI();
    }

    private void OnDisable()
    {
        VisualNovelManager.Inst.OnClickClueButton = null;
        Button_Confirm.onClick.RemoveAllListeners();
    }
}
