using UnityEngine;
using UnityEngine.UI;

public class Castle : MonoBehaviour
{
    public int maxHealth = 50;

    public int Health { get; private set; }
    public Slider healthSlider;

    private void Start()
    {
        Health = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = Health;
        healthSlider.minValue = 0;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        healthSlider.value = Health;
    }
}