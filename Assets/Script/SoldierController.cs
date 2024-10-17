using UnityEngine;

namespace Script
{
    public class SoldierController : MonoBehaviour
    {
        public float speed = 3f; // Movement speed of the unit
        public int attackDamage = 10; // Damage dealt by the unit
        public int health = 100; // Health of the unit
        public float attackRange = 1.5f; // Range within which the unit can attack
        public float attackCooldown = 1f; // Time between attacks

        private Enemy targetEnemy; // The current enemy target
        private float timeSinceLastAttack = 0f;

        void Update()
        {
            // If the unit is alive, search for a target
            if (health > 0)
            {
                // If there's no target, find the nearest enemy
                if (!targetEnemy || targetEnemy.health <= 0)
                {
                    FindNextEnemy();
                }

                // Move toward the target if there's an enemy
                if (targetEnemy != null)
                {
                    MoveTowardsEnemy();

                    // If the unit is within range, attack the enemy
                    if (Vector2.Distance(transform.position, targetEnemy.transform.position) <= attackRange)
                    {
                        AttackEnemy();
                    }
                }
            }
        }

        void FindNextEnemy()
        {
            // Find the nearest enemy
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            float minDistance = Mathf.Infinity;

            foreach (Enemy enemy in enemies)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < minDistance && enemy.health > 0)
                {
                    minDistance = distance;
                    targetEnemy = enemy;
                }
            }
        }

        void MoveTowardsEnemy()
        {
            // Move the unit towards the enemy's position
            Vector2 direction = (targetEnemy.transform.position - transform.position).normalized;
            transform.position =
                Vector2.MoveTowards(transform.position, targetEnemy.transform.position, speed * Time.deltaTime);
        }

        void AttackEnemy()
        {
            // Only attack if enough time has passed since the last attack
            timeSinceLastAttack += Time.deltaTime;

            if (timeSinceLastAttack >= attackCooldown)
            {
                targetEnemy.TakeDamage(attackDamage);
                timeSinceLastAttack = 0f; // Reset the attack cooldown
            }
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
            Debug.Log("Soldier health: " + health);

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
    }
}