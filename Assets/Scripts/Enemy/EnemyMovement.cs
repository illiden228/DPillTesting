using UnityEngine;

public class EnemyMovement : UnitMovement
{
    [SerializeField] private Rigidbody _rigidbody;

    protected override void MoveToDirection(Vector3 direction)
    {
        Vector3 move = direction * Speed * Time.fixedDeltaTime;
        _rigidbody.velocity = move;
    }

    protected override void StopMove()
    {
        
    }
}