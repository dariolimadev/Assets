using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    public float cellSize = 1f;
    public Vector2 gridOffset = Vector2.zero;
    public int reserveRadius = 3;

    public Color availableColor = Color.green;
    public Color reservedColor = Color.red;
    public Color invalidColor = Color.gray;

    public LayerMask obstacleLayer;

    private HashSet<Vector2Int> reservedCells = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> blockedCells = new HashSet<Vector2Int>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        ScanForObstacles();
    }

    public Vector2Int GetGridPosition(Vector2 worldPos)
    {
        int x = Mathf.FloorToInt((worldPos.x - gridOffset.x) / cellSize);
        int y = Mathf.FloorToInt((worldPos.y - gridOffset.y) / cellSize);
        return new Vector2Int(x, y);
    }

    public Vector2 GetWorldPosition(Vector2Int gridPos)
    {
        return new Vector2(gridPos.x * cellSize + cellSize * 0.5f + gridOffset.x, gridPos.y * cellSize + cellSize * 0.5f + gridOffset.y);
    }

    public bool IsWalkable(Vector2Int gridPos)
    {
        return !reservedCells.Contains(gridPos) && !blockedCells.Contains(gridPos);
    }

    public bool ReserveCell(Vector2Int gridPos)
    {
        if (!reservedCells.Contains(gridPos))
        {
            reservedCells.Add(gridPos);
            return true;
        }
        return false;
    }

    public void ReleaseCell(Vector2Int gridPos)
    {
        reservedCells.Remove(gridPos);
    }

    public bool IsReserved(Vector2Int gridPos)
    {
        return reservedCells.Contains(gridPos);
    }

    private void ScanForObstacles()
    {
        int gridSizeX = 50;
        int gridSizeY = 50;

        for (int x = -gridSizeX / 2; x < gridSizeX / 2; x++)
        {
            for (int y = -gridSizeY / 2; y < gridSizeY / 2; y++)
            {
                Vector2Int cell = new Vector2Int(x, y);
                Vector2 worldPos = GetWorldPosition(cell);

                Collider2D hit = Physics2D.OverlapBox(worldPos, Vector2.one * cellSize * 0.9f, 0f, obstacleLayer);
                if (hit != null)
                {
                    blockedCells.Add(cell);
                }
            }
        }

        Debug.Log("Células bloqueadas detectadas: " + blockedCells.Count);
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        int gridSizeX = 50;
        int gridSizeY = 50;

        for (int x = -gridSizeX / 2; x < gridSizeX / 2; x++)
        {
            for (int y = -gridSizeY / 2; y < gridSizeY / 2; y++)
            {
                Vector2Int cell = new Vector2Int(x, y);
                Vector2 worldPos = GetWorldPosition(cell);

                if (reservedCells.Contains(cell))
                    Gizmos.color = reservedColor;
                else if (blockedCells.Contains(cell))
                    Gizmos.color = invalidColor;
                else
                {
                    Color translucent = availableColor;
                    translucent.a = 0.2f;
                    Gizmos.color = translucent;
                }

                Gizmos.DrawCube(new Vector3(worldPos.x, worldPos.y, 0), Vector3.one * (cellSize * 0.95f));
            }
        }
    }
}
