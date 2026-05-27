using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceUI : UIBase
{
    [SerializeField] Transform ChoiceParent;

    private Dictionary<string, Choice> _data;
    private List<GameObject> _choiceSlots = new List<GameObject>();

    private void Awake()
    {
        VisualNovelManager.Inst.OnClickChoiceButton = ClearChoiceSlot;
    }

    private void OnEnable()
    {
        _data = GameDataManager.Inst.ChoiceDataList;

        SetChoiceMenu().Forget();
    }

    private async UniTask SetChoiceMenu()
    {
        string currentID = VisualNovelManager.Inst.CurrentDialogueID;
        string slotPath = "Prefabs/UI/ChoiceSlot";

        foreach (string choiceID in _data.Keys)
        {
            if (choiceID.Contains(currentID))
            {
                GameObject prefab = await ResourceManager.Inst.InstantiatePrefab(slotPath, ChoiceParent);

                ChoiceSlot choiceSlot = prefab.GetComponent<ChoiceSlot>();

                string text = _data[choiceID].Content;
                choiceSlot.SetChoiceText(text);
                choiceSlot.SetChoiceID(choiceID);

                _choiceSlots.Add(prefab);
            }
        }
    }

    private void ClearChoiceSlot()
    {
        foreach (GameObject choiceSlot in _choiceSlots)
        {
            Destroy(choiceSlot);
        }

        _choiceSlots.Clear();
    }
}
