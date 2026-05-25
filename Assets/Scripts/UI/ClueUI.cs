using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClueUI : UIBase
{
    [SerializeField] private List<GameObject> ClueButtons = new List<GameObject>();

    [SerializeField] private GameObject clueSlotPrefab;
    [SerializeField] private Transform parent;
    [SerializeField] private Button Button_Confirm;

    private List<GameObject> _clueSlots = new List<GameObject>();

    private void OnEnable()
    {
        VisualNovelManager.Inst.OnClickClueButton = SetClueSlot;
        Button_Confirm.onClick.AddListener(OnClickConfirm);
        ActiveClueParent(GameManager.Inst.GetDay());
    }

    private void ActiveClueParent(int day)
    {
        ClueButtons[day - 1].SetActive(true);
    }

    private void SetClueSlot(string id)
    {
        GameObject slot = Instantiate(clueSlotPrefab, parent);
        ClueSlot clueSlot = slot.GetComponent<ClueSlot>();
        clueSlot.SetClueInfo(id);

        _clueSlots.Add(slot);
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

        ClueButtons[GameManager.Inst.GetDay() - 1].SetActive(false);

        UIManager.Inst.OpenDialogueUI();
        UIManager.Inst.CloseClueUI();
    }
}
