using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreBoardFieldName : MonoBehaviour
{
    private Scoreboard scoreboard;
    private TMP_InputField inputField;
    [SerializeField] private GameObject inputGameObject;
    [SerializeField] private int maxCharacter = 5;


    private void Start()   
    {
        inputField = GetComponentInChildren<TMP_InputField>();
        inputField.onSubmit.AddListener(PassInputName);
        inputField.characterLimit = maxCharacter;
        scoreboard = FindObjectOfType<Scoreboard>();
        Hide();
    }

    private void PassInputName(string name)
    {
        scoreboard.SetPlayerName(name);
        Hide();
    }

    public void Show()
    {
        inputGameObject.SetActive(true);
    }

    public void Hide()
    {
        inputGameObject.SetActive(false);
    }
}
