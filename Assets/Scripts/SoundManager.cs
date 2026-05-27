using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Inst;

    [SerializeField] private AudioSource Audio_BGM;
    [SerializeField] private AudioSource Audio_SFX;

    public Action<string> OnBGM;
    public Action<string> OnSFX;
    public Action<AudioSource> OnPause;
    public Action<AudioSource> OnTyping;

    private void Awake()
    {
        Inst = this;

        OnBGM = (path) => SetBGMAndPlay(path).Forget();
        OnSFX = (clip) => SetSFXAndPlay(clip).Forget();
        OnPause = (audioSource) => PauseAudio(audioSource);
        OnTyping = (audioSource) => SetTypingAndPlay(audioSource).Forget();
    }

    private void Start()
    {
        SetBGMAndPlay("Audio/Base").Forget();
    }

    public async UniTask SetBGMAndPlay(string path)
    {
        PauseAudio(Audio_BGM);

        await GameUtil.LoadSoundAndSet(path, Audio_BGM);

        Audio_BGM.Play();
    }

    public async UniTask SetSFXAndPlay(string clip)
    {
        AudioClip audioClip = await GameUtil.LoadSoundAndSet(clip, Audio_SFX);

        Audio_SFX.PlayOneShot(audioClip);
    }

    public async UniTask SetTypingAndPlay(AudioSource audioSource)
    {
        PauseAudio(audioSource);

        await GameUtil.LoadSoundAndSet("Audio/Typing", audioSource);
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
