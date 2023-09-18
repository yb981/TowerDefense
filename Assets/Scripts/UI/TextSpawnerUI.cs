using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSpawnerUI : MonoBehaviour
{
    public static TextSpawnerUI Instance {get ; private set;}

    [SerializeField] private GameObject textPopUpPrefab;

    private void Awake() 
    {
        Instance = this;   
    }

    public GameObject CreateWorldPopUpText(string displayText, Vector3 spawnPosition)
    {
        GameObject popUpGameObject = Instantiate(textPopUpPrefab, spawnPosition,Quaternion.identity, transform);
        popUpGameObject.GetComponent<PopUpTextUI>().SetText(displayText);
        return popUpGameObject;
    }
}
