using UnityEngine;
using UnityEngine.Events;

public class AnimationFunctionTrigger : MonoBehaviour
{
    public GameObject menuGameObject;

    [SerializeField] private UnityEvent funcToTrigger;

    public void TriggerFunction()
    {
        if (funcToTrigger == null)
            Debug.Log("you forgot to assign a function to animator on " + this.gameObject);

        funcToTrigger.Invoke();
    }
}