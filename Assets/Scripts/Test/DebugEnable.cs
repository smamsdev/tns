using UnityEngine;

public class DebugEnable : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log($"GameObject {name} was enabled.", this);
        Debug.Log(System.Environment.StackTrace); // Prints the stack trace to help identify where it was called from.
    }
}
