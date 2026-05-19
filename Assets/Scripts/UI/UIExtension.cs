using UnityEditor;
using UnityEngine;

public enum UIRootType
{
    None,
    Background,
    Main,
    Content,
    Popup,
    Front
}

public enum UIType
{
    TitleUI,
    LobbyUI,
    DialogueUI,
    ChoiceUI,
    ClueUI
}

public static class UIExtension
{
    public static void InitStart(this UIManager uiManager)
    {
        uiManager.OpenMainUI(UIType.TitleUI);
        uiManager.OpenMainUI(UIType.LobbyUI, false);
        uiManager.OpenMainUI(UIType.DialogueUI, false);
    }

    public static void CloseTitleUI(this UIManager uiManager)
    {
        uiManager.CloseUI(UIType.TitleUI);
    }

    public static void OpenLobbyUI(this UIManager uiManager)
    {
        uiManager.OpenMainUI(UIType.LobbyUI);
    }

    public static void CloseLobbyUI(this UIManager uiManager)
    {
        uiManager.CloseUI(UIType.LobbyUI);
    }

    public static void OpenDialogueUI(this UIManager uiManager)
    {
        uiManager.OpenMainUI(UIType.DialogueUI);
    }

    public static void CloseDialogueUI(this UIManager uiManager)
    {
        uiManager.CloseUI(UIType.DialogueUI);
    }
}
