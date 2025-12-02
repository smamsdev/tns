using System.Collections;
using UnityEngine;

public class SceneSetup : MonoBehaviour
{
    public CustomSceneStart customSceneStart;
    CameraFollow cameraFollow;
    Animator fader;
    GameObject playerGO;
    public bool isTriggerOnLoad;
    public ToTrigger triggerOnLoad;

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

        StartSceneTrigger();
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
        CombatEvents.UnlockPlayerMovement();
        yield return null;
    }

    private void StartSceneTrigger()
    {
        if (isTriggerOnLoad)
        {
            StartCoroutine(triggerOnLoad.Triggered());
        }
    }


#if UNITY_EDITOR
    //can be really annoying to debug if you accidentally have 2 
    void OnValidate()
    {
        var all = GameObject.FindObjectsByType<SceneSetup>(UnityEngine.FindObjectsInactive.Include,UnityEngine.FindObjectsSortMode.None);

        if (all.Length > 1)
        {
            Debug.LogError($"Only ONE {nameof(SceneSetup)} is allowed in this scene!", this);
        }
    }
#endif
}
