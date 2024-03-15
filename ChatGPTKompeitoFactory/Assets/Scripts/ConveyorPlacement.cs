using System.Collections.Generic;
using UnityEngine;

public class ConveyorPlacement : MonoBehaviour
{
    // Prefabs for conveyor belts and machines
    public GameObject conveyorStraightPrefab;
    public GameObject conveyorRightTurnPrefab;
    public GameObject conveyorLeftTurnPrefab;
    public GameObject mergeRightPrefab;
    public GameObject mergeLeftPrefab;
    public GameObject splitRightPrefab;
    public GameObject splitLeftPrefab;
    public GameObject inputMachinePrefab;
    public GameObject fusionMachinePrefab;
    public GameObject fissionMachinePrefab;
    public GameObject extractorMachinePrefab;
    public GameObject mixingMachinePrefab;

    // Private variables for internal logic
    private GameObject currentConveyorPrefab;
    private GameObject currentConveyorInstance;
    private float gridSize = 1f;
    private Color selectedInputMachineColor = Color.white;
    private Color selectedFissionMachineColor = Color.white;
    private Color selectedExtractorMachineColor = Color.white;
    private Color selectedMixingMachineColor = Color.white; // Default color for Mixing Machine
    private List<GameObject> placedObjects = new List<GameObject>();


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && currentConveyorInstance != null)
        {
            PlaceConveyorOrMachine();
        }

        if (currentConveyorInstance != null)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPosition = SnapToGrid(worldPosition);
            currentConveyorInstance.transform.position = worldPosition;

            float rotationAmount = 0f;
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.R))
            {
                rotationAmount = 90f;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                rotationAmount = 90f;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                rotationAmount = -90f;
            }

            if (rotationAmount != 0f)
            {
                currentConveyorInstance.transform.Rotate(0, 0, rotationAmount);
            }
        }
    }

    // Methods for setting different conveyor belts and machines
    public void SetStraightConveyor()
    {
        SetPrefab(conveyorStraightPrefab);
    }

    public void SetRightTurnConveyor()
    {
        SetPrefab(conveyorRightTurnPrefab);
    }

    public void SetLeftTurnConveyor()
    {
        SetPrefab(conveyorLeftTurnPrefab);
    }

    public void SetMergeRight()
    {
        SetPrefab(mergeRightPrefab);
    }

    public void SetMergeLeft()
    {
        SetPrefab(mergeLeftPrefab);
    }

    public void SetSplitRight()
    {
        SetPrefab(splitRightPrefab);
    }

    public void SetSplitLeft()
    {
        SetPrefab(splitLeftPrefab);
    }

    public void SetInputMachine()
    {
        SetPrefab(inputMachinePrefab);
    }

    public void SetFusionMachine()
    {
        SetPrefab(fusionMachinePrefab);
    }

    public void SetFissionMachine(Color color)
    {
        selectedFissionMachineColor = color;
        SetPrefab(fissionMachinePrefab);
    }

    public void SetExtractorMachine(Color color)
    {
        selectedExtractorMachineColor = color;
        SetPrefab(extractorMachinePrefab);
    }

    // Helper method for setting the current prefab and creating a preview instance
    private void SetPrefab(GameObject prefab)
    {
        if (currentConveyorInstance != null )
        {
            Destroy(currentConveyorInstance);
        }
        currentConveyorPrefab = prefab;
        CreateConveyorPreview();
    }

    private void CreateConveyorPreview()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition = SnapToGrid(worldPosition);
        currentConveyorInstance = Instantiate(currentConveyorPrefab, worldPosition, Quaternion.identity);

        // Set color for Input Machine
        if (currentConveyorPrefab == inputMachinePrefab)
        {
            InputMachine inputMachine = currentConveyorInstance.GetComponent<InputMachine>();
            if (inputMachine != null)
            {
                inputMachine.machineColor = selectedInputMachineColor;
                Debug.Log("SeletedINputMachineColor:" + selectedInputMachineColor);
                inputMachine.GetComponent<SpriteRenderer>().color = selectedInputMachineColor;
            }
        }
        // Set color for Fission Machine
        else if (currentConveyorPrefab == fissionMachinePrefab)
        {
            currentConveyorInstance.GetComponent<SpriteRenderer>().color = selectedFissionMachineColor;
        }
        // Set color for Extractor Machine
        else if (currentConveyorPrefab == extractorMachinePrefab)
        {
            currentConveyorInstance.GetComponent<SpriteRenderer>().color = selectedExtractorMachineColor;
        }
        // Set color for Mixing Machine
        else if (currentConveyorPrefab == mixingMachinePrefab)
        {
            currentConveyorInstance.GetComponent<SpriteRenderer>().color = selectedMixingMachineColor;
        }
    }


    private Vector3 SnapToGrid(Vector3 position)
    {
        position.x = Mathf.Round(position.x / gridSize) * gridSize;
        position.y = Mathf.Round(position.y / gridSize) * gridSize;
        position.z = 0;
        return position;
    }

    private void PlaceConveyorOrMachine()
    {
        if (CanPlaceObject(currentConveyorInstance.transform.position))
        {
            GameObject placedObject = Instantiate(currentConveyorPrefab, currentConveyorInstance.transform.position, currentConveyorInstance.transform.rotation);
            placedObjects.Add(placedObject);
            if (currentConveyorPrefab == inputMachinePrefab)
            {
                InputMachine inputMachine = placedObject.GetComponent<InputMachine>();
                if (inputMachine != null)
                {
                    inputMachine.machineColor = selectedInputMachineColor;
                    inputMachine.GetComponent<SpriteRenderer>().color = selectedInputMachineColor;
                    placedObject.GetComponent<InputMachine>().PlaceMachine();
                }
            }
            else if (currentConveyorPrefab == fissionMachinePrefab)
            {
                placedObject.GetComponent<SpriteRenderer>().color = selectedFissionMachineColor;
            }
            else if (currentConveyorPrefab == extractorMachinePrefab)
            {
                placedObject.GetComponent<SpriteRenderer>().color = selectedExtractorMachineColor;
            }
            else if (currentConveyorPrefab == mixingMachinePrefab)
            {
                placedObject.GetComponent<SpriteRenderer>().color = selectedMixingMachineColor;

            }

            Destroy(currentConveyorInstance);
            currentConveyorPrefab = null;
        }
        else
        {

            Debug.Log("Cannot place object here!");
        }
    }

    private bool ObjectIsSelected()
    {
        ConveyorObjectInteraction[] allObjects = FindObjectsOfType<ConveyorObjectInteraction>();
        foreach (ConveyorObjectInteraction obj in allObjects)
        {
            if (obj.IsSelected())
            {
                return true;
            }
        }
        return false;
    }

    private bool CanPlaceObject(Vector3 position)
    {
        // Define the layer mask to only include the PlacementLayer
        // Replace "PlacementLayer" with the name of your layer used for placement
        int layerMask = LayerMask.GetMask("Machine");

        // Use the layerMask as a parameter in OverlapCircleAll
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.1f, layerMask);

        foreach (Collider2D collider in colliders)
        {
            // Now, this loop will only consider colliders on the PlacementLayer
            if (collider.gameObject != currentConveyorInstance)
            {
                return false; // Found another object in the way that is not the current moving instance
            }
        }
        return true; // No other objects found on the PlacementLayer at this position
    }

    public void SetInputMachineRed()
    {
        selectedInputMachineColor = new Color(1, 0, 0, 1);
        SetInputMachine();
    }

    public void SetInputMachineBlue()
    {
        selectedInputMachineColor = new Color(0, 0, 1, 1);
        SetInputMachine();
    }

    public void SetInputMachineGreen()
    {
        selectedInputMachineColor = new Color(0, 1, 0, 1);
        SetInputMachine();
    }

    // Fission Machine Color Buttons
    public void SetFissionMachineCyan()
    {
        selectedFissionMachineColor = Color.cyan;
        SetFissionMachine();
    }

    public void SetFissionMachineMagenta()
    {
        selectedFissionMachineColor = Color.magenta;
        SetFissionMachine();
    }

    public void SetFissionMachineYellow()
    {
        selectedFissionMachineColor = Color.yellow;
        SetFissionMachine();
    }

    // Extractor Machine Color Buttons
    public void SetExtractorMachineCyan()
    {
        selectedExtractorMachineColor = Color.cyan;
        SetExtractorMachine();
    }

    public void SetExtractorMachineMagenta()
    {
        selectedExtractorMachineColor = Color.magenta;
        SetExtractorMachine();
    }

    public void SetExtractorMachineYellow()
    {
        selectedExtractorMachineColor = Color.yellow;
        SetExtractorMachine();
    }

    private void SetFissionMachine()
    {
        if (currentConveyorInstance != null || ObjectIsSelected())
        {
            Destroy(currentConveyorInstance);
        }
        currentConveyorPrefab = fissionMachinePrefab;
        CreateConveyorPreview();
    }

    private void SetExtractorMachine()
    {
        if (currentConveyorInstance != null || ObjectIsSelected())
        {
            Destroy(currentConveyorInstance);
        }
        currentConveyorPrefab = extractorMachinePrefab;
        CreateConveyorPreview();
    }


    // Mixing Machine Color Buttons
    public void SetMixingMachineRed()
    {
        selectedMixingMachineColor = new Color(1, 0, 0, 1);
        SetMixingMachine();
    }

    public void SetMixingMachineBlue()
    {
        selectedMixingMachineColor = new Color(0, 0, 1, 1);
        SetMixingMachine();
    }

    public void SetMixingMachineGreen()
    {
        selectedMixingMachineColor = new Color(0, 1, 0, 1);
        SetMixingMachine();
    }

    private void SetMixingMachine()
    {
        if (currentConveyorInstance != null || ObjectIsSelected())
        {
            Destroy(currentConveyorInstance);
        }
        currentConveyorPrefab = mixingMachinePrefab;
        CreateConveyorPreview();
    }
    public void ResetPlacedObjects()
    {
        foreach (GameObject placedObject in placedObjects)
        {
            Destroy(placedObject);
        }
        placedObjects.Clear(); // Clear the list after destroying all objects
    }
}
