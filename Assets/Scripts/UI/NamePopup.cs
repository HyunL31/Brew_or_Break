using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 이름 설정 UI
/// </summary>

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

        // 저장 파일 생성 및 이름 저장
        int emptySlot = GameManager.Inst.GetEmptySlotIndex();
        GameManager.Inst.SetCurrentSaveIndex(emptySlot);
        GameManager.Inst.SetName(PlayerName.text, StoreName.text);
        GameManager.Inst.SaveData();

        VisualNovelManager.Inst.SetCurrentDialogueID();
        UIManager.Inst.CloseTitleUI();
        UIManager.Inst.OpenVisualNovelUI();
        UIManager.Inst.OpenDialogueUI();
        UIManager.Inst.CloseGenderUI();
        UIManager.Inst.CloseNamePopup();
    }
}