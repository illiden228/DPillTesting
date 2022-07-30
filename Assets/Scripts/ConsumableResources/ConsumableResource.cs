using System;
using UnityEngine;

public class ConsumableResource : BaseMonoBehaviour
{
    public struct Ctx
    {
        public Vector3 spawnPosition;
    }

    private Ctx _ctx;

    public event Action<Collider> Collided;

    public void Init(Ctx ctx)
    {
        _ctx = ctx;
        transform.SetParent(null);
        transform.position = _ctx.spawnPosition;
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        Collided?.Invoke(other);
    }
}