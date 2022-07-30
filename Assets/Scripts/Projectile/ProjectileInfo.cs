using UnityEngine;

public class ProjectileInfo : IInfo
{
    public UnitPm target;
    public Vector3 startDirection;
    public Vector3 startPosition;
    public float damage;
    public float speed;
}