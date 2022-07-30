using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolObject<T> : BaseDisposable, IPoolObject<T>
{
    public struct Ctx
    {
        public Transform parent;
        public GameObject prefab;
        public int startCapacity;
    }

    private readonly Ctx _ctx;
    private List<GameObject> _pool;

    public PoolObject(Ctx ctx)
    {
        _ctx = ctx;
        _pool = new List<GameObject>(_ctx.startCapacity);
        for (int i = 0; i < _ctx.startCapacity; i++)
        {
            GameObject newPoolObject = GameObject.Instantiate(_ctx.prefab, _ctx.parent);
            newPoolObject.SetActive(false);
            _pool.Add(newPoolObject);
        }
    }

    public T Get()
    {
        GameObject poolObject = _pool.FirstOrDefault(p => p.activeSelf == false);
        if (poolObject == null)
        {
            poolObject = GameObject.Instantiate(_ctx.prefab, _ctx.parent);
            _pool.Add(poolObject);
        }
        return poolObject.GetComponent<T>();
    }

    public void Return(GameObject poolObject)
    {
        poolObject.SetActive(false);
        poolObject.transform.SetParent(_ctx.parent);
    }

    protected override void OnDispose()
    {
        base.OnDispose();
        foreach (var gameObject in _pool)
        {
            GameObject.Destroy(gameObject);
        }
    }
}