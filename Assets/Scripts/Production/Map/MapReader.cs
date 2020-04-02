using TowerDefense;
using UnityEngine;

public class MapReader : MonoBehaviour
{
    [SerializeField] private GameObject[] tilePrefabs;

    private string[] rawTileData;
    private string[] rawEnemyData;
    private Map[] maps;

    private struct Map
    {
        public int maxWidth;
        public int maxHeight;
        public Tile[,] tiles;
    }
    private struct Tile
    {
        public int id;
        public int x;
        public int y;
    }

    private void Awake()
    {
        TextAsset[] rawMapData = Resources.LoadAll<TextAsset>(ProjectPaths.RESOURCES_MAP_SETTINGS);
        maps = new Map[rawMapData.Length];
        rawTileData = new string[rawMapData.Length];
        rawEnemyData = new string[rawMapData.Length];

        for (int i = 0; i < rawMapData.Length; i++)
        {
            string[] data = rawMapData[i].text.Split('#');
            rawTileData[i] = data[0];
            rawEnemyData[i] = data[1];
        }

        for (int i = 0; i < maps.Length; i++)
        {
            ConstructMap(i);
        }

        //int randomMap = UnityEngine.Random.Range(0, maps.Length);
        DrawMap(maps[UnityEngine.Random.Range(0, maps.Length)]);
    }

    private void ConstructMap(int mapId)
    {
        string[] tileLines = rawTileData[mapId].Split('\n');
        maps[mapId].maxHeight = tileLines.Length - 1;

        for (int i = 0; i < maps[mapId].maxHeight; i++)
        {
            if (tileLines[i].Length - 1 > maps[mapId].maxWidth)
            {
                maps[mapId].maxWidth = tileLines[i].Length - 1;
            }
        }

        maps[mapId].tiles = new Tile[maps[mapId].maxWidth, maps[mapId].maxHeight];

        for (int i = 0; i < maps[mapId].tiles.Length; i++)
        {
            for (int x = 0; x < maps[mapId].maxWidth; x++)
            {
                for (int y = 0; y < maps[mapId].maxHeight; y++)
                {
                    maps[mapId].tiles[x, y].x = x;
                    maps[mapId].tiles[x, y].y = y;
                }
            }
        }

        for (int y = 0; y < maps[mapId].maxHeight; y++)
        {
            int i = y;
            char[] tileIds = tileLines[i].ToCharArray(0, tileLines[i].Length - 1);
            for (int x = 0; x < tileLines[i].Length - 1; x++)
            {
                maps[mapId].tiles[x, y].id = (int)char.GetNumericValue(tileIds[x]);
            }
        }
    }

    private void DrawMap(Map map)
    {
        foreach (Tile tile in map.tiles)
        {
            GameObject obj = Instantiate(tilePrefabs[(int)TileMethods.TypeById[tile.id]]);
            obj.transform.position = Vector3.forward * tile.y * 2 + Vector3.right * tile.x * 2;
            obj.name = $"({tile.x},{tile.y})";
            obj.transform.SetParent(transform);
        }
        // Set camera position in center of level
        Camera.main.transform.position = new Vector3(((map.maxWidth - 1) * 2) * 0.5f, 25, ((map.maxHeight - 1) * 2) * 0.5f);
    }
}
