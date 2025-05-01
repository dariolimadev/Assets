using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyMovementSQM : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float pathUpdateInterval = 0.5f;
    public float visionRange = 6f;

    private Queue<Vector2> pathQueue = new Queue<Vector2>();
    private Rigidbody2D rb;
    private Transform player;
    private AStarPathfinding pathfinder;

    private bool isMoving = false;
    private Vector2 targetPosition;
    private float gridCellSize => GridManager.Instance.cellSize;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        rb.freezeRotation = true;

        transform.position = AlignToGrid(transform.position);
        targetPosition = transform.position;

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        pathfinder = FindObjectOfType<AStarPathfinding>();

        StartCoroutine(UpdatePathRoutine());
    }

    void Update()
    {
        if (isMoving)
        {
            MoveToTarget();
        }
        else if (pathQueue.Count > 0)
        {
            targetPosition = pathQueue.Dequeue();
            isMoving = true;
        }
    }

    IEnumerator UpdatePathRoutine()
    {
        while (true)
        {
            if (player != null && Vector2.Distance(transform.position, player.position) < visionRange)
            {
                List<Vector2> path = pathfinder.FindPath(transform.position, player.position);

                if (path != null && path.Count > 1)
                {
                    pathQueue.Clear();

                    // pula o primeiro (posição atual), adiciona o restante
                    for (int i = 1; i < path.Count; i++)
                        pathQueue.Enqueue(path[i]);
                }
            }

            yield return new WaitForSeconds(pathUpdateInterval);
        }
    }

    void MoveToTarget()
    {
        rb.MovePosition(Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.deltaTime));

        if (Vector2.Distance(rb.position, targetPosition) < 0.01f)
        {
            rb.position = targetPosition;
            isMoving = false;
        }
    }

    Vector2 AlignToGrid(Vector2 position)
    {
        return GridManager.Instance.GetWorldPosition(GridManager.Instance.GetGridPosition(position));
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying || GridManager.Instance == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        if (pathQueue != null)
        {
            Gizmos.color = Color.magenta;
            foreach (Vector2 pos in pathQueue)
                Gizmos.DrawCube(pos, Vector3.one * 0.3f);
        }
    }
}
