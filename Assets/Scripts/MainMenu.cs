using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    
    [SerializeField] private Button startButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button endButton;
    [SerializeField] private SceneFader sceneFader;

    void Start()
    {
        startButton.onClick.AddListener(() => {sceneFader.FadeToScene(GameConstants.SCENE_GAME);});
        creditsButton.onClick.AddListener(() => {Debug.Log("todo");});
        endButton.onClick.AddListener(() => { Application.Quit();});
    }
}
