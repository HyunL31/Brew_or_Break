using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 비주얼 노벨 기본 (캐릭터, 배경) UI
/// </summary>

public class VisualNovelUI : UIBase
{
    [SerializeField] private Image Image_Background;
    [SerializeField] private GameObject CharacterContainer;
    [SerializeField] private Image Image_Left;
    [SerializeField] private Image Image_Center;
    [SerializeField] private Image Image_Right;
    [SerializeField] private RectTransform Rect;

    private Dictionary<string, Dialogue> _data;

    private CancellationTokenSource _backgroundToken;
    private CancellationTokenSource _characterToken;
    private CancellationTokenSource _cameraShakeToken;
    private float _totalShakeTime = 1f;

    private void Awake()
    {
        _data = GameDataManager.Inst.DialogueDataList;

        VisualNovelManager.Inst.OnChangeBaseUI += (id) => ChangeBackgroundImage(id).Forget();
        VisualNovelManager.Inst.OnChangeBaseUI += (id) => ChangeSpeakerCharacter(id).Forget();
        VisualNovelManager.Inst.OnDialogueCommand += (id) => ApplyCommand(id);
    }

    // 배경 스프라이트 변경
    private async UniTask ChangeBackgroundImage(string id)
    {
        CancelBackground();
        _backgroundToken = new CancellationTokenSource();

        string background = _data[id].Background;
        string path = $"Background/{background}";

        if (background.Contains("gender"))
        {
            path = path.Replace("{gender}", GameManager.Inst.PlayerModel.Gender);
        }

        Sprite loadedSprite = await GameUtil.LoadSpriteOnly(path).AttachExternalCancellation(_backgroundToken.Token);

        if (VisualNovelManager.Inst.CurrentDialogueID == id && loadedSprite != null)
        {
            Image_Background.sprite = loadedSprite;
        }
    }

    // 캐릭터 스프라이트 변경
    private async UniTask ChangeSpeakerCharacter(string id)
    {
        CancelCharacter();
        _characterToken = new CancellationTokenSource();

        List<string> characters = _data[id].Facials;
        List<string> slots = _data[id].Slots;

        if (characters == null || characters.Count == 0)
        {
            CharacterContainer.gameObject.SetActive(false);
        }
        else
        {
            CharacterContainer.gameObject.SetActive(true);

            List<Sprite> loadSprites = new List<Sprite>();

            // 해당 캐릭터 스프라이트 저장
            for (int i = 0; i < characters.Count; i++)
            {
                string path = $"Character/{characters[i]}";

                if (characters[i].Contains("Player"))
                {
                    if (GameManager.Inst.PlayerModel.Gender == "Girl")
                    {
                        path = $"Character/Girl/{characters[i]}";
                    }
                    else
                    {
                        path = $"Character/Boy/{characters[i]}";
                    }
                }

                Sprite loadedSprite = await GameUtil.LoadSpriteOnly(path).AttachExternalCancellation(_characterToken.Token);
                loadSprites.Add(loadedSprite);
            }

            // 해당 슬롯에 이미지 저장
            if (VisualNovelManager.Inst.CurrentDialogueID == id)
            {
                InitImageSlot();

                for (int i = 0; i < loadSprites.Count; i++)
                {
                    Image targetSlot = GetTargetSlot(slots[i]);
                    targetSlot.gameObject.SetActive(true);

                    if (i != 0)
                    {
                        SetLowlight(targetSlot);
                    }

                    targetSlot.sprite = loadSprites[i];
                }
            }
        }
    }

    private Image GetTargetSlot(string slot)
    {
        if (slot == "C")
        {
            return Image_Center;
        }
        else if (slot == "R")
        {
            return Image_Right;
        }
        else if (slot == "L")
        {
            return Image_Left;
        }

        return null;
    }

    // 이미지 어둡게
    private void SetLowlight(Image targetImage)
    {
        targetImage.color = new Color(0.35f, 0.35f, 0.35f);
    }

    // 이미지 컴포넌트 초기화
    private void InitImageSlot()
    {
        Image_Center.gameObject.SetActive(false);
        Image_Right.gameObject.SetActive(false);
        Image_Left.gameObject.SetActive(false);

        Image_Center.color = Color.white;
        Image_Right.color = Color.white;
        Image_Left.color = Color.white;
    }

    // 다이얼로그 연출 커맨드
    // 카메라 흔들림
    private async UniTask ShakeCamera()
    {
        CancelShaking();
        _cameraShakeToken = new CancellationTokenSource();
        CancellationToken token = _cameraShakeToken.Token;

        float shakeTime = _totalShakeTime;
        Vector2 originPos = Rect.anchoredPosition;

        while (shakeTime > 0)
        {
            shakeTime -= Time.deltaTime;

            float progress = shakeTime / _totalShakeTime;
            Vector2 randomOffset = Random.insideUnitCircle * 30f * progress;

            Rect.anchoredPosition = originPos + randomOffset;

            await UniTask.Yield(token);
        }

        Rect.anchoredPosition = originPos;
    }

    private void ApplyCommand(string id)
    {
        if (_data[id].Command == "CameraShake")
        {
            ShakeCamera().Forget();
        }
    }

    // UniTask 토큰 취소
    private void CancelBackground()
    {
        if (_backgroundToken != null)
        {
            _backgroundToken.Cancel();
            _backgroundToken.Dispose();
            _backgroundToken = null;
        }
    }

    private void CancelCharacter()
    {
        if (_characterToken != null)
        {
            _characterToken.Cancel();
            _characterToken.Dispose();
            _characterToken = null;
        }
    }

    private void CancelShaking()
    {
        if (_cameraShakeToken != null)
        {
            _cameraShakeToken.Cancel();
            _cameraShakeToken.Dispose();
            _cameraShakeToken = null;
        }
    }
}