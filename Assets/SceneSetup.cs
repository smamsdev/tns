using System.Collections;
using UnityEngine;

public class SceneSetup : MonoBehaviour
{
    public CustomSceneStart customSceneStart;
    CameraFollow cameraFollow;
    Animator fader;
    GameObject playerGO;

    private void OnDisable()
    {
        CrossSceneReferences.Clear();
    }

    private void Start()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player");
        cameraFollow = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        fader = GameObject.FindGameObjectWithTag("Fader").GetComponent<Animator>();

        if (cameraFollow.transformToFollow == null)
        {
            cameraFollow.transformToFollow = playerGO.transform;
        }

        CheckForEntryCoords();
        if (customSceneStart != null)
        {
            customSceneStart.StartScene();
        }

        else
        {
            StartCoroutine(DefaultSceneStart());
        }
    }

    void CheckForEntryCoords()
    {
        if (FieldEvents.fromEntryPoint != false)
        {
            playerGO.transform.position = FieldEvents.positionOnEntry;
            FieldEvents.fromEntryPoint = false;
            return;
        }
    }

    IEnumerator DefaultSceneStart()
    {
        CombatEvents.LockPlayerMovement();
        cameraFollow.transform.position = new(
            cameraFollow.transformToFollow.position.x + cameraFollow.xOffset, 
            cameraFollow.transformToFollow.position.y + cameraFollow.yOffset, 
            -10f);

        fader.Play("Dissolve");
        yield return new WaitForSeconds(0.2f);
        CombatEvents.UnlockPlayerMovement();
    }

    //can be really annoying to debug if you accidentally have 2 
    void OnValidate()
    {
#if UNITY_EDITOR
        var all = FindObjectsOfType<SceneSetup>(true);

        if (all.Length > 1)
        {
            Debug.LogError(
                $"Only ONE {nameof(SceneSetup)} is allowed in this scene!",
                this
            );
        }
#endif
    }
}
