using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextWaveButton : MonoBehaviour
{
    private Button button;

    void Start()
    {
        LevelManager.instance.OnLevelPhaseBuild += LevelManager_OnLevelPhaseBuild;
        LevelManager.instance.OnLevelPhasePlay += LevelManager_OnLevelPhasePlay;

        button = GetComponent<Button>();
        button.onClick.AddListener(StartNextWave);
    }

    private void LevelManager_OnLevelPhaseBuild()
    {
        button.interactable = true;
    }

    private void LevelManager_OnLevelPhasePlay()
    {
        button.interactable = false;
    }

    private void StartNextWave()
    {
        LevelManager.instance.StartWave();
    }
}
