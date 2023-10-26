using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRangeIndicator : MonoBehaviour
{
    
    private SpriteRenderer rangeSprite;
    private Tower tower;

    void Start()
    {
        rangeSprite = GetComponent<SpriteRenderer>();
        tower = GetComponentInParent<Tower>();
        DisplayCorrectRange();
    }

    private void DisplayCorrectRange()
    {
        transform.localScale = Vector3.one * tower.GetRange()*2;
    }

}
