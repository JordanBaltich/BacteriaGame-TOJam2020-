using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MinionData", order = 1)]
public class MinionData : ScriptableObject
{
    public float maxHealth;
    public float attackPower;
    public float moveSpeed;
    public float attackRange;
    public float attackCooldown;
}
