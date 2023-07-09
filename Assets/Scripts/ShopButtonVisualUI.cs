using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButtonVisualUI : MonoBehaviour
{
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
        // Until real sprites
        image.color = monsterTransform.GetComponentInChildren<SpriteRenderer>().color;
    }
}
