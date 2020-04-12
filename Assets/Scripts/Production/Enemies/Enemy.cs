using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyData m_Data;
    private List<Vector2Int> m_PathList;
    private Queue<Vector2Int> m_Path;
    private Vector3 m_TargetPosition;
    private Animator m_Animator;

    private float m_Health;

    private float m_MovementSpeed;
    private bool m_IsSlowed = false;
    private float m_SlowTimer;

    private float m_DespawnTimer;
    private bool m_IsDead;

    public EnemyData Data { get => m_Data; }

    public void ConstructEnemy(in EnemyData data, List<Vector2Int> path)
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
        m_MovementSpeed = m_Data.speed;
        m_Health = m_Data.health;
        m_IsDead = false;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (GameManager.GameState == GameState.IsPlaying)
        {
            if (IsAlive())
            {
                CheckSlowedCondition();
                Movement();
            }
        }
    }

    private bool IsAlive()
    {
        if (m_Health > 0)
        {
            return true;
        }
        else
        {
            if (!m_IsDead)
            {
                m_Animator.ResetTrigger("Damaged");
                m_Animator.SetTrigger("Killed");
                GameManager.EnemyKills++;
            }
            m_IsDead = true;
            m_DespawnTimer += Time.deltaTime;
            if (m_DespawnTimer >= 0.5f)
            {
                m_DespawnTimer = 0;
                m_Animator.ResetTrigger("Damaged");
                m_Animator.ResetTrigger("Killed");
                m_Animator.ResetTrigger("isWalking");
                gameObject.SetActive(false);
            }
        }

        return false;
    }

    private void CheckSlowedCondition()
    {
        if (m_IsSlowed)
        {
            m_SlowTimer += Time.deltaTime;
            if (m_SlowTimer >= 0.5f)
            {
                m_SlowTimer = 0;
                m_IsSlowed = false;
                m_MovementSpeed = m_Data.speed;
            }

        }
    }

    private void Movement()
    {
        if (!m_IsDead)
        {
            if (m_Path.Count > 0)
            {
                m_Animator.SetTrigger("isWalking");
                if (transform.position != m_TargetPosition)
                {
                    transform.LookAt(m_TargetPosition);
                    transform.position = Vector3.MoveTowards(transform.position, m_TargetPosition, m_MovementSpeed * Time.deltaTime);
                }
                else
                {
                    Vector2Int position = m_Path.Dequeue();
                    m_TargetPosition = new Vector3(position.x, transform.position.y, position.y);
                }
            }
            else
            {
                GameManager.TowerHealth -= m_Data.damage;
                transform.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!m_IsDead)
        {
            if (other.TryGetComponent(out Bullet bullet))
            {
                float isCriticalHit = UnityEngine.Random.Range(0f, 1f);
                float damage = UnityEngine.Random.Range(bullet.Damage / 2, bullet.Damage + 1);
                if (damage <= 0) { damage = 1f; }
                damage = damage * (isCriticalHit <= bullet.CritChance ? 2 : 1);

                if (bullet.BulletType == BulletType.Cannon)
                {
                    for (int i = 0; i < EnemyManager.s_Enemies.Count; i++)
                    {
                        GameObject enemy = EnemyManager.s_Enemies[i];
                        if (Vector3.Distance(transform.position, enemy.transform.position) <= 3f)
                        {
                            enemy.GetComponent<Enemy>().m_Health -= damage;
                            enemy.GetComponent<Animator>().SetTrigger("Damaged");
                        }
                    }
                }
                else if (bullet.BulletType == BulletType.Frost)
                {
                    if (m_IsSlowed == false)
                    {
                        m_MovementSpeed = m_MovementSpeed * 0.3f;
                        m_IsSlowed = true;
                    }

                    m_Animator.SetTrigger("Damaged");
                    m_Health -= damage;
                }

                bullet.gameObject.SetActive(false);
            }
        }
    }
}
