using UnityEngine;

public class Utils
{
    // Example: Random position inside a rectangle (as from previous conversation)
    public static Vector2 GetRandomPositionInBounds(Bounds bounds)
    {
        var randomX = Random.Range(bounds.min.x, bounds.max.x);
        var randomY = Random.Range(bounds.min.y, bounds.max.y);
        return new Vector2(randomX, randomY);
    }
}