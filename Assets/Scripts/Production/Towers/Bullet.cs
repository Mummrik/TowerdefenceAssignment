using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private BulletType m_BulletType;
    [SerializeField, Range(1, 100), Tooltip("Percent to score a critical hit. (Damage * 2)")] private int m_CritChance = 2;
    [SerializeField] private int m_MaxDamage;

    private Vector3 m_TargetPosition;

    public int Damage { get => m_MaxDamage; }
    public BulletType BulletType { get => m_BulletType; }
    public float CritChance { get => m_CritChance * 0.01f; }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        m_TargetPosition = targetPosition;
    }

    void Update()
    {
        if (transform.position == m_TargetPosition)
        {
            gameObject.SetActive(false);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, m_TargetPosition, 0.1f);
        }
    }
}
