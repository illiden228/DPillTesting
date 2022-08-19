using UnityEngine;

public class EnemyPm : UnitPm
{
    public struct Ctx
    { }

    private readonly Ctx _ctx;
    private Vector3 _moveDirection;

    public EnemyPm(Ctx ctx, BaseCtx baseCtx) : base(baseCtx)
    {
        _ctx = ctx;
        currentProjectileType = ProjectileType.Empty;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if(!isAlive)
            return;
        
        if (target != null)
        {
            _moveDirection = (target.Position - unitView.transform.position).normalized;
            if (canAttack && InAttackRadius(target.Position))
            {
                unitView.Attack(target);
                Attack(target);
            }
            else
            {
                unitView.RotateTo(_moveDirection);
            }
        }
        else
        {
            _moveDirection = Vector3.zero;
        }
    }

    public override void UpdatePhysicState()
    {
        unitView.Move(_moveDirection); 
    }
}