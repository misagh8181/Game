using UnityEngine;
using UnityEngine.UI;

public class Castle : MonoBehaviour
{
    public int maxHealth = 50;

    public int Health { get; private set; }
    public Image healthBar;
    public Text healthText;

    private void Start()
    {
        Health = maxHealth;
        healthBar.fillAmount = Health / (float)maxHealth;
        healthText.text = Health.ToString();
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        healthBar.fillAmount = Health / (float)maxHealth;
        healthText.text = Health.ToString();
    }
}