using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFader : MonoBehaviour
{
    [SerializeField] Animator faderAnimator;
    public bool remainFadedDownOnSceneLoad;

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
        if (!remainFadedDownOnSceneLoad)

        {
            faderAnimator.SetBool("start", true);
        }
    }

    void FadeDown()
    {
        faderAnimator.SetTrigger("Trigger2");
    }
}
