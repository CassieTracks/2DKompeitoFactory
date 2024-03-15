using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject startButton;
    public GameObject stopButton;
    public ConveyorPlacement conveyorPlacement;

    private List<InputMachine> inputMachines = new List<InputMachine>();
    private bool isGameStarted = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        stopButton.SetActive(false);
    }

    public void RegisterInputMachine(InputMachine machine)
    {
        if (!inputMachines.Contains(machine))
        {
            inputMachines.Add(machine);
        }
    }

    public void StartGame()
    {
        isGameStarted = true;
        startButton.SetActive(false);
        stopButton.SetActive(true);
        foreach (InputMachine machine in inputMachines)
        {
            machine.StartSpawning();
        }
    }

    public void StopGame()
    {
        isGameStarted = false;
        stopButton.SetActive(false);
        startButton.SetActive(true);
        ClearKompeitos(); // Clear all kompeitos
        foreach (InputMachine machine in inputMachines)
        {
            machine.ResetMachine(); // Reset each machine for a new game
        }
    }

    public void ResetGame()
    {
        StopGame(); // Reuse the stop functionality if necessary
        inputMachines.Clear(); // Clear all machines and conveyors if using a list to track them

        // Find and destroy all objects on the "GameDynamicLayer". Replace layerNumber with your specific layer's number.
        int layerNumber = LayerMask.NameToLayer("Machine");
        GameObject[] allObjects = FindObjectsOfType<GameObject>(); // Finds all active and inactive objects in the scene
        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == layerNumber)
            {
                Destroy(obj);
            }
        }
    }




    private void ClearKompeitos()
    {
        KompeitoController[] allKompeitos = FindObjectsOfType<KompeitoController>();
        foreach (KompeitoController kompeito in allKompeitos)
        {
            Destroy(kompeito.gameObject);
        }
    }

    public bool IsGameStarted()
    {
        return isGameStarted;
    }

    public void UnregisterInputMachine(InputMachine machine)
    {
        if (inputMachines.Contains(machine))
        {
            inputMachines.Remove(machine);
        }
    }
}
