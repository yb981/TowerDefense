using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    [SerializeField] private float fadeSpeed = 1f;
    private Image image;
    private String nextScene;

    void Start()
    {
        image = GetComponentInChildren<Image>();
        StartCoroutine(FadeIn());
    }

    public void FadeToScene(string scene)
    {
        nextScene = scene;
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float fadePercent = 10;

        while(fadePercent <= 1f)
        {
            fadePercent += Time.deltaTime * fadeSpeed;
            image.color = new Color(image.color.r,image.color.g,image.color.b,fadePercent);
            yield return null;
        }

        LoadNextScene();
    }

    private IEnumerator FadeIn()
    {
        float fadePercent = 1f;

        while(fadePercent >= 0)
        {
            fadePercent -= Time.deltaTime * fadeSpeed;
            image.color = new Color(image.color.r,image.color.g,image.color.b,fadePercent);
            yield return null;
        }
    }

    private void LoadNextScene()
    {
        PersistenceManager.Instance.LastScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(nextScene);
    }
}
