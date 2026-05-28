using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingPopup : UIBase
{
    [Header("버튼")]
    [SerializeField] private Button Button_Close;
    [SerializeField] private Button Button_Reset;
    [SerializeField] private Button Button_Confirm;

    [Header("사운드")]
    [SerializeField] private AudioMixer AudioMixer;
    [SerializeField] private Slider Slider_BGM;
    [SerializeField] private Slider Slider_SFX;

    [Header("스크린")]
    [SerializeField] private Slider Slider_Bright;
    [SerializeField] private Toggle Toggle_FullScreen;
    [SerializeField] private TMP_Dropdown Dropdown_TextSpeed;

    [Header("초기화")]
    [SerializeField] private float InitialVolume = 0.5f;

    private float[] _textSpeed = { 0.09f, 0.06f, 0.03f, 0f };

    private void Awake()
    {
        Button_Close.onClick.AddListener(OnClickConfirm);
        Button_Reset.onClick.AddListener(OnClickReset);
        Button_Confirm.onClick.AddListener(OnClickConfirm);

        GetTextSpeed();
        InitSound();
        InitScreen();
    }

    private void InitSound()
    {
        Slider_BGM.value = PlayerPrefs.GetFloat("BGM", InitialVolume);
        Slider_SFX.value = PlayerPrefs.GetFloat("SFX", InitialVolume);

        SetBGMVolume(Slider_BGM.value);
        SetSFXVolume(Slider_SFX.value);

        Slider_BGM.onValueChanged.AddListener(SetBGMVolume);
        Slider_SFX.onValueChanged.AddListener(SetSFXVolume);
    }

    private void InitScreen()
    {
        Toggle_FullScreen.isOn = PlayerPrefs.GetInt("FullScreen", 1) == 1;
        Toggle_FullScreen.onValueChanged.AddListener(SetFullScreen);

        Dropdown_TextSpeed.value = PlayerPrefs.GetInt("TextSpeedIndex", 1);
        Dropdown_TextSpeed.RefreshShownValue();
        Dropdown_TextSpeed.onValueChanged.AddListener(SetTextSpeed);

        Slider_Bright.value = PlayerPrefs.GetFloat("Brightness", 0);
        GameManager.Inst.OnChangeBrightness?.Invoke(Slider_Bright.value);
        Slider_Bright.onValueChanged.AddListener(SetBrightness);

        SetFullScreen(Toggle_FullScreen.isOn);
        SetTextSpeed(Dropdown_TextSpeed.value);
        SetBrightness(Slider_Bright.value);
    }

    private void SetBGMVolume(float value)
    {
        AudioMixer.SetFloat("BGM", Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20);   // 슬라이더 값을 데시벨 단위로 변환
        PlayerPrefs.SetFloat("BGM", value);
    }

    private void SetSFXVolume(float value)
    {
        AudioMixer.SetFloat("SFX", Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20);
        PlayerPrefs.SetFloat("SFX", value);
    }

    private void SetFullScreen(bool isFull)
    {
        Screen.fullScreen = isFull;

        int full = isFull ? 1 : 0;
        PlayerPrefs.SetInt("FullScreen", full);
    }

    private void GetTextSpeed()
    {
        Dropdown_TextSpeed.ClearOptions();
        Dropdown_TextSpeed.AddOptions(new List<string> { "느림", "보통", "빠름", "즉시" });
    }

    private void SetTextSpeed(int idx)
    {
        float cps = _textSpeed[idx];

        PlayerPrefs.SetInt("TextSpeedIndex", idx);
        PlayerPrefs.SetFloat("TextSpeed", cps);
    }

    private void SetBrightness(float value)
    {
        GameManager.Inst.OnChangeBrightness?.Invoke(value);

        PlayerPrefs.SetFloat("Brightness", value);
    }

    private void OnClickReset()
    {
        PlayerPrefs.SetFloat("BGM", InitialVolume);
        PlayerPrefs.SetFloat("SFX", InitialVolume);
        PlayerPrefs.SetInt("FullScreen", 1);
        PlayerPrefs.SetInt("TextSpeedIndex", 1);
        PlayerPrefs.SetFloat("TextSpeed", _textSpeed[1]);
        PlayerPrefs.SetFloat("Brightness", 0);

        InitSound();
        InitScreen();
    }

    private void OnClickConfirm()
    {
        PlayerPrefs.Save();

        UIManager.Inst.CloseSettingPopup();
    }
}
