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
    VisualNovelUI,
    DialogueUI,
    ChoiceUI,
    ClueUI,
    InventoryPopup,
    NamePopup,
    CluePopup,
    LobbyPopup,
    ItemDescription
}

public static class UIExtension
{
    public static void InitStart(this UIManager uiManager)
    {
        uiManager.OpenMainUI(UIType.TitleUI);
        uiManager.OpenMainUI(UIType.LobbyUI, false);
        uiManager.OpenMainUI(UIType.VisualNovelUI, false);
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

    public static void OpenVisualNovelUI(this UIManager uiManager)
    {
        uiManager.OpenMainUI(UIType.VisualNovelUI);
    }

    public static void CloseVisualNovelUI(this UIManager uiManager)
    {
        uiManager.CloseUI(UIType.VisualNovelUI);
    }

    public static void OpenDialogueUI(this UIManager uiManager)
    {
        uiManager.OpenContentUI(UIType.DialogueUI);
    }

    public static void CloseDialogueUI(this UIManager uiManager)
    {
        uiManager.CloseUI(UIType.DialogueUI);
    }

    public static void OpenClueUI(this UIManager uiManager)
    {
        uiManager.OpenContentUI(UIType.ClueUI);
    }

    public static void CloseClueUI(this UIManager uiManager)
    {
        uiManager.CloseUI(UIType.ClueUI);
    }

    public static void OpenChoiceUI(this UIManager uiManager)
    {
        uiManager.OpenContentUI(UIType.ChoiceUI);
    }

    public static void CloseChoiceUI(this UIManager uiManager)
    {
        uiManager.CloseUI(UIType.ChoiceUI);
    }

    public static void OpenInventory(this UIManager uiManager)
    {
        uiManager.OpenPopupUI(UIType.InventoryPopup);
    }

    public static void CloseInventory(this UIManager uiManager)
    {
        uiManager.CloseUI(UIType.InventoryPopup);
    }

    public static void OpenNamePopup(this UIManager uiManager)
    {
        uiManager.OpenPopupUI(UIType.NamePopup);
    }

    public static void CloseNamePopup(this UIManager uiManager)
    {
        uiManager.CloseUI(UIType.NamePopup);
    }

    public static UIBase OpenItemDescription(this UIManager uiManager)
    {
        return uiManager.OpenPopupUI(UIType.ItemDescription);
    }

    public static void CloseItemDescription(this UIManager uiManager)
    {
        uiManager.CloseUI(UIType.ItemDescription);
    }
}
