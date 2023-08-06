using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{    
    public enum direction
    {
        right,
        top,
        left,
        down
    }

    [SerializeField] private bool[] openings = new bool[4];

    private int rotation;
    private List<Transform> rotationTiles = new List<Transform>();
    private int currentRotationIndex = 0;
    
    private void Start() 
    {
        AddRotationTilesToList();
    }

    private void AddRotationTilesToList()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            rotationTiles.Add(transform.GetChild(i));

            // Disable child/rotation if not the first
            if(i != currentRotationIndex)
            {
                rotationTiles[i].gameObject.SetActive(false);
            }
        }
    }

    public void TurnTile()
    {

        if(currentRotationIndex == rotationTiles.Count -1)
        {
            currentRotationIndex = 0;
        }else{
            currentRotationIndex++;
        }
        
        for (int i = 0; i < rotationTiles.Count; i++)
        {
            if(i != currentRotationIndex)
            {
                rotationTiles[i].gameObject.SetActive(false);
            }else{
                rotationTiles[i].gameObject.SetActive(true);
            }
        }
    }

    public void SetPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

}
