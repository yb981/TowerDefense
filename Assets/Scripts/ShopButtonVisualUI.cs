using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopButtonVisualUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmpCost;
    private ShopButtonUI shopButtonUI;
    private Image image;

    private void Awake() 
    {
        image = GetComponent<Image>();
        shopButtonUI = GetComponentInParent<ShopButtonUI>();
    }

    private void Start() 
    {
        KnightBluePrintSO monster = shopButtonUI.GetMonsterSO();
        Transform monsterTransform = monster.GetTransform();
        image.sprite = monsterTransform.GetComponentInChildren<SpriteRenderer>().sprite;
        image.color = monsterTransform.GetComponentInChildren<SpriteRenderer>().color;

        tmpCost.text = "$"+monster.GetCost().ToString();
    }
}
