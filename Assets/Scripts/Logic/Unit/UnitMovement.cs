using UnityEngine;

[System.Serializable]
public abstract class UnitMovement : BaseMonoBehaviour
{
    [SerializeField]protected float Speed;

    public virtual void SetSpeed(float speed)
    {
        Speed = speed;
    }

    public virtual void Move(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return;
        
        transform.position += direction * Speed * Time.deltaTime;
    }

    public virtual void RotateTo(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return;
        
        transform.forward = direction;
    }
}