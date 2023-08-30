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

    public Stack<Vector3> GetBestWaypointPath(Vector3 start, Vector3 destination)
    {
        Transform startNode = tileGrid.GetGridObject(start).GetTransform();
        Transform finalNode = tileGrid.GetGridObject(destination).GetTransform();
        Debug.Log("start node: "+startNode);
        Debug.Log("final node: "+finalNode);
        
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
}
