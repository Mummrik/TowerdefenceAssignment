using UnityEngine;
using System.Collections.Generic;

namespace AI
{
    public class Dijkstra : IPathFinder
    {
        private List<Vector2Int> m_Grid;
        private List<Vector2Int> m_Path;

        public Dijkstra(List<Vector2Int> newGrid)
        {
            m_Grid = newGrid;
        }

        public IEnumerable<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
        {
            Vector2Int currentNode = start;
            Dictionary<Vector2Int, Vector2Int> ancestors = new Dictionary<Vector2Int, Vector2Int>();
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            queue.Enqueue(currentNode);

            while (queue.Count > 0)
            {
                currentNode = queue.Dequeue();
                if (currentNode == goal)
                {
                    break;
                }

                for (int i = 0; i < Tools.DirectionTools.Dirs.Length; i++)
                {
                    Vector2Int scanNode = currentNode + Tools.DirectionTools.Dirs[i];
                    if (m_Grid.Contains(scanNode))
                    {
                        if (!ancestors.ContainsKey(scanNode))
                        {
                            queue.Enqueue(scanNode);
                            ancestors.Add(scanNode, currentNode);
                        }
                    }
                }
            }

            if (ancestors.ContainsKey(goal))
            {
                m_Path = new List<Vector2Int>();
                foreach (Vector2Int node in ancestors.Values)
                {
                    m_Path.Add(node * 2);
                }
                m_Path.Add(goal * 2);
                return m_Path;
            }
            return null;
        }

    }
}
