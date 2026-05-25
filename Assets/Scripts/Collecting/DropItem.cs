using Cysharp.Threading.Tasks;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer Sprite_Item;

    private int _instanceID;
    private string _itemID;

    public void SetItemID(string id)
    {
        _instanceID = GetComponent<Collider2D>().GetInstanceID();
        _itemID = id;
    }

    public int GetInstancedID()
    {
        return _instanceID;
    }

    public string GetItemID()
    {
        return _itemID;
    }

    public async UniTask SetImage()
    {
        string path = $"Icon/Item[{_itemID}]";

        await GameUtil.LoadTextureAndSet(path, Sprite_Item);
    }
}
