using UnityEngine;
using UnityEngine.UI;

namespace Script
{
    public class SoldierController : MonoBehaviour
    {
        public float speed = 3f; // Movement speed of the unit
        public int attackDamage = 10; // Damage dealt by the unit
        public int health = 100; // Health of the unit
        public float attackRange = 1.5f; // Range within which the unit can attack
        public float attackCooldown = 1f; // Time between attacks
        private Animator _animator;
        
        private Enemy _targetEnemy; // The current enemy target
        private float _timeSinceLastAttack;
        private Canvas _canvas;
        private Slider _slider;
        private Image _getSliderImage;
        private bool _isAttacked = false;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _canvas = GetComponentInChildren<Canvas>();
            _slider = _canvas.GetComponentInChildren<Slider>();
            _getSliderImage = _slider.GetComponentInChildren<Image>();
            _slider.value = _slider.maxValue = health;
            _slider.minValue = 0;
            _canvas.worldCamera = Camera.main;
            UpdateSliderColor();
        }

        void Update()
        {
            // If the unit is alive, search for a target
            if (health > 0)
            {
                // If there's no target, find the nearest enemy
                if (!_targetEnemy || _targetEnemy.health <= 0)
                {
                    _isAttacked = false;
                    _animator.SetBool("Attacked", false);
                    FindNextEnemy();
                }

                // Move toward the target if there's an enemy
                if (_targetEnemy)
                {
                    // If the unit is within range, attack the enemy
                    if (Vector2.Distance(transform.position, _targetEnemy.transform.position) <= attackRange)
                    {
                        _isAttacked = true;
                        _animator.SetBool("Attacked", true);
                        AttackEnemy();
                    }
                    else
                    {
                        MoveTowardsEnemy();
                    }
                }
            }
        }

        private void FindNextEnemy()
        {
            // Find the nearest enemy
            var enemies = FindObjectsOfType<Enemy>();
            var minDistance = Mathf.Infinity;

            foreach (Enemy enemy in enemies)
            {
                var distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (!(distance < minDistance) || enemy.health <= 0) continue;
                minDistance = distance;
                _targetEnemy = enemy;
            }
        }

        private void MoveTowardsEnemy()
        {
            // Move the unit towards the enemy's position
            transform.position =
                Vector2.MoveTowards(transform.position, _targetEnemy.transform.position, speed * Time.deltaTime);
        }

        private void AttackEnemy()
        {
            // Only attack if enough time has passed since the last attack
            _timeSinceLastAttack += Time.deltaTime;

            if (_timeSinceLastAttack >= attackCooldown)
            {
                _targetEnemy.TakeDamage(attackDamage);
                _timeSinceLastAttack = 0f; // Reset the attack cooldown
            }
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
            _slider.value = health;
            UpdateSliderColor();

            if (health <= 0)
            {
                Die();
            }
        }

        void Die()
        {
            // Handle unit death (e.g., play death animation, destroy object)
            Destroy(gameObject);
        }

        private void UpdateSliderColor()
        {
            var healthPercent = Mathf.InverseLerp(_slider.minValue, _slider.maxValue, health);
            var newColor = Color.Lerp(Color.red, Color.green, healthPercent);
            _getSliderImage.color = newColor;
        }
    }
}