using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemyMovementSQM movement;

    void Start()
    {
        movement = GetComponent<EnemyMovementSQM>();
        if (movement == null)
        {
            Debug.LogError("EnemyController requires an EnemyMovementSQM component!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Enemy attacked Player!");
        }
    }
}