using System;
using Cinemachine;
using UnityEngine;

public class UnitFactory : Factory<UnitType, UnitView, UnitInfo, UnitPm>
{
    public struct Ctx
    {
        public CinemachineVirtualCamera playerCamera;
        public Canvas canvasForJoystick;
        public float baseBorderCoord;
    }

    private readonly Ctx _ctx;
    private const int START_OBJECT_CAPACITY = 5; 
    
    private static readonly ResourceInfo _enemy = new ResourceInfo
    {
        Name = "Enemy",
        StorageName = "Prefabs/"
    };
    
    private static readonly ResourceInfo _player = new ResourceInfo
    {
        Name = "Player",
        StorageName = "Prefabs/"
    };
    
    public UnitFactory(BaseCtx baseCtx, Ctx ctx) : base(baseCtx)
    {
        _ctx = ctx;
        CreatePools(START_OBJECT_CAPACITY);
    }

    public override UnitPm Create(UnitType type, UnitInfo info)
    {
        UnitPm.BaseCtx unitBaseCtx = new UnitPm.BaseCtx
        {
            resourceLoader = baseCtx.resourceLoader,
            info = info,
            poolObject = pools[type],
            deathAnimationTrigger = CharacterAnimations.FAILLING_DEATH_TRIGGER,
            idleAnimationTrigger = CharacterAnimations.IDLE_TRIGGER,
            runAnimationTrigger = CharacterAnimations.RUNNING_TRIGGER
        };

        switch (type)
        {
            case UnitType.Enemy:
                return new EnemyPm(new EnemyPm.Ctx(), unitBaseCtx);;
            case UnitType.Player:
                PlayerPm.Ctx playerCtx = new PlayerPm.Ctx
                {
                    camera = _ctx.playerCamera,
                    canvasForJoystick = _ctx.canvasForJoystick,
                    baseBorderCoord = _ctx.baseBorderCoord
                };
                return new PlayerPm(unitBaseCtx, playerCtx);
        }
        
        Debug.LogError($"Unit type is {type} not found");
        return null;
    }

    protected override ResourceInfo GetResourceInfo(string type)
    {
        return (UnitType)Enum.Parse(typeof(UnitType), type) switch
        {
            UnitType.Enemy => _enemy,
            UnitType.Player => _player
        };
    }
}

public enum UnitType
{
    Player,
    Enemy
}