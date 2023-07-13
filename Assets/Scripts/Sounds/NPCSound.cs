using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSound : MonoBehaviour
{
    [SerializeField] private AudioClip damagedClip;
    [SerializeField] private AudioClip attackClip;
    //[SerializeField] private AudioClip dieClip;

    private void Start() 
    {
        GetComponent<Health>().OnDamageTaken += Health_OnDamageTaken;
        GetComponent<NPC>().OnAttacked += NPC_OnAttacked;
    }

    private void NPC_OnAttacked()
    {
        SoundManager.Instance.PlaySound(attackClip);
    }

    private void Health_OnDamageTaken()
    {
        SoundManager.Instance.PlaySound(damagedClip);
    }
}