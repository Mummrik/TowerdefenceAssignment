using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject m_Target;

    public void SetTarget(GameObject target)
    {
        m_Target = target;
    }

    void Update()
    {
        //transform.position += Vector3.forward * 10 * Time.deltaTime;

        if (transform.position == m_Target.transform.position)
        {
            gameObject.SetActive(false);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, m_Target.transform.position, 0.1f);
        }
    }
}
