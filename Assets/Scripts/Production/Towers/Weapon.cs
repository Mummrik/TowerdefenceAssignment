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
            bulletComponent.SetTarget(target);
            //bulletComponent.Reset();
            bullet.SetActive(true);
            //bulletComponent.Push();
        }
    }

    private void Update()
    {
        m_Timer += Time.deltaTime;
    }
}
