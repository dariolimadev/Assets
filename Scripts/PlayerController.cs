using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float tileSize = 1f;
    [SerializeField] private int maxHealth = 100;
    private int _currentHealth;

    private Rigidbody2D _rb;
    private Vector2 _moveDirection;
    private Vector2 _targetPosition;
    private bool _isMoving = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f;
        _rb.freezeRotation = true;
    }

    private void Start()
    {
        AlignToGrid();
        _targetPosition = transform.position;
        _currentHealth = maxHealth;
    }

    private void Update()
    {
        if (_isMoving) return;

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        _moveDirection = new Vector2(x, y).normalized;

        if (_moveDirection != Vector2.zero)
        {
            StartMove();
        }
    }

    private void FixedUpdate()
    {
        if (_isMoving)
        {
            _rb.MovePosition(Vector2.MoveTowards(transform.position, _targetPosition, moveSpeed * Time.fixedDeltaTime));

            if (Vector2.Distance(transform.position, _targetPosition) < 0.001f)
            {
                transform.position = _targetPosition;
                _isMoving = false;
            }
        }
    }

    private void StartMove()
    {
        Vector2 alignedPos = AlignToGrid(transform.position);
        Vector2 nextPos = alignedPos + _moveDirection * tileSize;
        _targetPosition = AlignToGrid(nextPos);
        _isMoving = true;
    }

    private void AlignToGrid()
    {
        transform.position = AlignToGrid(transform.position);
        _targetPosition = transform.position;
    }

    private Vector2 AlignToGrid(Vector2 position)
    {
        Vector2Int gridPos = GridManager.Instance.GetGridPosition(position); // Usa GridManager
        return GridManager.Instance.GetWorldPosition(gridPos); // Usa GridManager
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(AlignToGrid(transform.position), new Vector2(tileSize, tileSize));

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_targetPosition, new Vector2(tileSize, tileSize));
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        Debug.Log("Jogador recebeu " + damage + " de dano. Vida atual: " + _currentHealth);

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Jogador morreu!");
        Destroy(gameObject);
    }
}