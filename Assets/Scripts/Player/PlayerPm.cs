using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerPm : UnitPm
{
    public struct Ctx
    {
        public CinemachineVirtualCamera camera;
        public Canvas canvasForJoystick;
        public float baseBorderCoord;
    }

    private readonly Ctx _ctx;
    private JoystickPm _joystick;
    private ProjectilePm _projectile;

    public bool InHome => Position.z < _ctx.baseBorderCoord;

    public PlayerPm(BaseCtx baseCtx, Ctx ctx) : base(baseCtx) 
    {
        _ctx = ctx;
        currentProjectileType = ProjectileType.Bullet;
        
        _ctx.camera.Follow = unitView.transform;
        JoystickPm.Ctx joystickCtx = new JoystickPm.Ctx
        {
            resourceLoader = baseCtx.resourceLoader,
            parent = _ctx.canvasForJoystick,
        };
        _joystick = new JoystickPm(joystickCtx);
        AddDispose(_joystick);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if(!isAlive)
            return;
        
        unitView.Move(_joystick.Direction);

        if (target != null)
        {
            Vector3 targetDirection = target.Position - Position;
            unitView.RotateTo(targetDirection);
            if (canAttack)
            {
                unitView.Attack(target);
                Attack(target);
            }
        }
        else
        {
            unitView.RotateTo(_joystick.Direction);
        }
    }

    public List<ConsumableResourcePm> TakeAllResources()
    {
        return _bug.TakeAll();
    }

    public override void OnTouchedResource(ConsumableResourcePm resource)
    {
        _bug.Put(resource);
        resource.SetInBug();
    }
}