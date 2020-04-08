using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyData m_Data;
    private List<Vector2Int> m_PathList;
    private Queue<Vector2Int> m_Path;
    private Vector3 m_TargetPosition;
    private Animator m_Animator;

    public EnemyData Data { get => m_Data;}

    public void ConstructEnemy(EnemyData data, List<Vector2Int> path)
    {
        m_Data = data;
        m_PathList = new List<Vector2Int>(path);
        m_Path = new Queue<Vector2Int>(m_PathList);
        m_Animator = transform.GetComponent<Animator>();
    }

    public void OnSpawn()
    {
        m_Path = new Queue<Vector2Int>(m_PathList);
        transform.position = new Vector3(m_PathList[0].x, 1, m_PathList[0].y);
        m_TargetPosition = transform.position;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (m_Path.Count > 0)
        {
            m_Animator.SetBool("isWalking", true);
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
        else
        {
            //Debug.Log("Enemy have not path");
            m_Animator.SetBool("isWalking", false);
            EnemyManager.s_EnemyPool.Enqueue(gameObject);
            transform.gameObject.SetActive(false);
        }
    }

}
