using TowerDefense;
using UnityEngine;

public class MapReader : MonoBehaviour
{
    private string[] m_RawTileData;
    private static string[] s_RawEnemyData;
    private static Map[] s_Maps;

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
        s_Maps = new Map[rawMapData.Length];
        m_RawTileData = new string[rawMapData.Length];
        s_RawEnemyData = new string[rawMapData.Length];

        for (int i = 0; i < rawMapData.Length; i++)
        {
            string[] data = rawMapData[i].text.Split('#');
            m_RawTileData[i] = data[0];
            s_RawEnemyData[i] = data[1];
        }

        for (int i = 0; i < s_Maps.Length; i++)
        {
            ConstructMap(i);
        }
    }

    private void Start()
    {
        int randomMap = UnityEngine.Random.Range(0, s_Maps.Length);
        LoadMap(randomMap);
        //Destroy(gameObject);
    }
    public static void LoadMap(int mapId)
    {
        GameObject mapData = Instantiate(Resources.Load<GameObject>("MapData"));
        mapData.name = "MapData";
        mapData.GetComponent<MapData>().DrawMap(s_Maps[mapId]);
        mapData.GetComponent<EnemyManager>().ConstructEnemyWaves(s_RawEnemyData[mapId]);
    }

    private void ConstructMap(int mapId)
    {
        string[] tileLines = m_RawTileData[mapId].Split('\n');
        s_Maps[mapId].maxHeight = tileLines.Length - 1;

        for (int i = 0; i < s_Maps[mapId].maxHeight; i++)
        {
            if (tileLines[i].Length - 1 > s_Maps[mapId].maxWidth)
            {
                s_Maps[mapId].maxWidth = tileLines[i].Length - 1;
            }
        }

        s_Maps[mapId].tiles = new Tile[s_Maps[mapId].maxWidth, s_Maps[mapId].maxHeight];

        for (int i = 0; i < s_Maps[mapId].tiles.Length; i++)
        {
            for (int x = 0; x < s_Maps[mapId].maxWidth; x++)
            {
                for (int y = 0; y < s_Maps[mapId].maxHeight; y++)
                {
                    s_Maps[mapId].tiles[x, y].x = x;
                    s_Maps[mapId].tiles[x, y].y = y;
                }
            }
        }

        for (int y = 0; y < s_Maps[mapId].maxHeight; y++)
        {
            int i = y;
            char[] tileIds = tileLines[i].ToCharArray(0, tileLines[i].Length - 1);
            for (int x = 0; x < tileLines[i].Length - 1; x++)
            {
                s_Maps[mapId].tiles[x, y].id = tileIds[x];
            }
        }
    }
}
