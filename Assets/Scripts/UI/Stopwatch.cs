using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Stopwatch : MonoBehaviour
{
    [SerializeField] private RectTransform SecondHand;
    [SerializeField] private Image Image_Filled;

    private float _duration = 30f;

    private void OnEnable()
    {
        SoundManager.Inst.SetSFXAndPlay("Audio/Clock").Forget();
        RotateSceondHand().Forget();
    }

    // 스탑워치 타이머
    private async UniTaskVoid RotateSceondHand()
    {
        CancellationToken cancelToken = this.GetCancellationTokenOnDestroy();

        float elapsed = 0f;

        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;

            float progress = elapsed / _duration;
            float targetAngle = progress * -360f;

            SecondHand.rotation = Quaternion.Euler(0f, 0f, targetAngle);
            Image_Filled.fillAmount = progress;

            await UniTask.Yield(PlayerLoopTiming.Update, cancelToken);
        }

        SecondHand.rotation = Quaternion.Euler(0f, 0f, -360f);

        await UniTask.Delay(TimeSpan.FromSeconds(0.3f), cancellationToken: cancelToken);

        VisualNovelManager.Inst.OnEndTimer?.Invoke();

        this.gameObject.SetActive(false);
    }
}
