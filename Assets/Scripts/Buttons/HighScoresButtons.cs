using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoresButtons : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneFader sceneFader = FindObjectOfType<SceneFader>();
        GetComponent<Button>().onClick.AddListener(() => { sceneFader.FadeToScene(GameConstants.SCENE_MAIN_MENU);});
    }
}
