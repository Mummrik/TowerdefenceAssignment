using AI;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour
{
    [SerializeField] private GameObject[] m_TilePrefabs = new GameObject[0];
    private static List<Vector2Int> s_Path = new List<Vector2Int>();

    private List<GameObject> m_TilePool = new List<GameObject>();

    public void DrawMap(Map map)
    {
        foreach (Tile tile in map.tiles)
        {
            GameObject obj = Instantiate(m_TilePrefabs[(int)TileMethods.TypeByChar[tile.id]]);
            obj.transform.position = Vector3.forward * tile.y * 2 + Vector3.right * tile.x * 2;
            obj.name = $"({tile.x},{tile.y})";
            obj.transform.SetParent(transform);
            m_TilePool.Add(obj);
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
        s_Path = new List<Vector2Int>(pathFinder.FindPath(startPos, endPos));
    }

    public static List<Vector2Int> GetPath()
    {
        return s_Path;
    }

    // Draw path
    //private void OnDrawGizmos()
    //{
    //    if (s_Path != null && s_Path.Count > 0)
    //    {
    //        Gizmos.color = Color.magenta;
    //        for (int i = 0; i < s_Path.Count; i++)
    //        {
    //            Vector3 myPos = new Vector3(s_Path[i].x, 0, s_Path[i].y);
    //            Vector3 neighbourPos = new Vector3(s_Path[i < s_Path.Count - 1 ? i + 1 : i].x, 0, s_Path[i < s_Path.Count - 1 ? i + 1 : i].y);
    //            Gizmos.DrawLine(myPos, neighbourPos);
    //        }
    //    }
    //}
}
