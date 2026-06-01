using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 드래그 할 때 생성되는 아이템
/// </summary>

public class DrawItem : UIBase
{
    [SerializeField] private Image Image_Item;

    private void Awake()
    {
        // 드랍 존 UI를 막지 않도록
        Image_Item.raycastTarget = false;
    }

    public void SetItemImage(string id)
    {
        string path = $"Icon/Item[{id}]";

        GameUtil.LoadSpriteAndSet(path, Image_Item).Forget();
    }
}