using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance {get; private set;}

    public event Action OnLevelPhasePlay;
    public event Action OnLevelPhaseBuild;
    public event Action OnLevelPhasePostPlay;

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
        levelPhase = newPhase;
        switch(newPhase)
        {
            case LevelPhase.build: OnLevelPhaseBuild?.Invoke(); break;
            case LevelPhase.play: OnLevelPhasePlay?.Invoke(); break;
            case LevelPhase.postplay: OnLevelPhasePostPlay?.Invoke(); break;
            default: break;
        }
    }

    public LevelPhase GetLevelPhase()
    {
        return levelPhase;
    }

    public void StartWave()
    {
        if(levelPhase == LevelPhase.build)     ChangeLevelPhase(LevelPhase.play);
    }

    public void EndWave()
    {
        ChangeLevelPhase(LevelPhase.postplay);
        ResetWaveForNextBuildPhase();
    }

    private void ResetWaveForNextBuildPhase()
    {
        StartCoroutine(DelayedBackToBuildingPhase());
    }

    private IEnumerator DelayedBackToBuildingPhase()
    {
        yield return new WaitForSeconds(1f);
        ChangeLevelPhase(LevelPhase.build);
    }
}
