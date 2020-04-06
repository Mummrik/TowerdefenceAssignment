using UnityEngine;
using System.Collections.Generic;

namespace AI
{
    //TODO: Implement IPathFinder using Dijsktra algorithm.
    public class Dijkstra : IPathFinder
    {
        private List<Vector2Int> grid;
        private List<Vector2Int> path;

        public Dijkstra(List<Vector2Int> newGrid)
        {
            grid = newGrid;
            path = new List<Vector2Int>();
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
                    if (grid.Contains(scanNode))
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
                foreach (Vector2Int node in ancestors.Values)
                {
                    path.Add(node);
                }
                path.Add(goal);
                return path;
            }
            return null;
        }

    }
}
