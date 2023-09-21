using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    [SerializeField] private Transform castleFrontPrefab;
    [SerializeField] private Transform castleVerticalLeftPrefab;
    [SerializeField] private Transform castleVerticalRightPrefab;
    [SerializeField] private Transform castleTopRightPrefab;
    [SerializeField] private Transform castleBottomRightPrefab;
    [SerializeField] private Transform castleTopLeftPrefab;
    [SerializeField] private Transform castleBottomLeftPrefab;
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
                if(i == width - 1 && j == width-1) 
                {
                    BuildCastleTile(castleTopRightPrefab,i,j);
                }else if(i == width -1 && j == 0)
                {
                    BuildCastleTile(castleBottomRightPrefab,i,j);
                } else if(i == 0 && j == 0)
                {
                    BuildCastleTile(castleBottomLeftPrefab,i,j);
                }else if(i == 0 && j == width-1)
                {
                    BuildCastleTile(castleTopLeftPrefab,i,j);
                }
                else if(j == 0 || j == width-1)
                {
                    BuildCastleTile(castleFrontPrefab,i,j);
                }else if(i == 0)
                {   
                    BuildCastleTile(castleVerticalLeftPrefab,i,j);
                }else if(i == width - 1)
                {   
                    BuildCastleTile(castleVerticalRightPrefab,i,j);
                }
            }
        }
    }

    private void BuildCastleTile(Transform prefab,int x, int y)
    {
        tileGrid.PlaceNewTile(Instantiate(prefab,tileGrid.GetWorldPositionOfCell(x,y),Quaternion.identity,transform),x,y);
    }
}
