using System.Collections.Generic;

public class ProjectileController : BaseDisposable
{
    public struct Ctx
    {
        public IResourceLoader resourceLoader;
    }

    private readonly Ctx _ctx;
    private ProjectileFactory _projectileFactory;
    private List<ProjectilePm> _projectiles;

    public ProjectileFactory Factory => _projectileFactory;

    public ProjectileController(Ctx ctx)
    {
        _ctx = ctx;
        _projectiles = new List<ProjectilePm>();
        
        Factory<ProjectileType, Projectile, ProjectileInfo, ProjectilePm>.BaseCtx projectileFabricCtx = 
            new Factory<ProjectileType, Projectile, ProjectileInfo, ProjectilePm>.BaseCtx
        {
            resourceLoader = _ctx.resourceLoader
        };
        _projectileFactory = new ProjectileFactory(projectileFabricCtx, new ProjectileFactory.Ctx());
        AddDispose(_projectileFactory);

        _projectileFactory.Created += _projectiles.Add;
    }
    
    public void UpdateStates()
    {
        ClearDisposed();
        foreach (var projectile in _projectiles)
        {
            projectile.UpdateState();
        }
    }
    
    private void ClearDisposed()
    {
        for (int i = 0; i < _projectiles.Count; i++)
        {
            if (_projectiles[i].IsDisposed)
                _projectiles.Remove(_projectiles[i]);
        }
    }

    protected override void OnDispose()
    {
        base.OnDispose();
        _projectileFactory.Created -= _projectiles.Add;
    }
}