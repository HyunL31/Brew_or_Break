using Cysharp.Threading.Tasks;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 저장 슬롯
/// </summary>

public class SaveSlot : MonoBehaviour
{
    [Header("버튼")]
    [SerializeField] private Button Button_Discard;
    [SerializeField] private Button Button_Confirm;
    [SerializeField] private Button Button_Ending;

    [Header("이름")]
    [SerializeField] private TextMeshProUGUI Text_PlayerName;
    [SerializeField] private TextMeshProUGUI Text_StoreName;
    [SerializeField] private TextMeshProUGUI Text_Day;
    [SerializeField] private Image Image_Character;

    [Header("상태바")]
    [SerializeField] private TextMeshProUGUI Text_Reputation;
    [SerializeField] private Image Image_Reputation;
    [SerializeField] private TextMeshProUGUI Text_Compen;
    [SerializeField] private Image Image_Compen;
    [SerializeField] private TextMeshProUGUI Text_Level;
    [SerializeField] private Image Image_Level;

    private int _slotID;
    private PlayerModel _slotModel;

    private void Awake()
    {
        Button_Discard.onClick.AddListener(OnClickDiscard);
        Button_Confirm.onClick.AddListener(OnClickConfirm);
        Button_Ending.onClick.AddListener(OnClickEnding);
    }

    public void InitSlot(int slotID)
    {
        _slotID = slotID;

        _slotModel = SaveManager.Inst.RequestLoadData(slotID);

        Text_PlayerName.text = $"이름 : {_slotModel.PlayerName}";
        Text_StoreName.text = $"상호명 : {_slotModel.StoreName}";

        string path = $"Icon/Portrait[{_slotModel.Gender}_Player_01_04]";
        GameUtil.LoadSpriteAndSet(path, Image_Character).Forget();

        if (_slotModel.IsComplete)
        {
            Text_Day.text = "완결";
            Button_Confirm.interactable = false;
            Button_Ending.gameObject.SetActive(true);
        }
        else
        {
            Text_Day.text = $"Day {_slotModel.Day}";
        }

        Text_Reputation.text = $"가게 명성 : {_slotModel.Store.Reputation}";
        Text_Compen.text = $"변상금 : {_slotModel.Store.Compensation}";
        Text_Level.text = $"가게 레벨 : {_slotModel.Store.Level}";

        Image_Reputation.fillAmount = StoreManager.Inst.CalculatStat(StatType.Reputation, _slotModel.Store);
        Image_Compen.fillAmount = StoreManager.Inst.CalculatStat(StatType.Compensation, _slotModel.Store);
        Image_Level.fillAmount = StoreManager.Inst.CalculatStat(StatType.Level, _slotModel.Store);
    }

    private void OnClickConfirm()
    {
        GameManager.Inst.OnStartGame?.Invoke(_slotID);
        UIManager.Inst.OpenLobbyUI();
        UIManager.Inst.CloseTitleUI();
        UIManager.Inst.CloseSaveUI();
    }

    // 완결 파일 다시보기
    private void OnClickEnding()
    {
        GameManager.Inst.OnStartGame?.Invoke(_slotID);
        VisualNovelManager.Inst.OnSetDialogueID(_slotModel.EndingID);

        UIManager.Inst.OpenVisualNovelUI();
        UIManager.Inst.OpenDialogueUI();
        VisualNovelManager.Inst.OnEnterEnding?.Invoke();

        UIManager.Inst.CloseTitleUI();
        UIManager.Inst.CloseSaveUI();
    }

    // 저장 파일 삭제
    private void OnClickDiscard()
    {
        string path = Path.Combine(Application.persistentDataPath, $"BrewOrBreak{_slotID}.json");

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        GameManager.Inst.SlotIndex.Remove(_slotID);
        SaveManager.Inst.OnSaveClear?.Invoke();

        Destroy(this.gameObject);
    }
}
