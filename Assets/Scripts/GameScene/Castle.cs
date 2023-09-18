using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    [SerializeField] private Transform castlePrefab;
    private TileGrid tileGrid;
    
    private void Awake() 
    {
        tileGrid = FindObjectOfType<TileGrid>();
    }

    public void BuildCastle()
    {
        int width = tileGrid.GetWidth();
        int height = tileGrid.GetHeight();

        for(int i = 0 ; i < width ; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if(i == 0 || j == 0 || i == width-1 || j == width - 1)
                {
                    BuildCastleTile(i,j);
                }
            }
        }
    }

    private void BuildCastleTile(int x, int y)
    {
        tileGrid.PlaceNewTile(Instantiate(castlePrefab,tileGrid.GetWorldPositionOfCell(x,y),Quaternion.identity,transform),x,y);
    }
}
