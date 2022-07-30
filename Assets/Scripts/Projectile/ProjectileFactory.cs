using System;

public class ProjectileFactory : Factory<ProjectileType, Projectile, ProjectileInfo, ProjectilePm>
{
    public struct Ctx
    { }

    private Ctx _ctx;
    private const int START_OBJECT_CAPACITY = 5; 
    
    public event Action<ProjectilePm> Created;

    private static readonly ResourceInfo _empty = new ResourceInfo
    {
        Name = "EmptyProjectile",
        StorageName = "Prefabs/"
    };
    
    private static readonly ResourceInfo _bullet = new ResourceInfo
    {
        Name = "Bullet",
        StorageName = "Prefabs/"
    };

    public ProjectileFactory(BaseCtx baseCtx, Ctx ctx) : base(baseCtx)
    {
        _ctx = ctx;
        CreatePools(START_OBJECT_CAPACITY);
    }

    public override ProjectilePm Create(ProjectileType type, ProjectileInfo info)
    {
        ProjectilePm.Ctx projectileCtx = new ProjectilePm.Ctx
        {
            poolObject = pools[type],
            info = info
        };
        ProjectilePm projectilePm = new ProjectilePm(projectileCtx);
        Created?.Invoke(projectilePm);
        return projectilePm;
    }

    protected override ResourceInfo GetResourceInfo(string type)
    {
        return (ProjectileType)Enum.Parse(typeof(ProjectileType), type) switch
        {
            ProjectileType.Empty => _empty,
            ProjectileType.Bullet => _bullet
        };
    }
}

public enum ProjectileType
{
    Empty,
    Bullet
}