using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class VisualNovelUI : UIBase
{
    [SerializeField] private Image Image_Background;
    [SerializeField] private Image Image_Character;

    private Dictionary<string, Dialogue> _data;

    private CancellationTokenSource _backgroundToken;
    private CancellationTokenSource _characterToken;

    private void Awake()
    {
        _data = GameDataManager.Inst.DialogueDataList;

        VisualNovelManager.Inst.OnChangeBaseUI += (id) => ChangeBackgroundImage(id).Forget();
        VisualNovelManager.Inst.OnChangeBaseUI += (id) => ChangeSpeakerCharacter(id).Forget();
    }

    private async UniTask ChangeBackgroundImage(string id)
    {
        CancelBackground();
        _backgroundToken = new CancellationTokenSource();

        string background = _data[id].Background;
        string path = $"Background/{background}";

        await GameUtil.LoadSpriteAndSet(path, Image_Background).AttachExternalCancellation(_backgroundToken.Token);
    }

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

            await GameUtil.LoadSpriteAndSet(path, Image_Character).AttachExternalCancellation(_characterToken.Token);
        }
    }

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
}