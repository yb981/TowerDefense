using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpTextUI : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private TextMeshProUGUI popUpText;
    [Header("Tuning")]
    [SerializeField] private float floatSpeed = 0.05f;
    private RectTransform rectTransform;
    private Canvas canvas;
    private void Start() 
    {
        canvas = GetComponentInParent<Canvas>();
        canvas.sortingLayerName = "Units";
        canvas.sortingOrder = 100;

        rectTransform = GetComponent<RectTransform>();
        Destroy(gameObject,1f);
    }

    private void Update() 
    {
        rectTransform.position += Vector3.up * floatSpeed;
    }

    public void SetText(string displayText)
    {
        popUpText.text = displayText;
    }
}
