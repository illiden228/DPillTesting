using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class RandomResourceGenerator : BaseDisposable
{
    public struct Ctx
    {
        public ResourceLoader resourceLoader;
        public int minCountMoney;
        public int maxCountMoney;
        public int minCountCrystal;
        public int maxCountCrystal;
    }

    private Ctx _ctx;
    private ConsumableResourceFactory _resourceFactory;

    public RandomResourceGenerator(Ctx ctx)
    {
        _ctx = ctx;
        
        ConsumableResourceFactory.BaseCtx resourceFactoryBaseCtx =
            new Factory<ConsumableType, ConsumableResource, ConsumableInfo, ConsumableResourcePm>.BaseCtx
            {
                resourceLoader = _ctx.resourceLoader
            };
        _resourceFactory = new ConsumableResourceFactory(resourceFactoryBaseCtx, new ConsumableResourceFactory.Ctx());
        AddDispose(_resourceFactory);
    }

    public List<ConsumableResourcePm> GetResourceList()
    {
        List<ConsumableResourcePm> resources = new List<ConsumableResourcePm>();
        int countConsumableType = Enum.GetNames(typeof(ConsumableType)).Length;
        ConsumableType randomType = (ConsumableType)Random.Range(0, countConsumableType);
        ConsumableInfo resourceInfo = new ConsumableInfo
        {
            Count = GetRandomCount(randomType),
            Type = randomType
        };
        resources.Add(_resourceFactory.Create(randomType, resourceInfo));
        return resources;
    }

    private int GetRandomCount(ConsumableType type)
    {
        return type switch
        {
            ConsumableType.Crystal => Random.Range(_ctx.minCountCrystal, _ctx.maxCountCrystal),
            ConsumableType.Money => Random.Range(_ctx.minCountMoney, _ctx.maxCountMoney),
        };
    }
}