using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IngameMenu : MonoBehaviour
{
    
    [SerializeField] GameObject menuParent;
    [SerializeField] Button continueButton;
    [SerializeField] Button menuButton;
    [SerializeField] Button optionButton;
    [SerializeField] Button optionBackButton;
    [SerializeField] Options options;
    [SerializeField] GameObject ingameMenu;
    private SceneFader sceneFader;

    void Start()
    {
        sceneFader = FindObjectOfType<SceneFader>();
        menuParent.SetActive(false);
        continueButton.onClick.AddListener(ToggleMenu);
        menuButton.onClick.AddListener(GoBackToMenu);
        optionButton.onClick.AddListener(ToggleOptions);
        optionBackButton.onClick.AddListener(ShowIngameMenuButtons);
    }

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name == GameConstants.SCENE_GAME)
        {
            ToggleMenu();
        }    
    }

    private void ToggleMenu()
    {
        menuParent.SetActive(!menuParent.activeSelf);

        if(menuParent.activeSelf)
        {
            Time.timeScale = 0;
        }else{
            Time.timeScale = 1;
        }
    }

    private void GoBackToMenu()
    {
        Time.timeScale = 1;
        sceneFader.FadeToScene(GameConstants.SCENE_MAIN_MENU);
    }

    private void ToggleOptions()
    {
        HideIngameMenuButtons();
        options.ShowOptions();
    }

    private void HideIngameMenuButtons()
    {
        ingameMenu.SetActive(false);
    }

        private void ShowIngameMenuButtons()
    {
        ingameMenu.SetActive(true);
    }
}
