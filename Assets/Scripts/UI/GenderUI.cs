using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 성별 선택 UI
/// </summary>

public class GenderUI : UIBase
{
    [SerializeField] private Button Button_Close;
    [SerializeField] private Image Image_Girl;
    [SerializeField] private Image Image_Boy;
    [SerializeField] private Button Button_Girl;
    [SerializeField] private Button Button_Boy;
    [SerializeField] private Button Button_Confirm;

    private void Awake()
    {
        Button_Close.onClick.AddListener(OnClickClose);
        Button_Girl.onClick.AddListener(OnClickGirl);
        Button_Boy.onClick.AddListener(OnClickBoy);
        Button_Confirm.onClick.AddListener(SetName);
    }

    private void OnEnable()
    {
        GameUtil.LoadSpriteAndSet("Character/Girl", Image_Girl).Forget();
        GameUtil.LoadSpriteAndSet("Character/Boy", Image_Boy).Forget();

        Button_Confirm.gameObject.SetActive(false);

        Button_Girl.interactable = true;
        Button_Boy.interactable = true;
    }

    private void OnClickClose()
    {
        UIManager.Inst.CloseGenderUI();
    }

    private void OnClickGirl()
    {
        GameManager.Inst.PlayerModel.Gender = "Girl";
        Button_Confirm.gameObject.SetActive(true);

        Button_Girl.interactable = false;
        Button_Boy.interactable = true;

        SetButtonImage("Boy", Image_Boy, "Girl", Image_Girl);
    }

    private void OnClickBoy()
    {
        GameManager.Inst.PlayerModel.Gender = "Boy";
        Button_Confirm.gameObject.SetActive(true);

        Button_Girl.interactable = true;
        Button_Boy.interactable = false;

        SetButtonImage("Girl", Image_Girl, "Boy", Image_Boy);
    }

    private void SetButtonImage(string gender, Image image, string selectedGender, Image selectedImage)
    {
        string path = $"Character/{gender}";
        GameUtil.LoadSpriteAndSet(path, image).Forget();

        path = $"Character/{selectedGender}_Selected";
        GameUtil.LoadSpriteAndSet(path, selectedImage).Forget();
    }

    private void SetName()
    {
        UIManager.Inst.OpenNamePopup();
    }
}