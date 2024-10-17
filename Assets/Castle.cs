using UnityEngine;

public class Castle : MonoBehaviour
{
    public int Health = 100;

    public void TakeDamage(int damage)
    {
        Health -= damage;
        Debug.Log("Castle Health: " + Health);
    }
}