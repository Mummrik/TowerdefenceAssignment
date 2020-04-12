using Tools;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject m_BulletPrefab;
    [SerializeField, Tooltip("In seconds.")] private float m_ShootingCooldown = 1.0f;

    private GameObjectPool m_BulletPool;
    private float m_Timer;

    private void Awake()
    {
        m_BulletPool = new GameObjectPool(1, m_BulletPrefab);
    }

    public void Shoot(GameObject target)
    {
        if (m_Timer >= m_ShootingCooldown)
        {
            m_Timer = 0;
            GameObject bullet = m_BulletPool.Rent(false);
            bullet.transform.position = transform.position;
            Bullet bulletComponent = bullet.GetComponent<Bullet>();
            bulletComponent.SetTargetPosition(target.transform);
            bullet.SetActive(true);
        }
    }

    private void Update()
    {
        if (GameManager.GameState == GameState.IsPlaying)
        {
            m_Timer += Time.deltaTime;
        }
    }
}
