using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    
    [SerializeField] private Button startButton;
    [SerializeField] private Button highscoresButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button endButton;
    [SerializeField] private SceneFader sceneFader;

    [SerializeField] private Options options;

    void Start()
    {
        startButton.onClick.AddListener(() => {sceneFader.FadeToScene(GameConstants.SCENE_GAME);});
        highscoresButton.onClick.AddListener(() => {sceneFader.FadeToScene(GameConstants.SCENE_HIGHSCORE);});
        optionsButton.onClick.AddListener(() => {options.ShowOptions();});
        creditsButton.onClick.AddListener(() => {sceneFader.FadeToScene(GameConstants.SCENE_CREDITS);});
        endButton.onClick.AddListener(() => { Application.Quit();});
    }
}
