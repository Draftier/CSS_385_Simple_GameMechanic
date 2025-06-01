using TMPro;
using UnityEngine;

// Class used to control the projectile behavior in game
// Projectile is destroyed when it collides with the player or player's weapon
// Player sprite opacity decreases if hit by projectile
public class Projectile : MonoBehaviour
{
    // Controls collisions of projectile with player or player's weapon
    void OnTriggerEnter2D(Collider2D other)
    {   
        // Check if the projectile collides with the player or player's weapon
        if (other.CompareTag("Player"))
        {
            // If projectile hits the player, destroy the projectile
            // Increment hit count and change player color to indicate hit

            // Increment hit count and destroy projectile
            Destroy(gameObject);
            PlayerMovement.hitCount++; 

            // Change player sprite alpha
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            Color temp = playerMovement.spriteRenderer.color;
            temp.a -= 0.2f;
            playerMovement.spriteRenderer.color = temp;
        }
        else if (other.CompareTag("PlayerWeapon"))
        {
            // If projectile hits the player's weapon, destroy the projectile
            Destroy(gameObject);
        }
    }

    // Didn't know this existed till now, but this is called when the projectile goes off screen
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
