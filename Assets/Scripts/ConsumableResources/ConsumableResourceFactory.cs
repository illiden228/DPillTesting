using System;

public class ConsumableResourceFactory 
    : Factory<ConsumableType, ConsumableResource, ConsumableInfo, ConsumableResourcePm>
{

    public struct Ctx
    { }

    private readonly Ctx _ctx;
    private const int START_OBJECT_CAPACITY = 5; 
    
    private static readonly ResourceInfo _money = new ResourceInfo
    {
        Name = "Money",
        StorageName = "Prefabs/"
    };
    
    private static readonly ResourceInfo _crystal = new ResourceInfo
    {
        Name = "Crystal",
        StorageName = "Prefabs/"
    };
    
    public ConsumableResourceFactory(BaseCtx baseCtx, Ctx ctx) : base(baseCtx)
    {
        _ctx = ctx;
        CreatePools(START_OBJECT_CAPACITY);
    }

    public override ConsumableResourcePm Create(ConsumableType type, ConsumableInfo info)
    {
        ConsumableResourcePm.Ctx consumableCtx = new ConsumableResourcePm.Ctx
        {
            poolObject = pools[type],
            info = info
        };
        ConsumableResourcePm consumablePm = new ConsumableResourcePm(consumableCtx);
        return consumablePm;
    }

    protected override ResourceInfo GetResourceInfo(string type)
    {
        return (ConsumableType)Enum.Parse(typeof(ConsumableType), type) switch
        {
            ConsumableType.Money => _money,
            ConsumableType.Crystal => _crystal
        };
    }
}

public enum ConsumableType
{
    Money,
    Crystal
}