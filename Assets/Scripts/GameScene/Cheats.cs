using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour
{
    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            Monster[] monsters = FindObjectsOfType<Monster>();
            foreach (var monster in monsters)
            {
                monster.GetComponent<Health>().DoDamage(1000);
            }
        }    
    }
}
