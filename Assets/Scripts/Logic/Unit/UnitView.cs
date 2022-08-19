using System;
using UnityEngine;

public abstract class UnitView : BaseMonoBehaviour
{
    public struct BaseCtx
    {
        public float speed;
        public Vector3 spawnPosition;
        public string runAnimationTrigger;
        public string idleAnimationTrigger;
        public string deathAnimationTrigger;
    }
    
    private BaseCtx _baseCtx;
    [SerializeField] private UnitMovement _movement;
    [SerializeField] private Transform _projectileSpawn;
    [SerializeField] private Transform _hitTransform;
    [SerializeField] private Animator _animator;
    protected UnitMovement movement => _movement;

    public Transform ProjectileSpawn => _projectileSpawn;
    public Transform HitTransform => _hitTransform;

    public event Action<float> ProjectileHit;
    public event Action<ConsumableResourcePm> TouchedResource;

    public void Init(BaseCtx baseCtx)
    {
        _baseCtx = baseCtx;
        transform.position = baseCtx.spawnPosition;
        transform.SetParent(null);
        gameObject.SetActive(true);
        
        _movement.Init(new UnitMovement.BaseCtx
        {
            idleAnimation = () => _animator.SetTrigger(_baseCtx.idleAnimationTrigger),
            runAnumation = () => _animator.SetTrigger(_baseCtx.runAnimationTrigger)
        });
        _movement.SetSpeed(baseCtx.speed);
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

    public void OnDied()
    {
        _animator.SetTrigger(_baseCtx.deathAnimationTrigger);
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