using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MonsterStatsSO : ScriptableObject
{
    [SerializeField] public int rewardCreditsOnKill;
    [SerializeField] public int rewardScoreOnKill;
    [SerializeField] public float triggerRange = 5f;
    [SerializeField] public float attackRange = 1f;
    [SerializeField] public float attackSpeed = 3f;
    [SerializeField] public int attackDamage = 1;
    [SerializeField] public float currentMovementSpeed = 2f;
}
