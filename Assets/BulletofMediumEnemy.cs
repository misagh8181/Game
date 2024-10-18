using UnityEngine;

public class BulletofMediumEnemy : MonoBehaviour
{
    public int damage = 15; // Damage the enemy deals to a castle
    public float speed = 2f; // Speed of movement towards the castle
    private Vector2 _castlePositionTarget;
    private GameManager _gameManager;
    private float _timeSinceEnteredCanvas;
    private UIEnemy _uiEnemy;
    private Animator _animator;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _animator = GetComponent<Animator>();
        _uiEnemy = GetComponent<UIEnemy>();

        // Get random castle position target
        _castlePositionTarget =
            Utils.GetRandomPositionInBounds(GameObject.FindWithTag("Castle").GetComponent<BoxCollider2D>().bounds);
    }

    private void Update()
    {
        MoveTowardsCastle();
    }


    private void MoveTowardsCastle()
    {
        // Move the enemy towards the castle's position
        if (speed > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, _castlePositionTarget, speed * Time.deltaTime);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only handle collision with the castle, ignore soldiers
        if (other.CompareTag("Castle"))
        {
            _gameManager.EnemyReachedCastle(damage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Soldier"))
        {
            _gameManager.EnemyReachedCastle(damage);
            Destroy(gameObject);
        }
    }
}