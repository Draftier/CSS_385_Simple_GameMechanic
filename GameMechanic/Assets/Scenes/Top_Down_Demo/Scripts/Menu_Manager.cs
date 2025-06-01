using UnityEngine;
using UnityEngine.SceneManagement;

// Class that manages game menus (only retry in this game)
public class Menu_Manager : MonoBehaviour
{
    // Menu object
    public GameObject menu;

    void Start()
    {
        // Ensure menu not shown when game starts
        menu.SetActive(false);
    }

    // Function for retry button
    public void Retry()
    {
        // Reset time scale
        Time.timeScale = 1f;

        // Reset player hit and enemy counts
        PlayerMovement.hitCount = 0;
        Enemy.enemyCount = 0;

        // Reload he scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Function to show the death or win menu
    public void DeathOrWinMenu()
    {
        if (menu != null)
        { 
            menu.SetActive(true);
        }
        Time.timeScale = 0f; // Pause the game
    }
}
