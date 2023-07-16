using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [SerializeField] private Button backButton;

    private void Start() 
    {
        backButton.onClick.AddListener(Hide);
        gameObject.SetActive(false);
    }

    public void ShowOptions()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
