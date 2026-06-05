using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : UIBase
{
    [SerializeField] private GameObject StoreText;
    [SerializeField] private GameObject Collecting;
    [SerializeField] private Slider Slider_Loading;
    [SerializeField] private Image Image_Background;

    private CancellationTokenSource _cancelToken;
    private float _duration = 2.5f;

    private void OnEnable()
    {
        SetLoadingUI();
        StartLoading(_duration).Forget();
    }

    private void SetLoadingUI()
    {
        if (GameManager.Inst.IsOpeningStore)
        {
            StoreText.SetActive(true);
            Collecting.SetActive(false);

            GameUtil.LoadSpriteAndSet($"Background/{GameManager.Inst.PlayerModel.Gender}/StoreLoading", Image_Background).Forget();
        }
        else
        {
            StoreText.SetActive(false);
            Collecting.SetActive(true);

            GameUtil.LoadSpriteAndSet($"Background/{GameManager.Inst.PlayerModel.Gender}/CollectingLoading", Image_Background).Forget();
        }
    }

    private async UniTaskVoid StartLoading(float duration)
    {
        _cancelToken = new CancellationTokenSource();

        float elapsed = 0f;
        Slider_Loading.value = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsed / duration);

            Slider_Loading.value = progress;

            await UniTask.Yield(PlayerLoopTiming.Update, _cancelToken.Token);
        }

        if(GameManager.Inst.IsOpeningStore)
        {
            UIManager.Inst.OpenVisualNovelUI();
            UIManager.Inst.OpenDialogueUI();
        }

        UIManager.Inst.CloseLoadingUI();
    }
}
