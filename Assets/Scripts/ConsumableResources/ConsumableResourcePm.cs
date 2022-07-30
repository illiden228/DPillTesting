using UnityEngine;
using UnityEngine.UI;

public class ConsumableResourcePm : BaseDisposable
{
    public struct Ctx
    {
        public IPoolObject<ConsumableResource> poolObject;
        public ConsumableInfo info;
    }

    private readonly Ctx _ctx;
    private ConsumableResource _view;

    public ConsumableType Type => _ctx.info.Type;
    public int Count => _ctx.info.Count;

    public ConsumableResourcePm(Ctx ctx)
    {
        _ctx = ctx;
    }

    public void Merge(ConsumableResourcePm merged)
    {
        _ctx.info.Count += merged._ctx.info.Count;
    }

    public void SetInWorld(Vector3 position)
    {
        _view = _ctx.poolObject.Get();
        ConsumableResource.Ctx consumableCtx = new ConsumableResource.Ctx
        {
            spawnPosition = position
        };
        _view.Init(consumableCtx);
        _view.Collided += OnCollided;
    }

    public void SetInBug()
    {
        _view.Collided -= OnCollided;
        _ctx.poolObject.Return(_view.gameObject);
        _view = null;
    }

    private void OnCollided(Collider collider)
    {
        if(collider.TryGetComponent(out UnitView unit))
        {
            unit.TouchResource(this);
        }
    }

    protected override void OnDispose()
    {
        base.OnDispose();
        _view.Collided -= OnCollided;
        _ctx.poolObject.Return(_view.gameObject);
    }
}