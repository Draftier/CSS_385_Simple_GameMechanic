using UnityEngine;
using UnityEngine.UI; // Needed for UI

// Class that controls player movement, slow motion, and hit count
// PlayerMovement script allows player to move around the scene and use slow motion
public class PlayerMovement : MonoBehaviour
{
    // Player movement speed
    public float moveSpeed = 5f;

    // Slowmotion parameters

    // Maximum slow motion meter, recharge rate, and deplete rate
    // Number of seconds the player can use slow motion
    public float slowMoTime = 3f;

    // Rate at which the slow motion meter recharges and depletes in seconds
    public float slowMoRechargeRate = 1f;
    public float slowMoDepleteRate = 1f;
    // UI element to display the slow motion meter
    public Image slowMoBar;

    // SpriteRenderer to change player sprite color when hit
    public SpriteRenderer spriteRenderer;

    // Static variable to track the number of hits the player has taken
    public static int hitCount = 0;

    // Track the value of slow motion meter and whether slow motion is active
    // Slow motion meter is utilized for bar fill
    private float slowMoMeter;
    private bool isSlowing = false;

    // Rigidbody2D component for physics-based movement
    private Rigidbody2D rb;
    // Vector2 to store player movement input
    private Vector2 movement;

    // Reference to the Menu_Manager to handle game state (death or win menu)
    Menu_Manager Menu_Obj;



    void Awake()
    {
        // Set hitcount to 0 and find the Menu_Manager object
        hitCount = 0;
        Menu_Obj = Object.FindAnyObjectByType<Menu_Manager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize time scale
        Time.timeScale = 1f;

        // Initialize the Rigidbody2D component and slow motion meter
        rb = GetComponent<Rigidbody2D>();
        slowMoMeter = slowMoTime;

        // Set slow motion bar fill amount to 1 (full)
        if (slowMoBar != null)
        {
            slowMoBar.fillAmount = 1f;
        }

        // Get the SpriteRenderer component to change player sprite color when hit
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get input from WASD or Arrow keys
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Normalize to ensure consistent speed in diagonal movement
        movement = movement.normalized;

        // SlowMotion function called to handle slow motion input and meter
        SlowMotion();

        // Check if the player has been hit 5 times
        if (PlayerMovement.hitCount >= 5)
        {
            // Show retry screen
            Menu_Obj.DeathOrWinMenu();
        }
    }

    // FixedUpdate is called at a fixed interval and is used for physics updates
    void FixedUpdate()
    {
        // Move the player
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    // Function to handle slow motion
    void SlowMotion()
    {
        // If meter is not empty and left shift is pressed, activate slow motion
        if (Input.GetKey(KeyCode.LeftShift) && slowMoMeter > 0f)
        {
            // Check if not already slowing down
            if (!isSlowing)
            {
                // Set time scale and fixed delta time for slow motion
                Time.timeScale = 0.3f;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;

                // Set whether or not the player is slowing
                isSlowing = true;
            }

            // Deplete the time of slow motion
            slowMoMeter -= slowMoDepleteRate * Time.unscaledDeltaTime;
            slowMoMeter = Mathf.Max(slowMoMeter, 0f);

            // If slow motion time out exit slowmotion
            if(slowMoMeter <= 0f)
            {
                // Reset time scale and fixed delta time to normal
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02f;
                isSlowing = false;
            }
        }
        else
        {
            // If not pressing left shift and meter is not full
            if (isSlowing)
            {
                // Reset time scale and fixed delta time to normal
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02f;
                isSlowing = false;
            }

            // Recharge the slow motion time
            slowMoMeter += slowMoRechargeRate * Time.unscaledDeltaTime;
            slowMoMeter = Mathf.Min(slowMoMeter, slowMoTime);
        }

        // Update the slow motion bar fill amount
        if (slowMoBar != null)
        {
            slowMoBar.fillAmount = slowMoMeter / slowMoTime;
        }
    }
}
