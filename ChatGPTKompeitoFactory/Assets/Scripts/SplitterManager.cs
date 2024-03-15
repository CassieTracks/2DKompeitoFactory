using UnityEngine;

public class SplitterManager : MonoBehaviour
{
    public static int splitCounter = 0; // Static counter shared across all instances

    public static void IncrementSplitCounter()
    {
        splitCounter++;
    }

    public static int GetSplitCounter()
    {
        return splitCounter;
    }

    // Optionally, reset the counter under certain conditions if needed
    public static void ResetSplitCounter()
    {
        splitCounter = 0;
    }
}
