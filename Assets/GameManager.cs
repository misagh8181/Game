using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vector2 = System.Numerics.Vector2;

public class GameManager : MonoBehaviour
{
    public GameObject weakEnemyPrefab;
    public GameObject mediumEnemyPrefab;
    public GameObject strongEnemyPrefab;
    public int totalEnemiesToSpawn = 5; // Total number of enemies for this level
    public float spawnInterval = 2.0f; // Time between enemy spawns
    public BoxCollider2D spawnArea; // The BoxCollider2D defining the spawn area
    public Castle castle;
    public GameObject soldierPrefab;
    public GameObject archerPrefab;

    public AudioSource audioSource;

    public Text moneyText;
    private int _enemiesSpawned; // Counter for spawned enemies
    private int _enemiesHandled; // Counter for defeated enemies
    private float _timeSinceLastSpawn;
    private int _numberOfLauncherSpawned;

    public int costOfLauncher = 100;
    public int costOfSoldier = 300;

    private int _levelNumber = 1;
    private int _numberOfSoliderEnemies;
    private int _numberOfTankEnemies;
    private int _numberOfMissileEnemies;


    private int _money = 500;


    // private Vector2[] _archerSpawnPositions =
    // {
    //     new(-11, 4), new(-11, 2), new(-11, 0), new(-11, -2), new(-11, -4),
    //     new(-9, 4), new(-9, 2), new(-9, 0), new(-9, -2), new(-9 - 4)
    // };
    private Vector2[] _launcherSpawnPositions =
    {
        new(-11, 6), new(-13, 4), new(-15, 2)
    };

    public GameObject losePanel;
    public GameObject winPanel;

    private void Start()
    {
        if (PlayerPrefs.GetInt("level", 0) == 0)
        {
            PlayerPrefs.SetInt("level", 1);
        }

        DontDestroyOnLoad(audioSource.gameObject);
        _levelNumber = PlayerPrefs.GetInt("level", _levelNumber);
        totalEnemiesToSpawn = (int)Mathf.Log(_levelNumber * 10, 1.1f);
        _money = _levelNumber * 100 + 400;
        moneyText.text = _money.ToString();

        Time.timeScale = 1;
        losePanel.SetActive(false);
        winPanel.SetActive(false);
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
        // Logarithmic growth for medium and strong enemies, weak enemies decrease
        float strongEnemyChance = Mathf.Clamp01(Mathf.Log(_levelNumber + 1) / 5f); // Increase strong enemy chance
        float mediumEnemyChance = Mathf.Clamp01(Mathf.Log(_levelNumber + 1) / 7f); // Increase medium enemy chance
        float weakEnemyChance = Mathf.Clamp01(1f - (strongEnemyChance + mediumEnemyChance)); // Weak enemies decrease

        // Randomly select enemy type based on the level
        float randomValue = Random.value;
        GameObject enemyPrefabToSpawn;

        if (randomValue < weakEnemyChance)
        {
            enemyPrefabToSpawn = weakEnemyPrefab;
        }
        else if (randomValue < weakEnemyChance + mediumEnemyChance)
        {
            enemyPrefabToSpawn = mediumEnemyPrefab;
        }
        else
        {
            enemyPrefabToSpawn = strongEnemyPrefab;
        }

        // Spawn the enemy at a random position within the defined spawn area (using your random spawn logic)
        var spawnPosition = Utils.GetRandomPositionInBounds(spawnArea.bounds);
        Instantiate(enemyPrefabToSpawn, spawnPosition, enemyPrefabToSpawn.transform.rotation);

        // enemy.TakeDamage(1);
        _enemiesSpawned++; // Increment the counter for spawned enemies
    }

    public void EnemyDefeated()
    {
        ChangeMoney(30);
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
        PlayerPrefs.SetInt("level", ++_levelNumber);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SpawnUnits()
    {
        if (_money >= costOfSoldier)
        {
            Instantiate(soldierPrefab, new Vector3(-6f, 3f, 0), Quaternion.identity);
            ChangeMoney(-costOfSoldier);
        }
    }

    public void SpawnLauncher()
    {
        if (_numberOfLauncherSpawned < _launcherSpawnPositions.Length && _money >= costOfLauncher)
        {
            var position = _launcherSpawnPositions[_numberOfLauncherSpawned++];
            Instantiate(archerPrefab, new Vector3(position.X, position.Y), Quaternion.identity);
            ChangeMoney(-costOfLauncher);
        }
    }

    // Function to handle when an enemy reaches the castle
    public void EnemyReachedCastle(int damage)
    {
        castle.TakeDamage(damage);
        _enemiesHandled++; // Increment handled count when an enemy reaches the castle
        // Check if all enemies are handled
        if (_enemiesHandled >= totalEnemiesToSpawn)
        {
            TriggerWin();
        }
    }

    private void ChangeMoney(int amount)
    {
        _money += amount;
        moneyText.text = _money.ToString();
    }
}