using AI;
using System.Collections.Generic;
using UnityEngine;
using static MapReader;

public class MapData : MonoBehaviour
{
    [SerializeField] private GameObject[] tilePrefabs;
    private List<Vector2Int> path = new List<Vector2Int>();

    public void DrawMap(Map map)
    {
        foreach (Tile tile in map.tiles)
        {
            GameObject obj = Instantiate(tilePrefabs[(int)TileMethods.TypeByChar[tile.id]]);
            obj.transform.position = Vector3.forward * tile.y * 2 + Vector3.right * tile.x * 2;
            obj.name = $"({tile.x},{tile.y})";
            obj.transform.SetParent(transform);
        }
        // Set camera position in center of level
        Camera.main.transform.position = new Vector3(((map.maxWidth - 1) * 2) * 0.5f, 25, ((map.maxHeight - 1) * 2) * 0.5f);

        FindAccessibleTiles(map);
    }

    private void FindAccessibleTiles(Map map)
    {
        List<Vector2Int> accessibles = new List<Vector2Int>();
        Vector2Int startPos = Vector2Int.zero;
        Vector2Int endPos = Vector2Int.zero;

        foreach (Tile tile in map.tiles)
        {
            if (TileMethods.IsWalkable(TileMethods.TypeByChar[tile.id]))
            {
                accessibles.Add(new Vector2Int(tile.x, tile.y));

                if (TileMethods.TypeByChar[tile.id] == TileType.Start)
                {
                    startPos = new Vector2Int(tile.x, tile.y);
                }
                else if (TileMethods.TypeByChar[tile.id] == TileType.End)
                {
                    endPos = new Vector2Int(tile.x, tile.y);
                }
            }

        }
        if (startPos == endPos) { return; }

        IPathFinder pathFinder = new Dijkstra(accessibles);
        path = new List<Vector2Int>(pathFinder.FindPath(startPos, endPos));
    }

    private void OnDrawGizmos()
    {
        if (path != null && path.Count > 0)
        {
            Gizmos.color = Color.magenta;
            for (int i = 0; i < path.Count; i++)
            {
                Vector3 myPos = new Vector3(path[i].x, 0, path[i].y) * 2.0f;
                Vector3 neighbourPos = new Vector3(path[i < path.Count - 1 ? i + 1 : i].x, 0, path[i < path.Count - 1 ? i + 1 : i].y) * 2.0f;
                Gizmos.DrawLine(myPos, neighbourPos);
            }
        }
    }
}
