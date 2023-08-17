using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tile : MonoBehaviour
{
    public enum direction
    {
        right,
        top,
        left,
        down
    }

    [SerializeField] private BuildTileData buildTileData;
    /*     [SerializeField] private GridBuildingSystem.FieldType[,] buildArea  = 
                                                            { {GridBuildingSystem.FieldType.unit, GridBuildingSystem.FieldType.unit}, 
                                                            {GridBuildingSystem.FieldType.building, GridBuildingSystem.FieldType.building}} ; 
         */
    [SerializeField] private bool[] openingsForInit = new bool[4];
    private TileNode[] openingNodes = new TileNode[4];


    private int rotation;
    private List<Transform> rotationTiles = new List<Transform>();
    private int currentRotationIndex = 0;
    private TileGrid tileGrid;
    [Header("Coding references")]
    [SerializeField] private Tilemap tileMap;
    [SerializeField] private GridLayout tileVisualGrid;

    private void Start()
    {
        //AddRotationTilesToList();
        InitNodes();
        tileGrid = FindObjectOfType<TileGrid>();
    }

    private void Update()
    {
        /*         // TEST
                if(Input.GetKey(KeyCode.E))
                {
                    Vector3Int currentCell = tileVisualGrid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    if(tileMap.GetTile(currentCell) != null)
                        tileMap.SetTransformMatrix(currentCell,Matrix4x4.Rotate(Quaternion.Euler(0,0,-90f)));
                } */
    }

    private void InitNodes()
    {
        for (int i = 0; i < openingNodes.Length; i++)
        {
            openingNodes[i] = new TileNode();
            if (openingsForInit[i])
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
            if (i != currentRotationIndex)
            {
                rotationTiles[i].gameObject.SetActive(false);
            }
        }
    }

    public void RotateTile()
    {

        if (currentRotationIndex == rotationTiles.Count - 1)
        {
            currentRotationIndex = 0;
        }
        else
        {
            currentRotationIndex++;
        }

        RotateNodesCounterClockwise();
        RotateBuildAreaCounterClockwise();
        RotateVisual();
    }

    private void RotateVisual()
    {

        //visual rotation

        int adjustment = tileGrid.GetCellSize();

        Transform child = transform.GetChild(0);
        child.Rotate(new Vector3(0, 0, 90f), Space.Self);

        Vector3 adjustmentVector = new Vector3(0, 0, 0);

        if (child.eulerAngles.z == 90f)
        {
            adjustmentVector += new Vector3(adjustment, 0, 0);
        }
        else if (child.eulerAngles.z == 180f)
        {
            adjustmentVector += new Vector3(0, adjustment, 0);
        }
        else if (child.eulerAngles.z == 270f)
        {
            adjustmentVector += new Vector3(-adjustment, 0, 0);
        }
        else
        {
            adjustmentVector += new Vector3(0, -adjustment, 0);
        }
        child.position += adjustmentVector;

        RotateTilesInTilemap();
        /* 
                // tiles rotation
                for (int i = 0; i < rotationTiles.Count; i++)
                        {
                            if(i != currentRotationIndex)
                            {
                                rotationTiles[i].gameObject.SetActive(false);
                            }else{
                                rotationTiles[i].gameObject.SetActive(true);
                            }
                }  */
    }

    private void RotateTilesInTilemap()
    {
        BoundsInt bounds = tileMap.cellBounds; 

        //float rotation = transform.eulerAngles.z;
        //Debug.Log("bounds position: "+bounds.position);

        foreach (Vector3Int position in bounds.allPositionsWithin)
        {
            TileBase tile = tileMap.GetTile(position);

            if (tile != null)
            {
                Matrix4x4 rotationMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 0, -90f));

                tileMap.SetTransformMatrix(position, rotationMatrix);
            }
        }
    }

    private void RotateNodesCounterClockwise()
    {
        TileNode temporaryNode = openingNodes[openingNodes.Length - 1];
        for (int i = openingNodes.Length - 1; i > 0; i--)
        {
            openingNodes[i] = openingNodes[i - 1];
        }
        openingNodes[0] = temporaryNode;

        string rotation = "[";
        foreach (var item in openingNodes)
        {
            rotation += item.entry + ",";
        }
        rotation += "]";
        Debug.Log(rotation);
    }

    private void RotateBuildAreaClockwise()
    {
        // Create Copy
        BuildTileData copyData = CopyBuildTileData();

        // Rotate
        int numCols = buildTileData.rows.Length;
        for (int col = 0; col < numCols; col++)
        {
            for (int j = 0; j < buildTileData.rows[col].row.Length; j++)
            {
                copyData.rows[numCols - j - 1].row[col] = buildTileData.rows[col].row[j];
            }
        }

        buildTileData = copyData;
    }

    private void RotateBuildAreaCounterClockwise()
    {
        // Create Copy
        BuildTileData copyData = CopyBuildTileData();

        // Rotate
        int numCols = buildTileData.rows.Length;
        int numRows = buildTileData.rows[0].row.Length;
        for (int col = 0; col < numCols; col++)
        {
            for (int j = 0; j < buildTileData.rows[col].row.Length; j++)
            {
                copyData.rows[j].row[numRows - col - 1] = buildTileData.rows[col].row[j];
            }
        }

        buildTileData = copyData;
    }

    private BuildTileData CopyBuildTileData()
    {
        BuildTileData copyData = new BuildTileData();
        for (int i = 0; i < buildTileData.rows.Length; i++)
        {
            copyData.rows[i].row = new GridBuildingSystem.FieldType[buildTileData.rows[i].row.Length];
        }
        return copyData;
    }

    public void SetPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    public BuildTileData GetBuildArea()
    {
        return buildTileData;
    }

    public TileNode GetNorthNode()
    {
        return openingNodes[1];
    }

    public TileNode GetSouthNode()
    {
        return openingNodes[3];
    }

    public TileNode GetWestNode()
    {
        return openingNodes[2];
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
        public Transform connectionTile;
    }


}
