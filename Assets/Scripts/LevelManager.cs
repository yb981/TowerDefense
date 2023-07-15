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
    public event Action OnGameOver;

    public enum LevelPhase 
    {
        build,
        play,
        postplay,
    }

    [SerializeField] private SceneFader sceneFader;

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

    public void GameOver()
    {
        ChangeLevelPhase(LevelPhase.postplay);
        OnGameOver?.Invoke();
        PersistData();
        sceneFader.FadeToScene(GameConstants.SCENE_HIGHSCORE);
    }

    private void PersistData()
    {
        PersistenceManager.Instance.SaveScore(PlayerStats.Instance.GetScore());
        PersistenceManager.Instance.SetWaveCount(GetComponent<WaveManager>().GetCurrentWave());
        PersistenceManager.Instance.SetPlayTime(GetComponent<PlayTimer>().GetPlayTime());
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
