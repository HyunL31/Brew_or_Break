using System;

/// <summary>
/// UI 열고 닫기 전용 메서드 모음
/// </summary>

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
    Background,
    TitleUI,
    LobbyUI,
    VisualNovelUI,
    DialogueUI,
    ChoiceUI,
    ClueUI,
    InventoryPopup,
    NamePopup,
    ConfirmPopup,
    ItemDescription,
    CraftUI,
    RecipePopup,
    DrawItem,
    AccountUI,
    HUD,
    DialogueLog,
    SettingPopup,
    SaveUI,
    GenderUI,
    EndingPopup
}

public static class UIExtension
{
    public static void InitStart(this UIManager uiManager)
    {
        uiManager.OpenMainUI(UIType.TitleUI);
        uiManager.OpenMainUI(UIType.LobbyUI, false);
        uiManager.OpenBackgroundUI();
        uiManager.OpenMainUI(UIType.VisualNovelUI, false);
        uiManager.OpenPopupUI(UIType.SettingPopup, false);
    }

    public static void OpenBackgroundUI(this UIManager uiManager)
    {
        uiManager.OpenUI(UIRootType.Background, UIType.Background);
    }

    public static void CloseBackgroundUI(this UIManager uiManager)
    {
        uiManager.CloseUI(UIType.Background);
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

    public static void OpenCraftUI(this UIManager uiManager)
    {
        uiManager.OpenContentUI(UIType.CraftUI);
    }

    public static void CloseCraftUI(this UIManager uiManager)
    {
        uiManager.CloseUI(UIType.CraftUI);
    }

    public static void OpenAccountUI(this UIManager uiManager)
    {
        uiManager.OpenContentUI(UIType.AccountUI);
    }

    public static void CloseAccountUI(this UIManager uiManager)
    {
        uiManager.CloseUI(UIType.AccountUI);
    }

    public static void OpenHUD(this UIManager uiManager)
    {
        SoundManager.Inst.OnBGM("Audio/Base");
        uiManager.OpenContentUI(UIType.HUD);
    }

    public static void CloseHUD(this UIManager uiManager)
    {
        uiManager.CloseUI(UIType.HUD);
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
        return uiManager.OpenUI(UIRootType.Front, UIType.ItemDescription);
    }

    public static void CloseItemDescription(this UIManager uiManager)
    {
        uiManager.CloseUI(UIType.ItemDescription);
    }

    public static void OpenRecipePopup(this UIManager uiManager)
    {
        uiManager.OpenPopupUI(UIType.RecipePopup);
    }

    public static void CloseRecipePopup(this UIManager uiManager)
    {
        uiManager.CloseUI(UIType.RecipePopup);
    }
    public static UIBase OpenDrawItem(this UIManager uiManager)
    {
        return uiManager.OpenUI(UIRootType.Front, UIType.DrawItem);
    }

    public static void CloseDrawItem(this UIManager uiManager)
    {
        uiManager.CloseUI(UIType.DrawItem);
    }

    public static void OpenConfirmPopup(this UIManager uiManager, string text, Action callback = null)
    {
        UIBase uiBase = uiManager.OpenPopupUI(UIType.ConfirmPopup);

        if (uiBase is ConfirmPopup confirm)
        {
            confirm.SetText(text, callback);
        }
    }

    public static void CloseConfirmPopup(this UIManager uiManager)
    {
        uiManager.CloseUI(UIType.ConfirmPopup);
    }

    public static void OpenDialogueLog(this UIManager uiManager)
    {
        UIBase uiBase = uiManager.OpenPopupUI(UIType.DialogueLog);
    }

    public static void CloseDialogueLog(this UIManager uiManager)
    {
        uiManager.CloseUI(UIType.DialogueLog);
    }

    public static void OpenSettingPopup(this UIManager uiManager)
    {
        UIBase uiBase = uiManager.OpenPopupUI(UIType.SettingPopup);
    }

    public static void CloseSettingPopup(this UIManager uiManager)
    {
        uiManager.CloseUI(UIType.SettingPopup);
    }

    public static void OpenEndingPopup(this UIManager uiManager)
    {
        UIBase uiBase = uiManager.OpenPopupUI(UIType.EndingPopup);
    }

    public static void CloseEndingPopup(this UIManager uiManager)
    {
        uiManager.CloseUI(UIType.EndingPopup);
    }

    public static void OpenSaveUI(this UIManager uiManager)
    {
        UIBase uiBase = uiManager.OpenPopupUI(UIType.SaveUI);
    }

    public static void CloseSaveUI(this UIManager uiManager)
    {
        uiManager.CloseUI(UIType.SaveUI);
    }

    public static void OpenGenderUI(this UIManager uiManager)
    {
        UIBase uiBase = uiManager.OpenPopupUI(UIType.GenderUI);
    }

    public static void CloseGenderUI(this UIManager uiManager)
    {
        uiManager.CloseUI(UIType.GenderUI);
    }
}
