using UnityEditor;
using UnityEngine;

// Class that controls enemy behavior, allows enemy to track, follow, and keep distance
// From target, and fire projectiles at the target.
public class Enemy : MonoBehaviour
{
    // Transform enemy tracks and follows (currently player)
    public Transform player;

    // Prefab for the projectile enemy fires
    public GameObject projectilePrefab;

    // Force applied to the projectile when fired
    public float fireForce = 10f;
    // Rate at which the enemy fires projectiles
    public float fireRate = 1f;

    // Desired distance enemy tries to maintain from the transform
    public float desiredDistance = 3f;

    // Speed at which the enemy moves towards or away from the transform
    public float moveSpeed = 2f;

    // Cooldown timer for firing projectiles
    private float fireCooldown = 0f;

    // Static variable to track number of enemies in the scene
    public static int enemyCount = 0;

    // Reference to the Menu_Manager to handle game state (retry)
    Menu_Manager Menu_Obj;

    void Awake()
    {
        // Find retry menu
        Menu_Obj = Object.FindFirstObjectByType<Menu_Manager>();
        // Increment enemy count when enemy is created
        enemyCount++;
    }

    void OnDestroy()
    {
        // Decrement enemy count when enemy is destroyed
        enemyCount--;

        // If no enemies left, show death or win menu
        if (enemyCount <= 0)
        {
            Menu_Manager menuObj = Object.FindFirstObjectByType<Menu_Manager>();
            if (menuObj != null)
            {
                menuObj.DeathOrWinMenu();
            }

        }
    }

    // Update is called once per frame
    void Update()
    {

        // Find angle to transform and rotate towards them
        Vector2 direction = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Calculate distance to transform
        float distance = Vector2.Distance(transform.position, player.position);

        // Adjust position based on distance to maintain desired distance
        if (distance < desiredDistance - 0.1f)
        {
            // If transform is too close, move away from transform
            transform.position -= (Vector3)direction * moveSpeed * Time.deltaTime;
        }
        else if (distance > desiredDistance + 0.1f)
        {
            // If transform is too far, move towards transform
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
        }

        // Fire projectile towards the transform at a set rate
        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f)
        {
            Fire(direction);
            fireCooldown = 1f / fireRate;
        }
    }

    // Method to fire a projectile
    void Fire(Vector2 direction)
    {
        // Calculate spawn position for the projectile
        float spawnDistance = 0.6f;
        Vector2 spawnPos = (Vector2)transform.position + direction * spawnDistance;

        // Instantiate the projectile and set its velocity
        GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();

        rb.linearVelocity = direction * fireForce;
    }

    // Checks if enemy collides with the player's weapon
    void OnTriggerEnter2D(Collider2D other)
    {
        // If enemy collides with the player, destroy the enemy
        if (other.CompareTag("PlayerWeapon"))
        {
            Destroy(gameObject);
        }
    }
}
