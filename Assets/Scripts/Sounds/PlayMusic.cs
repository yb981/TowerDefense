using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    
    [SerializeField] private AudioClip music;

    void Start()
    {
        SoundManager.Instance.PlayMusic(music);
    }
}
