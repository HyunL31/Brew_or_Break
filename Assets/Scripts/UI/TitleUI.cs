using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 타이틀 메인 UI
/// </summary>

public class TitleUI : UIBase
{
    [Header("버튼")]
    [SerializeField] private Button Button_NewGame;
    [SerializeField] private Button Button_GameLoad;
    [SerializeField] private Button Button_GameSetting;
    [SerializeField] private Button Button_Quit;

    private void Awake()
    {
        Button_NewGame.onClick.AddListener(OnClickNewGame);
        Button_GameLoad.onClick.AddListener(OnClickLoadButton);
        Button_GameSetting.onClick.AddListener(OnClickSettingButton);
        Button_Quit.onClick.AddListener(Application.Quit);
    }

    private void OnEnable()
    {
        SoundManager.Inst.OnBGM?.Invoke("Audio/Base");
    }

    private void OnClickNewGame()
    {
        GameManager.Inst.LoadDefaultData();
        StoreManager.Inst.StoreInit();

        UIManager.Inst.OpenGenderUI();
    }

    private void OnClickLoadButton()
    {
        UIManager.Inst.OpenSaveUI();
    }

    private void OnClickSettingButton()
    {
        UIManager.Inst.OpenSettingPopup();
    }
}
