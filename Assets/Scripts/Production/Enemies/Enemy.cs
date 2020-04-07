using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyData m_Data;
    private Queue<Vector2Int> m_Path;
    private Vector3 m_TargetPosition;

    public void ConstructEnemy(EnemyData data, List<Vector2Int> path)
    {
        m_Data = data;
        m_Path = new Queue<Vector2Int>(path);
        Vector2Int tempPos = m_Path.Dequeue();
        m_TargetPosition = new Vector3(tempPos.x, transform.position.y, tempPos.y);
    }

    private void Update()
    {
        if (m_Path.Count > 0)
        {
            if (transform.position != m_TargetPosition)
            {
                transform.LookAt(m_TargetPosition);
                transform.position = Vector3.MoveTowards(transform.position, m_TargetPosition, m_Data.speed * Time.deltaTime);

            }
            else
            {
                Vector2Int tempPos = m_Path.Dequeue();
                m_TargetPosition = new Vector3(tempPos.x, transform.position.y, tempPos.y);
            }
        }
    }

}
