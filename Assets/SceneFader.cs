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
        CombatEvents.LockPlayerMovement();

        if (!remainFadedDownOnSceneLoad)
        {
            faderAnimator.SetBool("start", true); //end of animation contains an event to invoke UnlockMovement(), this will probably backfire at some point
        }
    }

    void FadeDown()
    {
        faderAnimator.SetTrigger("Trigger2");
    }

    public void LockMovement()

    {
        CombatEvents.LockPlayerMovement();
    }

    public void StartScene() //triggered via fadein animation event

    { 
        CombatEvents.UnlockPlayerMovement();
        FieldEvents.StartScene();
    }
}
