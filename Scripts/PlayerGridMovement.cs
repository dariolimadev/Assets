using UnityEngine;
using System.Collections;

public class PlayerGridMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gridSize = 1f;

    private Vector2 targetPosition2D;
    private bool isMoving = false;

    void Start()
    {
        AlignToGrid();
        targetPosition2D = transform.position;
    }

    void Update()
    {
        if (isMoving)
        {
            MoveToTarget();
            return;
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector2 moveDirection = new Vector2(horizontalInput, verticalInput).normalized;

        if (moveDirection != Vector2.zero)
        {
            StartMove(moveDirection);
        }
    }

    void StartMove(Vector2 moveDirection)
    {
        Vector2Int currentCell = GridManager.Instance.GetGridPosition(transform.position);
        Vector2 nextPosition = GridManager.Instance.GetWorldPosition(currentCell) + moveDirection * gridSize;
        targetPosition2D = nextPosition;
        isMoving = true;
    }

    void MoveToTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition2D, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPosition2D) < 0.001f)
        {
            AlignToGrid();
            isMoving = false;
        }
    }

    void AlignToGrid()
    {
        Vector2Int currentCell = GridManager.Instance.GetGridPosition(transform.position);
        transform.position = GridManager.Instance.GetWorldPosition(currentCell);
        targetPosition2D = transform.position;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Vector2Int currentCell = GridManager.Instance.GetGridPosition(transform.position);
        Vector2 alignedPos = GridManager.Instance.GetWorldPosition(currentCell);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(alignedPos.x, alignedPos.y, transform.position.z), Vector3.one * gridSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(targetPosition2D.x, targetPosition2D.y, transform.position.z), Vector3.one * gridSize);
    }
}