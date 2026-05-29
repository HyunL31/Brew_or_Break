using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NamePopup : UIBase
{
    [Header("이름 입력")]
    [SerializeField] private TMP_InputField PlayerName;
    [SerializeField] private TMP_InputField StoreName;
    [SerializeField] private Button Button_Confirm;
    [SerializeField] private Button Button_Close;

    private void Awake()
    {
        Button_Confirm.onClick.AddListener(OnClickConfirm);
        Button_Close.onClick.AddListener(UIManager.Inst.CloseNamePopup);
    }

    private void OnClickConfirm()
    {
        if ((PlayerName.text == string.Empty || StoreName.text == string.Empty) || PlayerName.text.Length > 10 || StoreName.text.Length > 16)
        {
            UIManager.Inst.OpenConfirmPopup("조건에 맞지 않는 이름입니다.\r\n\r\n플레이어 이름(최대 6글자)과\r\n가게 이름(최대 11글자)을\r\n정확히 작성해주세요.");
            return;
        }

        int emptySlot = GameManager.Inst.GetEmptySlotIndex();
        GameManager.Inst.SetCurrentSaveIndex(emptySlot);

        GameManager.Inst.LoadDefaultData();
        GameManager.Inst.SetName(PlayerName.text, StoreName.text);
        StoreManager.Inst.StoreInit();

        GameManager.Inst.SaveData();

        VisualNovelManager.Inst.SetCurrentDialogueID();
        UIManager.Inst.OpenVisualNovelUI();
        UIManager.Inst.OpenDialogueUI();
        UIManager.Inst.CloseNamePopup();
        UIManager.Inst.CloseTitleUI();
    }
}
