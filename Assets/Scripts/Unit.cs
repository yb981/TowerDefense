using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public event Action OnStateChanged;

    [SerializeField] protected float defaultMovementSpeed = 2f;
    protected Health health;
   [SerializeField]  protected float attackRange;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected int attackDamage;

    protected UnitState state;

    public enum UnitState
    {
        spawn,
        idle,
        walk,
        chase,
        attack,
    }

    protected virtual void Start()
    {
        health = GetComponent<Health>();
        health.OnHealthChanged += Health_OnHealthChanged;
    }

    protected void MoveTo(Transform goalPosition)
    {
        if (goalPosition == null)
            return;

        Vector3 dir = goalPosition.position - transform.position;

        transform.Translate(dir.normalized * Time.deltaTime * defaultMovementSpeed);
    }

    protected void MoveTo(Vector3 goalPosition)
    {
        if (goalPosition == null)
            return;

        Vector3 dir = goalPosition - transform.position;

        transform.Translate(dir.normalized * Time.deltaTime * defaultMovementSpeed);
    }

    protected virtual void Health_OnHealthChanged()
    {
        if (health.GetHealth() <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
        gameObject.SetActive(false);
    }

    protected void ChangeState(UnitState newState)
    {
        state = newState;
        OnStateChanged?.Invoke();
    }

    public UnitState GetState()
    {
        return state;
    }

    public void AddDamage(int bonusDamage)
    {
        attackDamage += bonusDamage;
    }
}
