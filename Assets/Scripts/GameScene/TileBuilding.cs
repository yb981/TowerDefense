using System;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;

public class TileBuilding : MonoBehaviour
{
    [Header("test")]
    [SerializeField] private Transform testTile;
    [Header("StartTile")]
    [SerializeField] private Transform startTilePrefab;
    [SerializeField] private int StartX;
    [SerializeField] private int StartY;
    [Header("BossTile")]
    [SerializeField] private Transform bossTilePrefab;
    [SerializeField] private int maxRangeFromStart;
    [SerializeField] private int minRangeFromStart;

    private Castle castle;
    private TileGrid tileGridComponent;
    private Transform ghostObject;
    private bool building;
    private Grid<TileGrid.GridTileObject> tileGrid;

    private void Start()
    {
        tileGridComponent = GetComponent<TileGrid>();
        tileGrid = tileGridComponent.GetTileGrid();

        TileSelection tileSelection = FindObjectOfType<TileSelection>();
        tileSelection.OnTileSelected += TileSelection_OnTileSelected;

        castle = FindObjectOfType<Castle>();

        MonsterBoss.OnBossDied += MonsterBoss_OnBossDied;

        CreateInitialTilesInGrid();
    }

    private void MonsterBoss_OnBossDied()
    {
        Debug.Log("tileBuilding recieved event, boss died");
        PlaceNewBossTile();
    }

    private void TileSelection_OnTileSelected(object sender, TileSelection.OnTileSelectedEventArgs e)
    {
        StartBuilding(e.tileBlueprint);
    }

    private void StartBuilding(TileBlueprint tileBlueprint)
    {
        building = true;
        ghostObject = Instantiate(tileBlueprint.tilePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        ghostObject.GetComponent<Tile>().SetTileEffect(tileBlueprint.mainTileEffect);
        StartCoroutine("BuildingTiles");
    }

    private IEnumerator BuildingTiles()
    {
        while (building)
        {
            // Update Position
            // TODO only update position if can build in that area
            // there is an entry point from existing tile
            tileGrid.GetXY(UtilsClass.GetMouseWorldPosition(), out int x, out int y);
            if (ghostObject.transform.position != tileGrid.GetWorldPosition(x, y))
            {
                ghostObject.transform.position = tileGrid.GetWorldPosition(x, y);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ghostObject.GetComponent<Tile>().RotateTile();
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (tileGridComponent.TryPlacingConnectionTile(ghostObject, x, y))
                {
                    building = false;
                    LevelManager.instance.EndTileBuild();
                }
            }
            yield return null;
        }
    }


    private void CreateInitialTilesInGrid()
    {
        StartCoroutine(CreateInitialTilesEndOfFrame());
    }

    private IEnumerator CreateInitialTilesEndOfFrame()
    {
        yield return null;

        //Transform instantiatedStart = Instantiate(startTilePrefab, tileGrid.GetWorldPosition(StartX, StartY), Quaternion.identity);
        tileGridComponent.SetStartTile(StartX, StartY);
        tileGridComponent.TryPlacingSoloTile(startTilePrefab, StartX, StartY);

        //tileGridComponent.PlaceNewTile(instantiatedStart, StartX, StartY);
        castle.BuildCastle();

        // Random Boss Coords
        PlaceNewBossTile();
    }


    public void PlaceNewBossTile()
    {
        Vector2Int newPos = new Vector2Int();

        // loop until free tile
        int safetyExit = 20;
        do
        {
            List<TileGrid.GridTileObject> allOpenEndTiles = tileGridComponent.GetAllOpenEnds();
            Vector2Int randomTilePosition = allOpenEndTiles[UnityEngine.Random.Range(0, allOpenEndTiles.Count)].GetPosition();

            newPos = FindRandomPosition(randomTilePosition, minRangeFromStart, maxRangeFromStart);
            safetyExit--;
        } while (!tileGridComponent.TryPlacingSoloTile(bossTilePrefab, newPos.x, newPos.y) && safetyExit >= 0);
    }

    private Vector2Int FindRandomPosition(Vector2Int StartPosition, int minRange, int maxRange)
    {
        int randomX = UnityEngine.Random.Range(0, maxRange);
        int randomY = UnityEngine.Random.Range(minRange - randomX, maxRange - randomX);

        // Random Sign
        randomX *= (int)((UnityEngine.Random.Range(0, 2) - 0.5) * 2);
        randomY *= (int)((UnityEngine.Random.Range(0, 2) - 0.5) * 2);

        return new Vector2Int(StartPosition.x + randomX, StartPosition.y + randomY);
    }
}
