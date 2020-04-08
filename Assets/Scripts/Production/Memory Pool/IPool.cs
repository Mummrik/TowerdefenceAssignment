using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public interface IPool<T>
    {
        T Rent(bool returnActive);
    }

    public class GameObjectPool : IPool<GameObject>
    {
        private readonly uint m_ExpandBy;
        private readonly GameObject m_Prefab;
        private readonly Transform m_Parent;

        private readonly Stack<GameObject> m_Objects = new Stack<GameObject>();

        public GameObjectPool(uint initSize, GameObject prefab, uint expandBy = 1, Transform parent = null)
        {
            m_ExpandBy = (uint)Mathf.Max(1, expandBy);
            m_Prefab = prefab;
            m_Parent = parent;
            m_Prefab.SetActive(false);
            Expand((uint)Mathf.Max(1, initSize));
        }

        private void Expand(uint amount)
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject obj = Object.Instantiate(m_Prefab, m_Parent);
                EmitOnDisable emitOnDisable = obj.AddComponent<EmitOnDisable>();
                emitOnDisable.OnDisableGameObject += UnRent;
                m_Objects.Push(obj);
            }
        }

        private void UnRent(GameObject gameObject)
        {
            m_Objects.Push(gameObject);
        }

        public GameObject Rent(bool returnActive)
        {
            if (m_Objects.Count == 0)
            {
                Expand(m_ExpandBy);
            }
            GameObject obj = m_Objects.Pop();
            obj.SetActive(returnActive);
            return obj;
        }
    }
}