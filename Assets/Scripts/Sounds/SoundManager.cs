using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance {get; private set;}

    [SerializeField] private AudioSource musicSource, effectSource;

    private void Awake() 
    {
        HandleSingleton();
    }

    private void Start() 
    {
        LoadPlayerPrefs();    
    }

    private void LoadPlayerPrefs()
    {
        if(PlayerPrefs.HasKey(GameConstants.MUSIC_VOLUME))
        {
            ChangeMusicVolume(PlayerPrefs.GetFloat(GameConstants.MUSIC_VOLUME));
            ChangeEffectsVolume(PlayerPrefs.GetFloat(GameConstants.EFFECTS_VOLUME));
        }
    }

    public void PlaySound(AudioClip clip)
    {
        effectSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip music)
    {
        musicSource.clip = music;
        musicSource.Play();
    }

    public void ChangeMasterVolume(float value)
    {
        AudioListener.volume = value;
    }

    public void ChangeMusicVolume(float value)
    {
        musicSource.volume = value;
    }

    public void ChangeEffectsVolume(float value)
    {
        effectSource.volume = value;
    }

    public float GetMusicVolume()
    {
        return musicSource.volume;
    }

    public float GetEffectsVolume()
    {
        return effectSource.volume;
    }

    private void HandleSingleton()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }
    }
}
