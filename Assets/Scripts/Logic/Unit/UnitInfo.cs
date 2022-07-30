using System.Collections.Generic;
using UnityEngine;

public class UnitInfo : IInfo
{
    public ProjectileFactory projectileFactory;
    public Vector3 spawnPoint;
    public float maxHp;
    public float moveSpeed;
    public float damage;
    public float attackRadius;
    public float attackSpeed;
    public List<ConsumableResourcePm> startResources;
}