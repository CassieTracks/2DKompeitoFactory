using UnityEngine;

public class ExtractorMachine : MonoBehaviour
{
    public GameObject kompeitoPrefab; // Assign the Kompeito prefab
    private Color selectedColor = Color.white; // Default color
    public Color machineColor = Color.white; // Default color

    void Start()
    {
    }

    private void ChangeColor(Color newColor)
    {
        selectedColor = newColor;
        GetComponent<SpriteRenderer>().color = selectedColor; // Change the color of the Extractor Machine

        // Change the color of the Kompeito prefab
        if (kompeitoPrefab != null)
        {
            SpriteRenderer kompeitoSpriteRenderer = kompeitoPrefab.GetComponent<SpriteRenderer>();
            if (kompeitoSpriteRenderer != null)
            {
                kompeitoSpriteRenderer.color = selectedColor;
            }
        }
    }
}
