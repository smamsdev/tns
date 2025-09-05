using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

public class RandomEncounter : MonoBehaviour
{
    public PlayerMovementScript playerMovementScript;
    public float encounterValue;
    public string sceneToLoad;

    private float encounterCooldownTimer = 0f;
    private float encounterCooldownDuration = 5f;

    private void OnEnable()
    {
        playerMovementScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementScript>();
        encounterValue = 0;
    }

    private void Update()
    {
        if (!FieldEvents.movementLocked && playerMovementScript.delta.sqrMagnitude > 0.0001f)
        {
            if (encounterCooldownTimer > 0f)
            {
                encounterCooldownTimer -= Time.deltaTime;
            }
            else
            {
                CheckForEncounter();
                encounterCooldownTimer = encounterCooldownDuration;
            }
        }
    }

    void CheckForEncounter()
    {
        int encounterThreshold = 1;
        int rngValue = Random.Range(1, 30);

        if (encounterValue + rngValue >= encounterThreshold)
        {
            StartCoroutine(EncounterTransition());
            //LoadScene(sceneToLoad);
            //encounterValue = 0; 
        }
        else
        {
            encounterValue += (rngValue + playerMovementScript.distanceTravelled);
        }
    }

    void LoadScene(string sceneName)
    {
        FieldEvents.SceneChanging();
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator EncounterTransition()
    {
        GameObject mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        CombatEvents.LockPlayerMovement();

        FieldEvents.LerpValues(0, 359, 2, output =>
        {
            mainCam.transform.rotation = Quaternion.Euler(0, 0, output);
        });

        // Get *all* components and filter by type name
        var allComponents = mainCam.GetComponents<Component>();

        foreach (var c in allComponents)
        {
            if (c.GetType().Name.Contains("PixelPerfectCamera"))
            {
                Debug.Log($"Found PixelPerfectCamera: {c.GetType().FullName}");
            }
        }

        var ppc = mainCam.GetComponent<UnityEngine.U2D.PixelPerfectCamera>();

        FieldEvents.LerpValues(150, 0, 2, output =>
        {
            ppc.assetsPPU = Mathf.RoundToInt(output);
        });




        yield return null;
    }
}