using UnityEngine;

public class ChoiceUI : UIBase
{
    [SerializeField] GameObject ChoiceSlotPrefab;
    [SerializeField] Transform ChoiceParent;

    private void OnEnable()
    {
        SetChoiceMenu();
    }

    private void SetChoiceMenu()
    {
        string currentID = VisualNovelManager.Inst.GetCurrentDialogueID();
        var data = GameDataManager.Inst.ChoiceDataList;

        foreach (string choiceID in data.Keys)
        {
            if (choiceID.Contains(currentID))
            {
                GameObject slot = Instantiate(ChoiceSlotPrefab, ChoiceParent);
                ChoiceSlot choiceSlot = slot.GetComponent<ChoiceSlot>();

                string text = data[choiceID].Content;
                choiceSlot.SetChoiceText(text);
                choiceSlot.SetChoiceID(choiceID);
            }
        }
    }
}
