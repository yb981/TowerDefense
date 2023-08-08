using System;
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

    [SerializeField] private bool[] openingsForInit = new bool[4];
    private TileNode[] openingNodes = new TileNode[4];

    private int rotation;
    private List<Transform> rotationTiles = new List<Transform>();
    private int currentRotationIndex = 0;
    
    private void Start() 
    {
        AddRotationTilesToList();

        InitNodes();
    }

    private void InitNodes()
    {
        for (int i = 0; i < openingNodes.Length; i++)
        {
            openingNodes[i] = new TileNode();
            if(openingsForInit[i])
            {
                openingNodes[i].entry = true;
            }
        }
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

        RotateNodesCounterClockwise();
        
        for (int i = 0; i < rotationTiles.Count; i++)
        {
            if(i != currentRotationIndex)
            {
                rotationTiles[i].gameObject.SetActive(false);
            }else{
                rotationTiles[i].gameObject.SetActive(true);
            }
        }

        Debug.Log("turned, openings: " );
        for (int i = 0; i < openingNodes.Length; i++)
        {
            Debug.Log(openingNodes[i].entry+"," );
        }
    } 

    private void RotateNodesCounterClockwise()
    {
        TileNode temporaryNode = openingNodes[0];
        for (int i = 0; i < openingNodes.Length; i++)
        {
            int next = i+1;
            if(next == openingNodes.Length)
            {
                // if last index, get saved first
                openingNodes[i] = temporaryNode;
            }else{
                openingNodes[i] = openingNodes[next];
            }
        }
    }

    public void SetPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    public TileNode GetNorthNode()
    {
        return openingNodes[1%rotationTiles.Count];
    }

    public TileNode GetSouthNode()
    {
        return openingNodes[3%rotationTiles.Count];
    }

    public TileNode GetWestNode()
    {
        return openingNodes[2%rotationTiles.Count];
    }

    public TileNode GetEastNode()
    {
        return openingNodes[0];
    }

    public TileNode[] GetOpeningNodes()
    {
        return openingNodes;
    }

    public class TileNode
    {
        public bool entry = false;
        public GameObject connectionTile;
    }

}
