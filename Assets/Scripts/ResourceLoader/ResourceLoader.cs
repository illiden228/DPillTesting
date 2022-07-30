using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class ResourceLoader : BaseDisposable, IResourceLoader
{
    private readonly Dictionary<string, GameObject> _prefabsCache;

    public ResourceLoader()
    {
        _prefabsCache = new Dictionary<string, GameObject>();
    }
    
    public void LoadPrefab(ResourceInfo resourceInfo, Action<GameObject> onLoad)
    {
        LoadResource<GameObject>(resourceInfo, _prefabsCache, onLoad);
    }

    private void LoadResource<T>(ResourceInfo resourceInfo, IDictionary<string, T> cache, Action<T> onLoad) where T : Object
    {
        string resourcePath = $"{resourceInfo.StorageName}{resourceInfo.Name}";
        T resource;
        if (cache.TryGetValue(resourcePath, out resource))
        {
            onLoad?.Invoke(resource);
            return;
        }
        
        resource = Resources.Load<T>(resourcePath);
        cache[resourcePath] = resource;
        onLoad?.Invoke(resource);
    }
}