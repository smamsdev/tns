using UnityEngine;

public class DebugDisable : MonoBehaviour
{
    private void OnDisable()
    {
        Debug.Log($"GameObject {name} was disabled.", this);
        Debug.Log(System.Environment.StackTrace); // Prints the stack trace to help identify where it was called from.
    }
}