using Script;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 500; // Maximum health of the enemy
    public int health; // Current health of the enemy
    public int damageToUnit = 20; // Damage the enemy deals to a unit
    public int damageToCastle = 10; // Damage the enemy deals to a unit
    public float speed = 2f; // Speed of movement towards the castle
    public float attackCooldown = 1f; // Time between attacks

    private SoldierController _currentSoldier; // Reference to the unit currently engaged with this enemy
    private Vector2 _castlePositionTarget;
    private GameManager _gameManager;
    private float _timeSinceLastAttack;
    private UIEnemy _uiEnemy;
    private Animator _animator;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _animator = GetComponent<Animator>();
        _uiEnemy = GetComponent<UIEnemy>();
        _uiEnemy.StartHealth(maxHealth);

        health = Random.Range(50, maxHealth);

        _castlePositionTarget =
            Utils.GetRandomPositionInBounds(GameObject.FindWithTag("Castle").GetComponent<BoxCollider2D>().bounds);
    }

    private void Update()
    {
        // If no unit is engaging the enemy or the unit is dead, continue moving towards the castle
        if (!_currentSoldier || _currentSoldier.health <= 0)
        {
            MoveTowardsCastle();
        }
        else
        {
            EngageUnit();
        }
    }

    void MoveTowardsCastle()
    {
        // Move the enemy towards the castle's position
        transform.position = Vector2.MoveTowards(transform.position, _castlePositionTarget, speed * Time.deltaTime);
    }

    private void EngageUnit()
    {
        // Only attack if enough time has passed since the last attack
        _timeSinceLastAttack += Time.deltaTime;

        if (_currentSoldier && _timeSinceLastAttack >= attackCooldown)
        {
            _currentSoldier.TakeDamage(damageToUnit);
            _timeSinceLastAttack = 0f; // Reset the attack cooldown
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
        if (other.CompareTag("Soldier"))
        {
            _animator.SetTrigger("Engage");
            _currentSoldier = other.GetComponent<SoldierController>();
        }
        else if (other.CompareTag("Castle"))
        {
            _gameManager.EnemyReachedCastle(damageToCastle);
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Soldier"))
        {
            _animator.SetTrigger("Engage");
            _currentSoldier = null;
        }
    }
}