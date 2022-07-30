using UnityEngine;

public class EnemyPm : UnitPm
{
    public struct Ctx
    { }

    private readonly Ctx _ctx;

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
            Vector3 direction = target.Position - unitView.transform.position;
            if (canAttack && InAttackRadius(target.Position))
            {
                unitView.Attack(target);
                Attack(target);
            }
            else
            {
                unitView.RotateTo(direction);
                unitView.Move(direction);
            }
        }
    }
}