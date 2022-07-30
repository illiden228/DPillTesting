using System;
using UnityEngine;

public class Projectile : BaseMonoBehaviour
{
    public struct Ctx
    {
        public Vector3 startPosition;
        public Vector3 startDiretion;
        public float speed;
    }
    
    [SerializeField] private ProjectileMovement _movement;
    private Ctx _ctx;

    public Action<Collider> Collided;

    public void Init(Ctx ctx)
    {
        _ctx = ctx;
        transform.SetParent(null);
        transform.position = _ctx.startPosition;
        _movement.RotateTo(_ctx.startDiretion);
        _movement.SetSpeed(_ctx.speed);
        gameObject.SetActive(true);
    }
    
    public void Move(Vector3 direction)
    {
        _movement.Move(direction);
    }
    
    public void RotateTo(Vector3 direction)
    {
        _movement.RotateTo(direction);
    }

    private void OnTriggerEnter(Collider other)
    {
        Collided?.Invoke(other);
    }
}