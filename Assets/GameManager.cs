using UnityEngine;
using UnityEngine.SceneManagement;
using Vector2 = System.Numerics.Vector2;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab; // Array to hold different enemy types
    public int totalEnemiesToSpawn = 20; // Total number of enemies for this level
    public float spawnInterval = 2.0f; // Time between enemy spawns
    public BoxCollider2D spawnArea; // The BoxCollider2D defining the spawn area
    public Castle castle;
    public GameObject soldierPrefab;
    public GameObject archerPrefab;
    private int _enemiesSpawned; // Counter for spawned enemies
    private int _enemiesHandled; // Counter for defeated enemies
    private float _timeSinceLastSpawn;
    private int _numberOfArcherSpawned;

    private Vector2[] _archerSpawnPositions =
    {
        new(-11, 4), new(-11, 2), new(-11, 0), new(-11, -2), new(-11, -4),
        new(-9, 4), new(-9, 2), new(-9, 0), new(-9, -2), new(-9 - 4)
    };

    public GameObject losePanel;
    public GameObject winPanel;

    private void Start()
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
        if (_enemiesHandled >= totalEnemiesToSpawn)
        {
            TriggerWin();
        }
    }

    private void SpawnEnemy()
    {
        // Spawn the enemy at a random position within the defined spawn area (using your random spawn logic)
        var spawnPosition = Utils.GetRandomPositionInBounds(spawnArea.bounds);
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // enemy.TakeDamage(1);
        _enemiesSpawned++; // Increment the counter for spawned enemies
    }

    public void EnemyDefeated()
    {
        _enemiesHandled++; // Increment the counter for defeated enemies
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

    public void SpawnArcher()
    {
        if (_numberOfArcherSpawned < 10)
        {
            var position = _archerSpawnPositions[_numberOfArcherSpawned++];
            Instantiate(archerPrefab, new Vector3(position.X, position.Y), Quaternion.identity);
        }
    }

    // Function to handle when an enemy reaches the castle
    public void EnemyReachedCastle()
    {
        _enemiesHandled++; // Increment handled count when an enemy reaches the castle
        // Check if all enemies are handled
        if (_enemiesHandled >= totalEnemiesToSpawn)
        {
            TriggerWin();
        }
    }
}