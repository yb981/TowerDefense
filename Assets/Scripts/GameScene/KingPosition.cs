using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingPosition : MonoBehaviour
{

    private void Start() 
    {
        PlaceKing();
    }

    private void PlaceKing()
    {
        King king = FindObjectOfType<King>();
        king.SetSpawnPoint(transform.position);
        king.transform.position = transform.position;
    }   
}
