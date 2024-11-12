using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFader : MonoBehaviour
{
    [SerializeField] Animator faderAnimator;

    private void OnEnable()
    {
        FieldEvents.SceneChanging += FadeDown;
        faderAnimator = GetComponent<Animator>();
    }

    private void OnDisable()
    {
        FieldEvents.SceneChanging -= FadeDown;

    }

    private void Start()
    {
        faderAnimator.ResetTrigger("Trigger2");
    }

    void FadeDown()
    {
        faderAnimator.SetTrigger("Trigger2");

    }
}
