using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Inst;

    private Dictionary<string, AsyncOperationHandle> _handles = new Dictionary<string, AsyncOperationHandle>();

    private void Awake()
    {
        Inst = this;
    }

    public void LoadAsset<T>(string path, Action<T> callback) where T : UnityEngine.Object
    {
        if (_handles.TryGetValue(path, out AsyncOperationHandle handle))
        {
            callback?.Invoke(handle.Result as T);
            return;
        }

        AsyncOperationHandle<T> loadHandle = Addressables.LoadAssetAsync<T>(path);

        loadHandle.Completed += (op) =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                _handles[path] = op;
                callback?.Invoke(op.Result);
            }
        };
    }

    public void LoadSprite(string path, Action<Sprite> callback)
    {
        if (_handles.TryGetValue(path, out AsyncOperationHandle handle))
        {
            callback?.Invoke(handle.Result as Sprite);
            return;
        }

        AsyncOperationHandle<Sprite> handleOrigin = Addressables.LoadAssetAsync<Sprite>(path);

        handleOrigin.Completed += (op) =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                _handles[path] = op;
                callback?.Invoke(op.Result);
            }
        };
    }

    public void InstantiatePrefab(string path, Transform parent, Action<GameObject> callback)
    {
        AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(path, parent);

        handle.Completed += (op) =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                _handles[path] = op;
                callback?.Invoke(op.Result);
            }
        };
    }

    private void Release(string address)
    {
        if (_handles.TryGetValue(address, out AsyncOperationHandle handle))
        {
            Addressables.Release(address);
            _handles.Remove(address);
        }
    }
}
