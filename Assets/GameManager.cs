using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab; // Array to hold different enemy types
    public int totalEnemiesToSpawn = 20; // Total number of enemies for this level
    public float spawnInterval = 2.0f; // Time between enemy spawns
    public BoxCollider2D spawnArea; // The BoxCollider2D defining the spawn area
    public Castle castle;
    public GameObject soldierPrefab;
    private int _enemiesSpawned; // Counter for spawned enemies
    private int _enemiesDefeated; // Counter for defeated enemies
    private float _timeSinceLastSpawn;

    public GameObject losePanel;
    public GameObject winPanel;

    void Start()
    {
        Time.timeScale = 1;
        losePanel.SetActive(false);
        winPanel.SetActive(false);
        SpawnUnits();
    }

    private void Update()
    {
        // Handle enemy spawning at intervals
        _timeSinceLastSpawn += Time.deltaTime;
        if (_timeSinceLastSpawn >= spawnInterval && _enemiesSpawned < totalEnemiesToSpawn)
        {
            SpawnEnemy();
            _timeSinceLastSpawn = 0f;
        }

        // Check for game over
        if (castle.Health <= 0)
        {
            TriggerGameOver();
        }

        // Check if all enemies have been spawned and defeated
        if (_enemiesDefeated >= totalEnemiesToSpawn)
        {
            TriggerWin();
        }
    }

    void SpawnEnemy()
    {
        // Spawn the enemy at a random position within the defined spawn area (using your random spawn logic)
        var spawnPosition = Utils.GetRandomPositionInBounds(spawnArea.bounds);
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // enemy.TakeDamage(1);
        _enemiesSpawned++; // Increment the counter for spawned enemies
    }

    public void EnemyDefeated()
    {
        _enemiesDefeated++; // Increment the counter for defeated enemies
    }

    void TriggerGameOver()
    {
        Time.timeScale = 0;
        losePanel.SetActive(true);
    }

    void TriggerWin()
    {
        Time.timeScale = 0;
        winPanel.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void SpawnUnits()
    {
        Instantiate(soldierPrefab, new Vector3(-6f, 3f, 0), Quaternion.identity);
    }
}