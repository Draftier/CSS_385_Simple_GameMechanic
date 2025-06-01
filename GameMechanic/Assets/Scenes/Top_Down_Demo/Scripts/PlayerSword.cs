using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

// This script controls the player sword, allowing it to swing around the player based on mouse position and
// Whether or not player clicked mouse. Sword stops moving afte player is "dead" or "win" condition is met.
public class PlayerSword : MonoBehaviour
{
    // Player transform to reset sword position
    public Transform player;

    // Swing parameters like angle and duration
    public float swingAngle = 120f;
    public float swingDuration = 0.2f;

    // State of swings utilized in swing logic
    private bool isSwinging = false;
    private float swingTimer = 0f;

    // Used to calculate angles for swinging
    private float startAngle;
    private float endAngle;
    private float centerAngle;
    public int test;

    // Update is called once per frame
    void Update()
    {
        // Get angle from player to mouse in world space (Makes sword move with mouse)
        // Only do this if player has not been hit 5 times or there are still enemies left
        test = Enemy.enemyCount; // Example of accessing static variable
        if (PlayerMovement.hitCount < 5)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 toMouse = mouseWorld - player.position;
            centerAngle = Mathf.Atan2(toMouse.y, toMouse.x) * Mathf.Rad2Deg;

            // Change swing state when mouse button is pressed
            // If the player clicks the mouse button and is not already swinging
            if (Input.GetMouseButtonDown(0) && !isSwinging)
            {
                isSwinging = true;
                swingTimer = 0f;
                startAngle = centerAngle - swingAngle / 2f;
                endAngle = centerAngle + swingAngle / 2f;
            }

            // If player is swinging then update the swing timer and calculate the angle
            // Rotate the sword around the player based on the swing angle
            if (isSwinging)
            {
                // Update swing timer and calculate the angle based on the swing duration
                swingTimer += Time.deltaTime;
                // Calculate the normalized time (0 to 1) for the swing
                float t = swingTimer / swingDuration;
                // Clamp t to ensure it doesn't exceed 1
                float angle = Mathf.Lerp(startAngle, endAngle, t);

                // Rotate around the player, facing the swing angle
                transform.position = player.position + Quaternion.Euler(0, 0, angle) * Vector3.right * 1f;
                transform.rotation = Quaternion.Euler(0, 0, angle);

                // If the swing is complete, stop swinging
                if (t >= 1f)
                {
                    isSwinging = false;
                }
            }
            else
            {
                // Tie sword to initial position when not swinging
                float restAngle = centerAngle - swingAngle / 2f;
                transform.position = player.position + Quaternion.Euler(0, 0, restAngle) * Vector3.right * 1f;
                transform.rotation = Quaternion.Euler(0, 0, restAngle);
            }
        }

    }
}
