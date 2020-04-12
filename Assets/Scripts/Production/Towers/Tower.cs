using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private float m_ShootingRange = 6;

    private GameObject m_Target;
    private Transform m_Turret;
    private bool m_ReverseSpin;
    private Weapon m_Weapon;

    private void Awake()
    {
        m_Turret = transform.GetChild(0).GetChild(0);
        m_Weapon = transform.GetComponent<Weapon>();
        m_Weapon.transform.position = m_Turret.position;
        if (Random.Range(0, 10) < 5)
        {
            m_ReverseSpin = true;
        }
    }

    private void Update()
    {
        if (GameManager.GameState == GameState.IsPlaying)
        {
            if (m_Target == null)
            {
                m_Turret.Rotate(new Vector3(0, m_ReverseSpin ? -0.25f : 0.25f, 0));
                for (int i = 0; i < EnemyManager.s_Enemies.Count; i++)
                {
                    GameObject target = EnemyManager.s_Enemies[i];
                    if (target.activeSelf && Vector3.Distance(transform.position, target.transform.position) <= m_ShootingRange)
                    {
                        m_Target = target;
                        break;
                    }
                }
            }
            else
            {
                if (!m_Target.activeSelf || Vector3.Distance(transform.position, m_Target.transform.position) > m_ShootingRange)
                {
                    m_Target = null;
                    return;
                }
                m_Turret.LookAt(m_Target.transform);
                m_Weapon.Shoot(m_Target);
            }
        }
    }

}
