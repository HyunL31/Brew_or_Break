using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// 리소스 로드 매니저 (어드레서블)
/// </summary>

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Inst;

    private Dictionary<string, AsyncOperationHandle> _handles = new Dictionary<string, AsyncOperationHandle>();

    private void Awake()
    {
        Inst = this;
    }

    public async UniTask<T> LoadAsset<T>(string path) where T : UnityEngine.Object
    {
        if (_handles.TryGetValue(path, out AsyncOperationHandle handle))
        {
            return handle.Result as T;
        }

        AsyncOperationHandle<T> loadHandle = Addressables.LoadAssetAsync<T>(path);

        T result = await loadHandle.ToUniTask();

        _handles[path] = loadHandle;
        return result;
    }

    public async UniTask<Sprite> LoadSprite(string path)
    {
        if (_handles.TryGetValue(path, out AsyncOperationHandle handle))
        {
            return handle.Result as Sprite;
        }

        AsyncOperationHandle<Sprite> handleOrigin = Addressables.LoadAssetAsync<Sprite>(path);

        Sprite result = await handleOrigin.ToUniTask();

        _handles[path] = handleOrigin;

        return result;
    }

    public async UniTask<GameObject> InstantiatePrefab(string path, Transform parent)
    {
        AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(path, parent);

        GameObject instance = await handle.ToUniTask();

        return instance;
    }

    public void Release(string address)
    {
        if (_handles.TryGetValue(address, out AsyncOperationHandle handle))
        {
            Addressables.Release(address);
            _handles.Remove(address);
        }
    }
}
