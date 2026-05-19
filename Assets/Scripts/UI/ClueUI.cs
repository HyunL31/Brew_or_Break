using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClueUI : UIBase
{
    [SerializeField] private List<GameObject> ClueButtons = new List<GameObject>();

    [SerializeField] private GameObject clueSlotPrefab;
    [SerializeField] private Transform parent;
    [SerializeField] private Button Button_Confirm;

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
    }

    private void OnClickConfirm()
    {
        // [TODO] 팝업창 띄우기

        ClueButtons[GameManager.Inst.GetDay() - 1].SetActive(false);

        UIManager.Inst.CloseClueUI();
        UIManager.Inst.OpenDialogueUI();
    }
}
