using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointAlgorithm
{
    private Grid<TileGrid.GridTileObject> tileGrid;

    public WaypointAlgorithm(Grid<TileGrid.GridTileObject> tileGrid)
    {
        this.tileGrid = tileGrid;
    }

    public Queue<Vector3> GetBestWaypointPath(Vector3 start, Vector3 destination)
    {
        Debug.Log("starting to find waypoints");
        Transform startNode = tileGrid.GetGridObject(start).GetTransform();
        Transform finalNode = tileGrid.GetGridObject(destination).GetTransform();

        return AStar(startNode.GetComponent<Tile>(),finalNode.GetComponent<Tile>());
    }

    private Queue<Vector3> ReconstructPath(Dictionary<Tile, Tile> cameFrom, Tile current)
    {
        Queue<Vector3> path = new Queue<Vector3>();
        tileGrid.GetXY(current.transform.position, out int x, out int y);
        path.Enqueue(tileGrid.GetCellCenter(x, y));

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            tileGrid.GetXY(current.transform.position, out x, out y);
            path.Enqueue(tileGrid.GetCellCenter(x, y));
        }

        Debug.Log(path);
        Vector3[] array = path.ToArray();
        foreach (var item in array)
        {
            Debug.Log(item);
        }

        return path;
    }

    private Queue<Vector3> AStar(Tile start, Tile end)
    {
        // Create Queue and add first element
        Queue<Tile> openSet = new Queue<Tile>();
        Queue<Tile> closedSet = new Queue<Tile>();
        openSet.Enqueue(start);
        closedSet.Enqueue(start);

        Dictionary<Tile, Tile> cameFrom = new Dictionary<Tile, Tile>();

        // 
        while (openSet.Count != 0)
        {
            Tile current = openSet.Dequeue();

            // End condition
            if (current == end)
            {
                Debug.Log("Found path");
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
                        openSet.Enqueue(newTile);
                        closedSet.Enqueue(newTile);
                        cameFrom[newTile] = current;
                    }
                }
            }
        }

        Debug.LogError("no path found");
        return new Queue<Vector3>();
    }
}
