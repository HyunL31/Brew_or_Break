using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Inst;

    [SerializeField] private AudioSource Audio_BGM;
    [SerializeField] private AudioSource Audio_SFX;

    private void Awake()
    {
        Inst = this;
    }

    private void Start()
    {
        LoadSoundInit();
        SetBGMAndPlay("Base");
    }

    public void LoadSoundInit()
    {
        GameUtil.LoadSoundAndSet("Popup", Audio_SFX);
        GameUtil.LoadSoundAndSet("Typing", Audio_SFX);
        GameUtil.LoadSoundAndSet("Button", Audio_SFX);
        GameUtil.LoadSoundAndSet("Bomb", Audio_SFX);
    }

    public void SetBGMAndPlay(string clip)
    {
        PauseAudio(Audio_BGM);

        GameUtil.LoadSoundAndSet(clip, Audio_BGM);

        Audio_BGM.Play();
    }

    public void SetSFXAndPlay(string clip)
    {
        AudioClip audioClip = GameUtil.LoadSoundAndSet(clip, Audio_SFX);

        Audio_SFX.PlayOneShot(audioClip);
    }

    public void SetTypingAndPlay(AudioSource audioSource)
    {
        PauseAudio(audioSource);

        GameUtil.LoadSoundAndSet("Typing", audioSource);
        audioSource.Play();
    }

    public void PauseAudio(AudioSource audioSource)
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }
}
