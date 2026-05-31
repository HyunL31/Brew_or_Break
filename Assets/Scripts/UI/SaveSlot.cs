using Cysharp.Threading.Tasks;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [Header("버튼")]
    [SerializeField] private Button Button_Discard;
    [SerializeField] private Button Button_Confirm;

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

    private void Awake()
    {
        Button_Discard.onClick.AddListener(OnClickDiscard);
        Button_Confirm.onClick.AddListener(OnClickConfirm);
    }

    public void InitSlot(int slotID)
    {
        _slotID = slotID;

        PlayerModel slotModel = SaveManager.Inst.RequestLoadData(slotID);

        Text_PlayerName.text = $"이름 : {slotModel.PlayerName}";
        Text_StoreName.text = $"상호명 : {slotModel.StoreName}";

        string path = $"Icon/Portrait[{slotModel.Gender}_Player_01_04]";
        GameUtil.LoadSpriteAndSet(path, Image_Character).Forget();

        if (slotModel.IsComplete)
        {
            Text_Day.text = "완결";
            Button_Confirm.interactable = false;
        }
        else
        {
            Text_Day.text = $"Day {slotModel.Day}";
        }

        Text_Reputation.text = $"가게 명성 : {slotModel.Store.Reputation}";
        Text_Compen.text = $"변상금 : {slotModel.Store.Compensation}";
        Text_Level.text = $"가게 레벨 : {slotModel.Store.Level}";

        Image_Reputation.fillAmount = StoreManager.Inst.CalculatStat(StatType.Reputation, slotModel.Store);
        Image_Compen.fillAmount = StoreManager.Inst.CalculatStat(StatType.Compensation, slotModel.Store);
        Image_Level.fillAmount = StoreManager.Inst.CalculatStat(StatType.Level, slotModel.Store);
    }

    private void OnClickConfirm()
    {
        GameManager.Inst.OnStartGame?.Invoke(_slotID);
        UIManager.Inst.OpenLobbyUI();
        UIManager.Inst.CloseTitleUI();
        UIManager.Inst.CloseSaveUI();
    }

    private void OnClickDiscard()
    {
        string path = Path.Combine(Application.persistentDataPath, $"BrewOrBreak{_slotID}.json");

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        GameManager.Inst.SlotIndex.Remove(_slotID);
        Destroy(this.gameObject);
    }
}
