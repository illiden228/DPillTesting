using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Factory<Type, PoolType, IInfo, TReturn> : BaseDisposable
    where Type : Enum
{
    public struct BaseCtx
    {
        public IResourceLoader resourceLoader;
    }

    protected BaseCtx baseCtx;
    protected Dictionary<Type, IPoolObject<PoolType>> pools;

    protected Factory(BaseCtx baseCtx)
    {
        this.baseCtx = baseCtx;
        pools = new Dictionary<Type, IPoolObject<PoolType>>();
    }

    public abstract TReturn Create(Type type, IInfo info);
    protected abstract ResourceInfo GetResourceInfo(string type);

    protected virtual void CreatePools(int objectCapacity)
    {
        var types = Enum.GetNames(typeof(Type));
        foreach (var type in types)
        {
            ResourceInfo resource = GetResourceInfo(type);
            baseCtx.resourceLoader.LoadPrefab(resource, prefab =>
            {
                Transform parent = new GameObject($"pool {type}").transform;
                PoolObject<PoolType>.Ctx newPoolObjectCtx = new PoolObject<PoolType>.Ctx
                {
                    parent = parent,
                    prefab = prefab,
                    startCapacity = objectCapacity
                };
                var typeObj = Enum.Parse(typeof(Type), type);
                
                pools[(Type) typeObj] = new PoolObject<PoolType>(newPoolObjectCtx);
            });
        }
    }
}