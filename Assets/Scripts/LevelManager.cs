using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance {get; private set;}

    public event Action OnLevelPhasePlay;
    public event Action OnLevelPhaseBuild;

    public enum LevelPhase 
    {
        build,
        play,
        postplay,
    }

    private int credits;
    private LevelPhase levelPhase;

    private void Awake() 
    {
        instance = this;    
    }

    private void ChangeLevelPhase(LevelPhase newPhase)
    {
        switch(newPhase)
        {
            case LevelPhase.build: OnLevelPhaseBuild?.Invoke(); break;
            case LevelPhase.play: OnLevelPhasePlay?.Invoke(); break;
            default: break;
        }
    }

    public LevelPhase GetLevelPhase()
    {
        return levelPhase;
    }

    public void StartWave()
    {
        ChangeLevelPhase(LevelPhase.play);
    }
}
