using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance {get; private set;}

    public event Action OnLevelPhaseTileBuild;
    public event Action OnLevelPhasePlay;
    public event Action OnLevelPhaseBuild;
    public event Action OnLevelPhasePostPlay;
    public event Action OnGameOver;

    public enum LevelPhase 
    {
        tilebuild,
        build,
        play,
        postplay,
    }

    [SerializeField] private SceneFader sceneFader;
    [SerializeField] private float changePhaseDelay = 1f; 

    private LevelPhase levelPhase;
    
    private void Awake() 
    {
        instance = this;    
    }

    private void Start() 
    {
        ChangeLevelPhase(LevelPhase.build);

        Invoke("SetStartingPositionOfCamera",0.2f);
    }

    private void ChangeLevelPhase(LevelPhase newPhase)
    {
        levelPhase = newPhase;
        switch(newPhase)
        {
            case LevelPhase.tilebuild: OnLevelPhaseTileBuild?.Invoke(); break;
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

    public void EndTileBuild()
    {
        if(levelPhase == LevelPhase.tilebuild)     ChangeLevelPhase(LevelPhase.build);
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
        StartCoroutine(DelayedPhaseTransition(LevelPhase.tilebuild, 0.5f));
    }

    private IEnumerator DelayedPhaseTransition(LevelPhase phase, float time)
    {
        yield return new WaitForSeconds(time);
        ChangeLevelPhase(phase);
    }
}
