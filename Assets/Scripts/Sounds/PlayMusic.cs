using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{

    [SerializeField] private AudioClip[] music;
    private int currentClip;

    void Start()
    {
        SoundManager.Instance.PlayMusic(ChooseRandomSong());
    }

    private void Update()
    {
        if (!SoundManager.Instance.IsMusicPlaying())
        {
            SoundManager.Instance.PlayMusic(NextAudioClip());
        }
    }

    private AudioClip ChooseRandomSong()
    {
        currentClip = Random.Range(0, music.Length);
        return music[currentClip];
    }

    private AudioClip NextAudioClip()
    {
        if (music.Length > 1)
        {
            int newClipIndex;
            do
            {
                newClipIndex = Random.Range(0, music.Length);
            }while(newClipIndex == currentClip);

            currentClip = newClipIndex;
            return music[currentClip];
        }

        return music[0];
    }
}
