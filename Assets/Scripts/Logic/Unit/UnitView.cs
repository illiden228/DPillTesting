using System;
using UnityEngine;

public abstract class UnitView : BaseMonoBehaviour
{
    public struct BaseCtx
    {
        public float speed;
        public Vector3 spawnPosition;
    }
    
    private BaseCtx baseCtx;
    [SerializeField] private UnitMovement _movement;
    [SerializeField] private Transform _projectileSpawn;
    [SerializeField] private Transform _hitTransform;
    protected UnitMovement movement => _movement;

    public Transform ProjectileSpawn => _projectileSpawn;
    public Transform HitTransform => _hitTransform;

    public event Action<float> ProjectileHit;
    public event Action<ConsumableResourcePm> TouchedResource;

    public void Init(BaseCtx baseCtx)
    {
        this.baseCtx = baseCtx;
        _movement.SetSpeed(baseCtx.speed);
        transform.position = baseCtx.spawnPosition;
        transform.SetParent(null);
        gameObject.SetActive(true);
    }

    public void OnSpeedChanged(float speed)
    {
        _movement.SetSpeed(speed);
    }

    public virtual void Move(Vector3 direction)
    {
        movement.Move(direction);
    }

    public virtual void RotateTo(Vector3 direction)
    {
        movement.RotateTo(direction);
    }

    public virtual void Attack(UnitPm unit)
    {
    }

    public void HitProjectile(float damage)
    {
        ProjectileHit?.Invoke(damage);
    }

    public void TouchResource(ConsumableResourcePm resourcePm)
    {
        TouchedResource?.Invoke(resourcePm);
    }
}