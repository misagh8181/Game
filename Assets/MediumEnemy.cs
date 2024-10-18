using UnityEngine;

public class MediumEnemy : MonoBehaviour
{
    public int maxHealth = 500; // Maximum health of the enemy
    public int health; // Current health of the enemy
    public int damageToCastle = 15; // Damage the enemy deals to a castle
    public float speed = 2f; // Speed of movement towards the castle
    public float stopTime = 2f; // Time before the enemy stops
    private Vector2 _castlePositionTarget;
    private GameManager _gameManager;
    private float _timeSinceEnteredCanvas;
    private UIEnemy _uiEnemy;
    private Animator _animator;
    private bool _hasStopped = false; // Boolean to control stopping and running animation

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _animator = GetComponent<Animator>();
        _uiEnemy = GetComponent<UIEnemy>();
        _uiEnemy.StartHealth(maxHealth);

        // Set health within a range for medium enemy
        health = Random.Range(100, maxHealth);

        // Get random castle position target
        _castlePositionTarget = Utils.GetRandomPositionInBounds(GameObject.FindWithTag("Castle").GetComponent<BoxCollider2D>().bounds);
    }

    private void Update()
    {
        // Handle stopping and triggering the animation
        if (!_hasStopped)
        {
            _timeSinceEnteredCanvas += Time.deltaTime;

            if (_timeSinceEnteredCanvas >= stopTime)
            {
                StopAndTriggerAnimation();
            }
            else
            {
                MoveTowardsCastle();
            }
        }
        else
        {
            MoveTowardsCastle();
        }
    }

    private void StopAndTriggerAnimation()
    {
        _hasStopped = true; // Set the stopping condition
        speed = 0f; // Stop movement

        // Trigger the animation based on the condition
        _animator.SetBool("Stopped", true); // Assuming you have an "Stopped" boolean condition in the Animator
    }

    private void MoveTowardsCastle()
    {
        // Move the enemy towards the castle's position
        if (speed > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, _castlePositionTarget, speed * Time.deltaTime);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth); // Ensure health stays within bounds
        _uiEnemy.SetHealth(health);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _gameManager.EnemyDefeated();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only handle collision with the castle, ignore soldiers
        if (other.CompareTag("Castle"))
        {
            _gameManager.EnemyReachedCastle(damageToCastle);
            Destroy(gameObject);
        }
    }
    
}
