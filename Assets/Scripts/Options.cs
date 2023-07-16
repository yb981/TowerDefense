using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private Slider volumeMusicSlider;
    [SerializeField] private Slider volumeEffectsSlider;


    private void Start()
    {
        backButton.onClick.AddListener(SaveAndHide);
        volumeMusicSlider.onValueChanged.AddListener((float value) => ChangeMusicVolume(value));
        volumeEffectsSlider.onValueChanged.AddListener((float value) => ChangeEffectsVolume(value));
        gameObject.SetActive(false);
    }

    public void ShowOptions()
    {
        gameObject.SetActive(true);
        LoadPrefs();
    }

    private void LoadPrefs()
    {
        volumeMusicSlider.value = SoundManager.Instance.GetMusicVolume();
        volumeEffectsSlider.value = SoundManager.Instance.GetEffectsVolume();
    }

    private void ChangeMusicVolume(float value)
    {
        SoundManager.Instance.ChangeMusicVolume(value);
    }

    private void ChangeEffectsVolume(float value)
    {
        SoundManager.Instance.ChangeEffectsVolume(value);
    }

    private void SaveAndHide()
    {
        SavePrefs();
        gameObject.SetActive(false);
    }

    private void SavePrefs()
    {
        PlayerPrefs.SetFloat(GameConstants.MUSIC_VOLUME, volumeMusicSlider.value);
        PlayerPrefs.SetFloat(GameConstants.EFFECTS_VOLUME, volumeEffectsSlider.value);
    }
}
