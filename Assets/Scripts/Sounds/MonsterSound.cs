using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSound : MonoBehaviour
{
    [SerializeField] private AudioClip damagedClip;
    [SerializeField] private AudioClip attackClip;
    //[SerializeField] private AudioClip dieClip;

    private void Start() 
    {
        GetComponent<Health>().OnHealthChanged += Health_OnHealthChanged;
        GetComponent<Monster>().OnMonsterAttacked += Monster_OnMonsterAttacked;
    }

    private void Monster_OnMonsterAttacked()
    {
        SoundManager.Instance.PlaySound(attackClip);
    }

    private void Health_OnHealthChanged()
    {
        SoundManager.Instance.PlaySound(damagedClip);
    }
}
