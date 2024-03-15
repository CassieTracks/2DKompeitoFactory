using UnityEngine;

public class ConveyorObjectInteraction : MonoBehaviour
{
    private bool isSelected = false;
    private float gridSize = 1f;
    private SpriteRenderer spriteRenderer;
    private Color originalColor; // Variable to store the original color

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color; // Store the original color

    }

    void Update()
    {
        if (isSelected)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                transform.Rotate(0, 0, 90f);
            }

            if (Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Backspace))
            {
                Destroy(gameObject);
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                worldPosition = SnapToGrid(worldPosition);
                transform.position = worldPosition;
                spriteRenderer.color = Color.gray; // Or any other color you want to use for selection

            }
            else if (Input.GetMouseButtonUp(0))
            {
                isSelected = false; // Deselect the object when the mouse button is released
                spriteRenderer.color = originalColor;
            }
        }


    }

    void OnMouseDown()
    {
        Debug.Log("Input machine clicked");
        isSelected = true;
        
    }

    private Vector3 SnapToGrid(Vector3 position)
    {
        position.x = Mathf.Round(position.x / gridSize) * gridSize;
        position.y = Mathf.Round(position.y / gridSize) * gridSize;
        position.z = 0;
        return position;
    }

    public bool IsSelected()
    {
        return isSelected;
    }
    
}
