using System.Collections.Generic;
using Tools;
using UnityEngine;

public struct EnemyData
{
    public int id;
    public int health;
    public float speed;
    public int damage;
}
public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject[] m_EnemyPrefabs = new GameObject[0];
    [SerializeField, Tooltip("Time in seconds.")] private float m_SpawnTime = 1.0f;

    private List<Vector2Int> m_Path;
    private Queue<EnemyData> m_EnemyWaves;
    private float m_Timer;
    private Transform m_Parent;

    public static List<GameObject> s_Enemies;

    private GameObjectPool m_EnemyPool1;
    private GameObjectPool m_EnemyPool2;

    private int m_TotalEnemyCount;

    private void Awake()
    {
        m_EnemyWaves = new Queue<EnemyData>();
        s_Enemies = new List<GameObject>();
        m_Parent = new GameObject("Enemies").transform;
        m_EnemyPool1 = new GameObjectPool(1, m_EnemyPrefabs[1], 1, m_Parent);
        m_EnemyPool2 = new GameObjectPool(1, m_EnemyPrefabs[0], 1, m_Parent);
    }
    public void ConstructEnemyWaves(string enemyData)
    {
        m_Path = MapData.GetPath();

        List<string> waves = new List<string>(enemyData.Split('\n'));
        waves.RemoveAt(0);
        for (int i = 0; i < waves.Count; i++)
        {
            string[] data = waves[i].Split(' ');
            int enemyType1 = int.Parse(data[0]);
            int enemyType2 = int.Parse(data[1]);

            for (int j = 0; j < enemyType1; j++)
            {
                EnemyData enemy = new EnemyData();
                enemy.id = 0;
                enemy.health = 75;
                enemy.speed = 5.0f;
                enemy.damage = 1;

                m_EnemyWaves.Enqueue(enemy);
                m_TotalEnemyCount++;
            }
            for (int j = 0; j < enemyType2; j++)
            {
                EnemyData enemy = new EnemyData();
                enemy.id = 1;
                enemy.health = 125;
                enemy.speed = 2.5f;
                enemy.damage = 2;

                m_EnemyWaves.Enqueue(enemy);
                m_TotalEnemyCount++;
            }
        }
        GameManager.TotalEnemies = m_TotalEnemyCount;
    }

    private void Update()
    {
        if (GameManager.GameState == GameState.IsPlaying)
        {
            if (m_EnemyWaves.Count > 0)
            {
                m_Timer += Time.deltaTime;
                if (m_Timer >= m_SpawnTime)
                {
                    EnemyData enemyData = m_EnemyWaves.Dequeue();

                    if (UnitMethods.TypeById[enemyData.id] == UnitType.Standard)
                    {
                        GameObject obj = m_EnemyPool1.Rent(false);
                        Enemy enemy = obj.GetComponent<Enemy>();
                        enemy.ConstructEnemy(enemyData, m_Path);
                        enemy.OnSpawn();

                        if (!s_Enemies.Contains(obj))
                        {
                            s_Enemies.Add(obj);
                        }
                    }
                    else if (UnitMethods.TypeById[enemyData.id] == UnitType.Big)
                    {
                        GameObject obj = m_EnemyPool2.Rent(false);
                        Enemy enemy = obj.GetComponent<Enemy>();
                        enemy.ConstructEnemy(enemyData, m_Path);
                        enemy.OnSpawn();

                        if (!s_Enemies.Contains(obj))
                        {
                            s_Enemies.Add(obj);
                        }
                    }

                    m_Timer -= m_SpawnTime;
                }
            }
        }
    }
}
