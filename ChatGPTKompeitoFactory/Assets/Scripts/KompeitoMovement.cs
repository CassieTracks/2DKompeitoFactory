using System.Collections;
using UnityEngine;

public class KompeitoController : MonoBehaviour
{
    public float speed = 1f;
    private bool isActive = true;
    //public GameObject smallKompeitoPrefab;
    //public GameObject largeKompeitoPrefab;
    public float interactionDelay = 0.5f; // Delay in seconds
    private float lastInteractionTime = -1f;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Vector2 movementDirection = Vector2.right;
    private int triggerCount = 0; // Track the number of triggers we're currently overlapping
    private bool hasEntered = false;
    public MovementState currentMovementState = MovementState.Straight;
    private int splitCounter = 0; // To handle alternate behavior for splits
    public enum MovementState
    {
        Straight,
        GoRight,
        GoLeft,
        SplitRight,
        SplitLeft
    }
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        // Assuming initial movement to the right; adjust as needed
    }

    public void SetColor(Color color)
    {
        SpriteRenderer kompeitoSpriteRenderer = GetComponent<SpriteRenderer>();
        if (kompeitoSpriteRenderer != null)
        {
            kompeitoSpriteRenderer.color = color; // Apply the color
            Debug.Log("Kompeito Color Set: " + color);
        }
    }


    void Update()
    {
        if (isActive)
        {
            if (triggerCount > 0) // Only move if inside a trigger area
            {
                rb.velocity = movementDirection * speed;
            }
            else
            {
                rb.velocity = Vector2.zero; // Stop moving if not inside a trigger
            }

        }


    
    }

    void OnTriggerEnter2D(Collider2D collider)
    {


        Debug.Log(gameObject.name + " Collided with: " + collider.gameObject.name + ", Tag: " + collider.gameObject.tag);

        switch (collider.tag)
        {
            case "EnterGoRight":
                currentMovementState = MovementState.GoRight;
                hasEntered = true;
                break;
            case "EnterGoLeft":
                currentMovementState = MovementState.GoLeft;
                hasEntered = true;
                break;
            case "EnterGoStraight":
                currentMovementState = MovementState.Straight;
                hasEntered = true;
                break;
            case "EnterSplitRight":
                // Use the SplitterManager's static method to get the current counter
                bool isEven = SplitterManager.GetSplitCounter() % 2 == 0;
                currentMovementState = isEven ? MovementState.Straight : MovementState.GoRight;
                // Increment the counter
                SplitterManager.IncrementSplitCounter();
                hasEntered = true;
                break;

            case "EnterSplitLeft":
                // Similar logic for EnterSplitLeft
                isEven = SplitterManager.GetSplitCounter() % 2 == 0;
                currentMovementState = isEven ? MovementState.Straight : MovementState.GoLeft;
                SplitterManager.IncrementSplitCounter();
                hasEntered = true;
                break;
            case "DecisionGate":
                ApplyDecisionGateAction();
                break;
                // Implement other cases as needed
        }






         if (collider.CompareTag("Exit") && hasEntered)
        {
            // The Kompeito has previously hit an "Enter" object and can now go through the "Exit"
            // Implement the logic for going through the "Exit" here (e.g., moving to the next area)
            Debug.Log("Kompeito has exited successfully.");

            // Optionally reset hasEntered if you want the Kompeito to hit another "Enter" before going through another "Exit"
            hasEntered = false;
        }else if (collider.CompareTag("Exit"))
        {
            rb.velocity = Vector2.zero; // Stop moving immediately
            isActive = false; // Optionally, disable further movement updates

        }

        if (collider.CompareTag("Stopper"))
        {
            Debug.Log("stop!");
            rb.velocity = Vector2.zero; // Stop moving immediately
            isActive = false; // Optionally, disable further movement updates
        }
        if (collider.gameObject.CompareTag("Mixer"))
            {
                isActive = true;
                Color mixerColor = collider.gameObject.GetComponent<SpriteRenderer>().color;
                Color kompeitoColor = spriteRenderer.color;

                Debug.Log(mixerColor + " " + kompeitoColor);

                float r = (mixerColor.r + kompeitoColor.r) / 2;
                float g = (mixerColor.g + kompeitoColor.g) / 2;
                float b = (mixerColor.b + kompeitoColor.b) / 2;
                float a = (mixerColor.a + kompeitoColor.a) / 2;

                float maxColorValue = Mathf.Max(r, g, b);
                float percentValue = 1;

                if (maxColorValue < 1)
                {
                    percentValue = 1 / (1 - maxColorValue);

                }



                Debug.Log(r + " " + g + " " + b);
                Debug.Log(r * percentValue + " " + g * percentValue + " " + b * percentValue);

                SetColor(new Color(r * percentValue, g * percentValue, b * percentValue, 1)); // Assuming alpha is always 1
            }
            else if (collider.gameObject.CompareTag("Fusion"))
            {
                //lastInteractionTime = currentTime;
            }
            else if (collider.gameObject.CompareTag("Fission"))
            {
                // Split into two smaller Kompeitos
                //Instantiate(smallKompeitoPrefab, transform.position, Quaternion.identity);
                //Instantiate(smallKompeitoPrefab, transform.position, Quaternion.identity);
                //lastInteractionTime = currentTime;
                //Destroy(gameObject); // Destroy the original Kompeito
            }
            else if (collider.gameObject.CompareTag("Extractor"))
            {
                // Modify color based on Extractor's color
                Color extractorColor = collider.gameObject.GetComponent<SpriteRenderer>().color;
                //SetColor(Color.Lerp(spriteRenderer.color, extractorColor, 0.5f));
                //lastInteractionTime = currentTime;
            }
            if (collider.CompareTag("RightConveyor"))
            {
                //transform.Rotate(0, 90, 0);
                //isActive = true;

                //RotateMovementDirection(-90);
            }
        if (collider.CompareTag("LeftConveyor"))
        {
            //transform.Rotate(0, 90, 0);
            //isActive = true;

            //RotateMovementDirection(90);
        }
        
        triggerCount++; // Increment counter when entering a trigger area
        
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        triggerCount--; // Decrement counter when exiting a trigger area
        triggerCount = Mathf.Max(0, triggerCount); // Ensure counter doesn't go below 0

    }
    private void RotateMovementDirection(float angleInDegrees)
    {
        // Convert the angle to radians, since Mathf.Cos and Mathf.Sin use radians
        float angleInRadians = angleInDegrees * Mathf.Deg2Rad;
        // Rotate the direction
        float cosAngle = Mathf.Cos(angleInRadians);
        float sinAngle = Mathf.Sin(angleInRadians);
        Vector2 newDirection = new Vector2(
            cosAngle * movementDirection.x - sinAngle * movementDirection.y,
            sinAngle * movementDirection.x + cosAngle * movementDirection.y
        );
        movementDirection = newDirection.normalized; // Ensure the direction vector is normalized
    }
    private void ApplyDecisionGateAction()
    {
        // Execute the action based on the current movement state
        Debug.Log(gameObject.name + " applyDecisionGateAction: " + currentMovementState);
        switch (currentMovementState)
        {
            case MovementState.GoRight:
                RotateMovementDirection(-90);
                break;
            case MovementState.GoLeft:
                RotateMovementDirection(90);
                break;
            case MovementState.Straight:
                // No rotation needed, continue straight
                break;
                // Handle split cases if needed
        }

        // Reset the movement state if necessary
        // currentMovementState = MovementState.Straight; // Uncomment if resetting state after each decisionGate
    }
}
