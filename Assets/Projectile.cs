using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f; // Horizontal speed
    public float arcHeight = 5f; // Controls the height of the arc
    public int damage = 100;
    private Vector2 startPosition;
    private Vector2 targetPosition;
    private float timeElapsed;

    public void Launch(Vector2 target)
    {
        startPosition = transform.position;
        targetPosition = target;
        timeElapsed = 0f;

        // Set the initial rotation to -45 degrees
        transform.rotation = Quaternion.Euler(0, 0, -45f);
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        // Horizontal movement
        float progress = timeElapsed * speed / Vector2.Distance(startPosition, targetPosition);
        float xPosition = Mathf.Lerp(startPosition.x, targetPosition.x, progress);

        // Create an arc effect for the Y position
        float yPosition = Mathf.Lerp(startPosition.y, targetPosition.y, progress);
        float arc = arcHeight * Mathf.Sin(Mathf.Clamp01(progress) * Mathf.PI); // Sin curve for the arc

        // Update the arrow's position
        transform.position = new Vector2(xPosition, yPosition + arc);

        // Smoothly rotate the arrow from -45° to -135° based on its progress
        float rotationAngle = Mathf.Lerp(-45f, -170f, progress);
        transform.rotation = Quaternion.Euler(0, 0, rotationAngle);

        // Destroy the arrow if it has reached its target
        if (progress >= 1f)
        {
            Destroy(gameObject);
        }
    }
}