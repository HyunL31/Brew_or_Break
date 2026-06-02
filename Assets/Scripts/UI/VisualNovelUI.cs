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
    [SerializeField] private Image Image_Character;
    [SerializeField] private Image Image_SecondSpeaker;

    private Dictionary<string, Dialogue> _data;

    private CancellationTokenSource _backgroundToken;
    private CancellationTokenSource _characterToken;
    private CancellationTokenSource _secondSpeakerToken;

    private void Awake()
    {
        _data = GameDataManager.Inst.DialogueDataList;

        VisualNovelManager.Inst.OnChangeBaseUI += (id) => ChangeBackgroundImage(id).Forget();
        VisualNovelManager.Inst.OnChangeBaseUI += (id) => ChangeSpeakerCharacter(id).Forget();
        VisualNovelManager.Inst.OnChangeBaseUI += (id) => ChangeSecondSpeaker(id).Forget();
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

        Sprite downloadedSprite = await GameUtil.LoadSpriteOnly(path).AttachExternalCancellation(_backgroundToken.Token);

        if (VisualNovelManager.Inst.CurrentDialogueID == id && downloadedSprite != null)
        {
            Image_Background.sprite = downloadedSprite;
        }
    }

    // 캐릭터 스프라이트 변경
    private async UniTask ChangeSpeakerCharacter(string id)
    {
        CancelCharacter();
        _characterToken = new CancellationTokenSource();

        string characterFacial = _data[id].Facial;

        if (characterFacial == string.Empty)
        {
            Image_Character.gameObject.SetActive(false);
        }
        else
        {
            Image_Character.gameObject.SetActive(true);

            string path = $"Character/{characterFacial}";

            if (characterFacial.Contains("Player"))
            {
                if (GameManager.Inst.PlayerModel.Gender == "Girl")
                {
                    path = $"Character/Girl/{characterFacial}";
                }
                else
                {
                    path = $"Character/Boy/{characterFacial}";
                }
            }

            Sprite downloadedSprite = await GameUtil.LoadSpriteOnly(path).AttachExternalCancellation(_characterToken.Token);

            if (VisualNovelManager.Inst.CurrentDialogueID == id && downloadedSprite != null)
            {
                Image_Character.sprite = downloadedSprite;
            }
        }
    }

    // 두 번째 캐릭터 스프라이트 변경
    private async UniTask ChangeSecondSpeaker(string id)
    {
        CancelSecondSpeaker();
        _secondSpeakerToken = new CancellationTokenSource();

        string characterFacial = _data[id].SecondSpeaker;

        if (characterFacial == string.Empty)
        {
            Image_SecondSpeaker.gameObject.SetActive(false);
        }
        else
        {
            Image_SecondSpeaker.gameObject.SetActive(true);

            string path = $"Character/{characterFacial}";

            if (characterFacial.Contains("Player"))
            {
                if (GameManager.Inst.PlayerModel.Gender == "Girl")
                {
                    path = $"Character/Girl/{characterFacial}";
                }
                else
                {
                    path = $"Character/Boy/{characterFacial}";
                }
            }

            Sprite downloadedSprite = await GameUtil.LoadSpriteOnly(path).AttachExternalCancellation(_secondSpeakerToken.Token);

            if (VisualNovelManager.Inst.CurrentDialogueID == id && downloadedSprite != null)
            {
                Image_SecondSpeaker.sprite = downloadedSprite;
            }
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

    private void CancelSecondSpeaker()
    {
        if (_secondSpeakerToken != null)
        {
            _secondSpeakerToken.Cancel();
            _secondSpeakerToken.Dispose();
            _secondSpeakerToken = null;
        }
    }
}