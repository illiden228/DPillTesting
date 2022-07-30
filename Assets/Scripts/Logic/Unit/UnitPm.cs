using System;
using UnityEngine;

public abstract class UnitPm : BaseDisposable
{
    public struct BaseCtx
    {
        public IResourceLoader resourceLoader;
        public IPoolObject<UnitView> poolObject;
        public UnitInfo info;
    }

    protected BaseCtx baseCtx;
    protected float hp;
    protected float currentSpeed;
    protected float currentDamage;
    protected float currentAttackSpeed;
    protected UnitPm target;
    protected bool isAlive = true;
    protected float attackTimer = 0;
    protected bool canAttack = false;
    protected UnitView unitView;
    protected ProjectileType currentProjectileType;
    protected BugPm _bug;
    
    public Action<float> HpChanged;

    public float BaseDamage => baseCtx.info.damage;
    public bool CanAttack => canAttack;
    public bool IsAlive => isAlive;
    public Vector3 Position => unitView.transform.position;
    public Transform HitTransform => unitView.HitTransform;
    public bool IsDisposed => isDisposed;
    public float Hp
    {
        get => hp;
        private set
        {
            hp = Mathf.Max(0, value);
            HpChanged?.Invoke(hp);
        }
    }

    protected UnitPm(BaseCtx baseCtx)
    {
        this.baseCtx = baseCtx;
        Hp = baseCtx.info.maxHp;
        currentDamage = baseCtx.info.damage;
        currentSpeed = baseCtx.info.moveSpeed;
        currentAttackSpeed = baseCtx.info.attackSpeed;

        unitView = baseCtx.poolObject.Get();
        UnitView.BaseCtx unitCtx = new UnitView.BaseCtx
        {
            speed = currentSpeed,
            spawnPosition = baseCtx.info.spawnPoint
        };
        unitView.Init(unitCtx);
        unitView.ProjectileHit += TakeDamage;
        unitView.TouchedResource += OnTouchedResource;

        _bug = new BugPm(new BugPm.Ctx{ ownerTransform = unitView.transform});
        _bug.Put(baseCtx.info.startResources);
    }


    public virtual void UpdateState()
    {
        if (!canAttack)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
                canAttack = true;
        }
    }

    public virtual void Attack(UnitPm unit)
    {
        Vector3 direction = unit.Position - Position;
        ProjectileInfo projectileInfo = new ProjectileInfo
        {
            damage = baseCtx.info.damage,
            speed = 13f,
            target = unit,
            startDirection = direction,
            startPosition = unitView.ProjectileSpawn.position
        };
        baseCtx.info.projectileFactory.Create(currentProjectileType, projectileInfo);
        attackTimer = currentAttackSpeed;
        canAttack = false;
    }

    public virtual void TakeDamage(float damage)
    {
        if(!isAlive)
            return;
        
        hp -= damage;
        Debug.Log($"{unitView.name } получил удар {damage}, hp = {hp}" );
            
        if (Hp <= 0)
            Died();
    }

    public virtual void Died()
    {
        isAlive = false;
        Dispose();
        Debug.Log("умер " + unitView.name);
    }

    public virtual bool InAttackRadius(Vector3 targetPos)
    {
        return InAttackRadius(targetPos, out float sqrDistance);
    }
    
    public virtual bool InAttackRadius(Vector3 targetPos, out float sqrDistance)
    {
        Vector3 distance = targetPos - unitView.transform.position;
        sqrDistance = distance.sqrMagnitude;
        return sqrDistance < baseCtx.info.attackRadius * baseCtx.info.attackRadius;
    }

    public virtual void SetTarget(UnitPm unit)
    {
        target = unit;
    }

    public virtual void OnTouchedResource(ConsumableResourcePm resource)
    { }

    protected override void OnDispose()
    {
        base.OnDispose();
        _bug.AbortAll();
        _bug.Dispose();
        unitView.ProjectileHit -= TakeDamage;
        unitView.TouchedResource -= OnTouchedResource;
        baseCtx.poolObject.Return(unitView.gameObject);
    }
}