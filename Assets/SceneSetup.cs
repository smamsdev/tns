using System.Collections;
using UnityEngine;

public class SceneSetup : MonoBehaviour
{
    public Transform transformToFollow;
    public GameObject mainCamera;
    public bool useEntryCoordinates;
    public Vector2 forceLook;
    public Vector3 forcedEntryCoordinates;
    public bool forceEntryCoorinates;
    public string sceneName;

    [SerializeField] Animator defaultFaderAnimator;
    public bool isCustomSceneStart;

    private void OnEnable()
    {
        FieldEvents.SceneChanging += FadeDown;

        if (sceneName == "")
        {
            Debug.Log("scene name is blank!");
        }

        FieldEvents.sceneName = sceneName;

        var playerGO = GameObject.Find("Player");

        if (FieldEvents.isReturningFromEncounter)
        { 
            useEntryCoordinates = true;
            forceEntryCoorinates = true;
            forcedEntryCoordinates = FieldEvents.coordinatesBeforeEncounter;
            forceLook = FieldEvents.lookDirBeforeEncounter;
            FieldEvents.isReturningFromEncounter = false;
        }

        if (forceLook != Vector2.zero)
        {
            var playerMovementScript = playerGO.GetComponent<PlayerMovementScript>();
            playerMovementScript.lookDirection = forceLook;
        }

        if (forceLook != Vector2.zero && !FieldEvents.isReturningFromEncounter)
        {
            var playerMovementScript = playerGO.GetComponent<PlayerMovementScript>();
            playerMovementScript.lookDirection = forceLook;
        }

        if (useEntryCoordinates)
        {
            if (forceEntryCoorinates)
            {
                FieldEvents.entryCoordinates = forcedEntryCoordinates;
            }

            playerGO.transform.position = FieldEvents.entryCoordinates;
        }
        mainCamera.transform.position = new Vector3(transformToFollow.position.x, transformToFollow.transform.position.y, transformToFollow.transform.position.z - 10);
    }

    private void OnDisable()
    {
        FieldEvents.SceneChanging -= FadeDown;
    }

    private void Start()
    {
        CombatEvents.LockPlayerMovement();

        if (!isCustomSceneStart)
        {
            StartCoroutine(DefaultSceneStart());
        }

        else
        { 
            FieldEvents.StartScene();
        }
    }

    IEnumerator DefaultSceneStart()
    {
        defaultFaderAnimator.SetBool("start", true);
        yield return new WaitForSeconds(0.5f);
        CombatEvents.UnlockPlayerMovement();

        StartScene();
    }

    public void FadeUp()
    {
        defaultFaderAnimator.SetBool("start", true);
    }

    public void StartScene()

    {
        FieldEvents.StartScene();
    }

    void FadeDown()
    {
        defaultFaderAnimator.SetTrigger("Trigger2");
    }
}