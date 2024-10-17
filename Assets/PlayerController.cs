using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject projectilePrefab; // The projectile to shoot
    public Transform firePoint; // The point where projectiles will be instantiated
    public float shootInterval = 2f; // Time between each shot
    private float timeSinceLastShot = 0f;

    void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        if (timeSinceLastShot >= shootInterval)
        {
            // Shoot only if there are enemies in the scene
            GameObject targetEnemy = FindNearestEnemy();
            if (targetEnemy != null)
            {
                Shoot(targetEnemy);
                timeSinceLastShot = 0f; // Reset the timer
            }
        }
    }

    void Shoot(GameObject targetEnemy)
    {
        var projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Calculate the direction to the target
        Vector2 direction = targetEnemy.transform.position;

        // Set the direction and launch the arrow
        var projScript = projectile.GetComponent<Projectile>();
        projScript.Launch(direction);
    }

    // This method finds the nearest enemy by checking the distance to each enemy in the scene
    GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy; // Returns the closest enemy, or null if no enemies are found
    }
}