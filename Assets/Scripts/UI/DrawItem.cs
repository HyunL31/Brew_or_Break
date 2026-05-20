using UnityEngine;
using UnityEngine.UI;

public class DrawItem : UIBase
{
    [SerializeField] private Image Image_Item;

    public void SetItemImage(string id)
    {
        string path = $"Icon/Item[{id}]";

        GameUtil.LoadSpriteAndSet(path, Image_Item);
    }
}