using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonSound : MonoBehaviour
{
    [SerializeField] private AudioClip click;
    
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => SoundManager.Instance.PlaySound(click));
    }
}
