using UnityEngine;

public class ProjectilePm : BaseDisposable
{
    public struct Ctx
    {
        public IPoolObject<Projectile> poolObject;
        public ProjectileInfo info;
    }

    private Ctx _ctx;
    private UnitPm _target;
    private Projectile _projectileView;

    public bool IsDisposed => isDisposed;

    public ProjectilePm(Ctx ctx)
    {
        _ctx = ctx;
        _target = _ctx.info.target;
        _projectileView = _ctx.poolObject.Get();
        Projectile.Ctx projectileCtx = new Projectile.Ctx
        {
            speed = _ctx.info.speed,
            startDiretion = _ctx.info.startDirection,
            startPosition = _ctx.info.startPosition
        };
        _projectileView.Init(projectileCtx);
        _projectileView.Collided += OnCollided;
    }

    public void UpdateState()
    {
        if(_target == null)
            _projectileView.Move(_ctx.info.startDirection);
        else
        {
            Vector3 directionToTarget = _target.HitTransform.position - _projectileView.transform.position;
            _projectileView.RotateTo(directionToTarget);
            _projectileView.Move(directionToTarget);
        }
    }

    private void OnCollided(Collider collider)
    {
        if (collider.TryGetComponent(out UnitView unit))
        {
            unit.HitProjectile(_ctx.info.damage);
        }
        Dispose();
    }

    protected override void OnDispose()
    {
        base.OnDispose();
        _projectileView.Collided -= OnCollided;
        _ctx.poolObject.Return(_projectileView.gameObject);
    }
}