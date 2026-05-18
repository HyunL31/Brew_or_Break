using UnityEngine;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    [Header("버튼")]
    [SerializeField] private Button NewGameButton;
    [SerializeField] private Button GameLoadButton;
    [SerializeField] private Button GameSettingButton;
    [SerializeField] private Button QuitButton;

    [Header("패널")]
    [SerializeField] private GameObject LobbyUI;

    private void Awake()
    {
        NewGameButton.onClick.AddListener(OnClickNewGameButton);
        GameLoadButton.onClick.AddListener(OnClickLoadButton);
        GameSettingButton.onClick.AddListener(OnClickSettingButton);
        QuitButton.onClick.AddListener(OnClickQuitButton);
    }

    private void OnClickNewGameButton()
    {
        LobbyUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    private void OnClickLoadButton()
    {
        LobbyUI.SetActive(true);
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
