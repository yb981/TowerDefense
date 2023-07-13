using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private SceneFader sceneFader;

    void Start()
    {
        button.onClick.AddListener(() => {sceneFader.FadeToScene(GameConstants.SCENE_MAIN_MENU);});
    }

}
