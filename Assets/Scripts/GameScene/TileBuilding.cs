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
    [SerializeField] private Transform startTile;
    [SerializeField] private int StartX;
    [SerializeField] private int StartY;
    [Header("BossTile")]
    [SerializeField] private Transform bossTile;
    [SerializeField] private int maxRangeFromStart;
    [SerializeField] private int minRangeFromStart;
    private int BossX;
    private int BossY;
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

        MonsterBoss.OnBossDied += MonsterBoss_OnBossDied;

        CreateTilesInGrid();
    }

    private void MonsterBoss_OnBossDied()
    {
        Debug.Log("tileBuilding recieved event, boss died");
        PlaceNewBossTile();
    }

    private void TileSelection_OnTileSelected(object sender, TileSelection.OnTileSelectedEventArgs e)
    {
        StartBuilding(e.tilePrefab);
    }

    private void StartBuilding(Transform newTile)
    {
        building = true;
        ghostObject = Instantiate(newTile, new Vector3(0, 0, 0), Quaternion.identity);
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


    private void CreateTilesInGrid()
    {
        Transform startT = Instantiate(startTile, tileGrid.GetWorldPosition(StartX, StartY), Quaternion.identity);
        StartCoroutine(CreateStartTileEndOfFrameAndBossTile(startT, StartX, StartY));
    }

    private IEnumerator CreateStartTileEndOfFrameAndBossTile(Transform tile, int x, int y)
    {
        yield return null;
        tileGridComponent.SetStartTile(StartX,StartY);
        tileGridComponent.PlaceNewTile(tile, StartX, StartY);

        // Random Boss Coords
        BossX = (int)(StartX + ((UnityEngine.Random.Range(0, 2) - 0.5) * 2) * UnityEngine.Random.Range(minRangeFromStart, maxRangeFromStart));
        BossY = (int)(StartY + ((UnityEngine.Random.Range(0, 2) - 0.5) * 2) * UnityEngine.Random.Range(minRangeFromStart, maxRangeFromStart));

        Transform bossT = Instantiate(bossTile, tileGrid.GetWorldPosition(BossX, BossY), Quaternion.identity);
        tileGridComponent.PlaceNewTile(bossT, BossX, BossY);
    }


    public void PlaceNewBossTile()
    {
        int newBossX;
        int newBossY;


        // loop until free tile
        int safetyExit = 4;
        do
        {
            newBossX = (int)(BossX + ((UnityEngine.Random.Range(0, 2) - 0.5) * 2) * UnityEngine.Random.Range(minRangeFromStart, maxRangeFromStart));
            newBossY = (int)(BossY + ((UnityEngine.Random.Range(0, 2) - 0.5) * 2) * UnityEngine.Random.Range(minRangeFromStart, maxRangeFromStart));
            safetyExit--;
        } while (!tileGridComponent.TryPlacingSoloTile(bossTile ,newBossX, newBossY) && safetyExit >= 0);
    }
}
