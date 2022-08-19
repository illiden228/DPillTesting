using System;
using UnityEngine;

[System.Serializable]
public abstract class UnitMovement : BaseMonoBehaviour
{
    public struct BaseCtx
    {
        public Action runAnumation;
        public Action idleAnimation;
    }
    
    [SerializeField] protected float Speed;
    private BaseCtx _baseCtx;
    private bool _isRun = false;

    private bool IsRun
    {
        set
        {
            if(value == _isRun)
                return;

            _isRun = value;
            ChangeAnimation();
        }
    }
    
    public virtual void Init(BaseCtx baseCtx)
    {
        _baseCtx = baseCtx;
        IsRun = false;
    }

    public virtual void SetSpeed(float speed)
    {
        Speed = speed;
    }

    public virtual void Move(Vector3 direction)
    {
        if (direction == Vector3.zero)
        {
            IsRun = false;
            StopMove();
            return;
        }

        MoveToDirection(direction);
        IsRun = true;
    }

    public virtual void RotateTo(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return;
        
        transform.forward = direction;
    }

    private void ChangeAnimation()
    {
        if(_isRun)
            _baseCtx.runAnumation?.Invoke();
        else
            _baseCtx.idleAnimation?.Invoke();
    }

    protected virtual void StopMove()
    {
    }

    protected virtual void MoveToDirection(Vector3 direction)
    {
        transform.position += direction * Speed * Time.deltaTime;
    }
}