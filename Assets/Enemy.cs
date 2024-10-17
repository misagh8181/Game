using Script;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 500; // Maximum health of the enemy
    public int health; // Current health of the enemy
    public int damageToUnit = 20; // Damage the enemy deals to a unit
    public float speed = 2f; // Speed of movement towards the castle
    public int maxColorHealth = 500; // Health at which the enemy is fully red
    public int minColorHealth = 20; // Health at which the enemy is fully green
    public float attackCooldown = 1f; // Time between attacks

    private SoldierController _currentSoldier; // Reference to the unit currently engaged with this enemy
    private SpriteRenderer _spriteRenderer;
    private Vector2 _castlePositionTarget;
    private GameManager _gameManager;
    private float _timeSinceLastAttack;
    private Canvas _canvas;
    private Slider _slider;


    private void Start()
    {
        health = Random.Range(50, maxHealth); // Set the enemy's health to maximum at the start
        _canvas = GetComponentInChildren<Canvas>();
        _slider = _canvas.GetComponentInChildren<Slider>();
        _slider.value = _slider.maxValue = health;
        _slider.minValue = 0;
        _canvas.worldCamera = Camera.main;

        _gameManager = FindObjectOfType<GameManager>();
        _castlePositionTarget =
            Utils.GetRandomPositionInBounds(GameObject.FindWithTag("Castle").GetComponent<BoxCollider2D>().bounds);
        Debug.Log(_castlePositionTarget);
        _spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        UpdateColor(); // Set initial color based on health
        UpdateSliderColor();
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
        _slider.value = health;
        UpdateColor(); // Update color based on health
        UpdateSliderColor();

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        _gameManager.EnemyDefeated();
        Destroy(gameObject);
    }

    void UpdateColor()
    {
        float healthPercent = Mathf.InverseLerp(minColorHealth, maxColorHealth, health);
        Color newColor = Color.Lerp(Color.green, Color.red, healthPercent);
        _spriteRenderer.color = newColor;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Soldier"))
        {
            _currentSoldier = other.GetComponent<SoldierController>();
        }
        else if (other.CompareTag("Castle"))
        {
            other.GetComponent<Castle>().TakeDamage(10);
            _gameManager.EnemyReachedCastle();
            Destroy(gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Soldier"))
        {
            _currentSoldier = null;
        }
    }

    private void UpdateSliderColor()
    {
        var healthPercent = Mathf.InverseLerp(_slider.minValue, _slider.maxValue, health);
        var newColor = Color.Lerp(Color.red, Color.green, healthPercent);
        _slider.GetComponentInChildren<Image>().color = newColor;
    }
}