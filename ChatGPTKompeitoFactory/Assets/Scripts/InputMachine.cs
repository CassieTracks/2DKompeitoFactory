using UnityEngine;

public class InputMachine : MonoBehaviour
{
    public GameObject kompeitoPrefab;
    public Color machineColor = Color.white;
    private bool isPlaced = false;
    private bool isSpawning = false;
    private GameObject spawnedKompeito = null;

    void Start()
    {
        GameManager.Instance.RegisterInputMachine(this);
    }

    public void PlaceMachine()
    {
        isPlaced = true;
    }

    public void StartSpawning()
    {
        if (isPlaced && GameManager.Instance.IsGameStarted())
        {
            spawnedKompeito = SpawnKompeito();
        }
    }

    private GameObject SpawnKompeito()
    {
        GameObject kompeito = Instantiate(kompeitoPrefab, transform.position, Quaternion.identity);
        KompeitoController kompeitoController = kompeito.AddComponent<KompeitoController>();
        kompeitoController.SetColor(machineColor);
        return kompeito;
    }

    public void SetColor(Color color)
    {
        machineColor = color;
    }

    public void ResetMachine()
    {
        // Optionally destroy the currently spawned Kompeito if it exists
        if (spawnedKompeito != null)
        {
            Destroy(spawnedKompeito);
            spawnedKompeito = null;
        }
        // Reset any other necessary state here
    }

    void OnDestroy()
    {
        GameManager.Instance.UnregisterInputMachine(this);
    }

}
