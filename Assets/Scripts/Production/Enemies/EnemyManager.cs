using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EnemyData
{
    public int id;
    public int health;
    public float speed;
}
public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject[] m_EnemyPrefabs;
    [SerializeField, Tooltip("Time in seconds.")] private float m_SpawnTime = 1.0f;

    private List<Vector2Int> m_Path;
    private Queue<EnemyData> m_Enemies = new Queue<EnemyData>();
    private float m_Timer;



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
                enemy.health = 10;
                enemy.speed = 5.0f;

                m_Enemies.Enqueue(enemy);
            }
            for (int j = 0; j < enemyType2; j++)
            {
                EnemyData enemy = new EnemyData();
                enemy.id = 1;
                enemy.health = 20;
                enemy.speed = 2.5f;

                m_Enemies.Enqueue(enemy);
            }
        }
        
    }

    private void Update()
    {
        if (m_Enemies.Count > 0)
        {
            m_Timer += Time.deltaTime;
            if (m_Timer >= m_SpawnTime)
            {
                EnemyData enemyData = m_Enemies.Dequeue();
                GameObject obj = Instantiate(m_EnemyPrefabs[enemyData.id]);
                obj.transform.position = new Vector3(m_Path[0].x, 1, m_Path[0].y);
                Enemy enemy = obj.GetComponent<Enemy>();
                enemy.ConstructEnemy(enemyData, m_Path);
                m_Timer -= m_SpawnTime;
            }
        }
    }
}
