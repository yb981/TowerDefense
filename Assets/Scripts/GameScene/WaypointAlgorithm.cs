using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointAlgorithm
{
    private Grid<TileGrid.GridTileObject> tileGrid;
    private TileGrid tileGridComp;

    public WaypointAlgorithm(TileGrid tileGridComp, Grid<TileGrid.GridTileObject> tileGrid)
    {
        this.tileGrid = tileGrid;
        this.tileGridComp = tileGridComp;
    }

    public Stack<Vector3> GetBestWaypointPath(Vector3 start, Vector3 destination)
    {
        Transform startNode = tileGrid.GetGridObject(start).GetTransform();
        Transform finalNode = tileGrid.GetGridObject(destination).GetTransform();

        return AStar(startNode.GetComponent<Tile>(), finalNode.GetComponent<Tile>());
    }

    private Stack<Vector3> ReconstructPath(Dictionary<Tile, Tile> cameFrom, Tile current)
    {
        Stack<Vector3> path = new Stack<Vector3>();
        tileGrid.GetXY(current.transform.position, out int x, out int y);
        path.Push(tileGrid.GetCellCenter(x, y));

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            tileGrid.GetXY(current.transform.position, out x, out y);
            path.Push(tileGrid.GetCellCenter(x, y));
        }

        return path;
    }

    private Stack<Vector3> AStar(Tile start, Tile end)
    {
        // Create Stack and add first element
        Stack<Tile> openSet = new Stack<Tile>();
        Stack<Tile> closedSet = new Stack<Tile>();
        openSet.Push(start);
        closedSet.Push(start);

        Dictionary<Tile, Tile> cameFrom = new Dictionary<Tile, Tile>();

        // 
        while (openSet.Count != 0)
        {
            Tile current = openSet.Pop();

            // End condition
            if (current == end)
            {
                return ReconstructPath(cameFrom, current);
            }

            Tile.TileNode[] tmpNeighbors = current.GetOpeningNodes();
            for (int i = 0; i < tmpNeighbors.Length; i++)
            {
                if (tmpNeighbors[i].connectionTile != null)
                {
                    Tile newTile = tmpNeighbors[i].connectionTile.GetComponent<Tile>();

                    if (!closedSet.Contains(newTile))
                    {
                        openSet.Push(newTile);
                        closedSet.Push(newTile);
                        cameFrom[newTile] = current;
                    }
                }
            }
        }

        Debug.LogError("no path found");
        return new Stack<Vector3>();
    }

/*     public List<TileGrid.GridTileObject> GetAllTiles()
    {
        List<TileGrid.GridTileObject> allTiles = new List<TileGrid.GridTileObject>();

        TileGrid.GridTileObject startNode = tileGrid.GetGridObject(tileGridComp.GetStartTileCenterPosition());


        allTiles.Add(startNode);

        // Add all new nodes if any
        Tile.TileNode[] openings = startNode.GetTransform().GetComponent<Tile>().GetOpeningNodes();
        if (openings[0].entry)
        {
            Transform newTile = openings[0].connectionTile;
            if (newTile != null)
            {

                if (!allTiles.Contains(newTile))
            }
        }

        return allTiles; 
    }*/
}
