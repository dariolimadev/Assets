// AStarPathfinding.cs
using UnityEngine;
using System.Collections.Generic;

public class AStarPathfinding : MonoBehaviour
{
    public List<Vector2> FindPath(Vector2 startWorld, Vector2 targetWorld)
    {
        Vector2Int start = GridManager.Instance.GetGridPosition(startWorld);
        Vector2Int target = GridManager.Instance.GetGridPosition(targetWorld);

        List<Vector2Int> open = new List<Vector2Int> { start };
        HashSet<Vector2Int> closed = new HashSet<Vector2Int>();
        var cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        var gScore = new Dictionary<Vector2Int, int> { [start] = 0 };
        var fScore = new Dictionary<Vector2Int, int> { [start] = Heuristic(start, target) };

        while (open.Count > 0)
        {
            Vector2Int current = GetLowestF(open, fScore);
            if (current == target) return BuildPath(cameFrom, current);

            open.Remove(current);
            closed.Add(current);

            foreach (var n in GetNeighbors(current))
            {
                if (closed.Contains(n) || !GridManager.Instance.IsWalkable(n)) continue;
                int tentative = gScore[current] + 1;
                if (!gScore.ContainsKey(n) || tentative < gScore[n])
                {
                    cameFrom[n] = current;
                    gScore[n] = tentative;
                    fScore[n] = tentative + Heuristic(n, target);
                    if (!open.Contains(n)) open.Add(n);
                }
            }
        }

        return new List<Vector2>();
    }

    int Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    Vector2Int GetLowestF(List<Vector2Int> list, Dictionary<Vector2Int, int> fScore)
    {
        Vector2Int best = list[0];
        int bestScore = fScore.ContainsKey(best) ? fScore[best] : int.MaxValue;
        foreach (var node in list)
        {
            int s = fScore.ContainsKey(node) ? fScore[node] : int.MaxValue;
            if (s < bestScore) { bestScore = s; best = node; }
        }
        return best;
    }

    List<Vector2Int> GetNeighbors(Vector2Int pos)
    {
        return new List<Vector2Int>
        {
            pos + Vector2Int.up,
            pos + Vector2Int.down,
            pos + Vector2Int.left,
            pos + Vector2Int.right
        };
    }

    List<Vector2> BuildPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
    {
        var path = new List<Vector2>();
        while (cameFrom.ContainsKey(current))
        {
            path.Insert(0, GridManager.Instance.GetWorldPosition(current));
            current = cameFrom[current];
        }
        path.Insert(0, GridManager.Instance.GetWorldPosition(current));
        return path;
    }
}
