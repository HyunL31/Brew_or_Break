using UnityEngine;
using UnityEngine.UI;

public class TitleUI : UIBase
{
    [Header("버튼")]
    [SerializeField] private Button Button_NewGame;
    [SerializeField] private Button Button_GameLoad;
    [SerializeField] private Button Button_GameSetting;
    [SerializeField] private Button Button_Quit;

    private void Awake()
    {
        Button_NewGame.onClick.AddListener(OnClickNewGameButton);
        Button_GameLoad.onClick.AddListener(OnClickLoadButton);
        Button_GameSetting.onClick.AddListener(OnClickSettingButton);
        Button_Quit.onClick.AddListener(OnClickQuitButton);
    }

    private void OnClickNewGameButton()
    {
        VisualNovelManager.Inst.SetCurrentDialogueID();
        UIManager.Inst.OpenVisualNovelUI();
        UIManager.Inst.OpenDialogueUI();
        UIManager.Inst.CloseTitleUI();
    }

    private void OnClickLoadButton()
    {
        UIManager.Inst.OpenLobbyUI();
        this.gameObject.SetActive(false);
    }

    private void OnClickSettingButton()
    {
        // [TODO] 설정 팝업 구현
    }

    private void OnClickQuitButton()
    {
        Application.Quit();
    }
}
