using TowerDefense;
using UnityEngine;

public class MapReader : MonoBehaviour
{
    private string[] rawTileData;
    private string[] rawEnemyData;
    private static Map[] maps;

    public struct Map
    {
        public int maxWidth;
        public int maxHeight;
        public Tile[,] tiles;
    }
    public struct Tile
    {
        public char id;
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
    }

    private void Start()
    {
        int randomMap = UnityEngine.Random.Range(0, maps.Length);
        LoadMap(randomMap);
        //Destroy(gameObject);
    }
    public static void LoadMap(int mapId)
    {
        GameObject mapData = Instantiate(Resources.Load<GameObject>("MapData"));
        mapData.name = "MapData";
        mapData.GetComponent<MapData>().DrawMap(maps[mapId]);
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
                maps[mapId].tiles[x, y].id = tileIds[x];
            }
        }
    }
}
