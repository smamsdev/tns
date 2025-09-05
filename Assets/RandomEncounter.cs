using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;

public class RandomEncounter : MonoBehaviour
{
    PlayerMovementScript playerMovementScript;
    public float encounterValue;
    public string sceneToLoad;

    private float encounterCooldownTimer = 0f;
    private float encounterCooldownDuration = 5f;
    [SerializeField] Animator defaultFaderAnimator;

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
        int encounterThreshold = 20;
        int rngValue = Random.Range(1, 30);

        if (encounterValue + rngValue >= encounterThreshold)
        {
            StartCoroutine(EncounterTransition()); 
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
        FieldEvents.sceneBeforeEncounterName = SceneManager.GetActiveScene().name;
        FieldEvents.coordinatesBeforeEncounter = playerMovementScript.transform.position;
        FieldEvents.lookDirBeforeEncounter = playerMovementScript.lookDirection;
        GameObject mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        CombatEvents.LockPlayerMovement();

        FieldEvents.LerpValues(0, 359, 1, output =>
        {
            mainCam.transform.rotation = Quaternion.Euler(0, 0, output);
        });

        var ppc = mainCam.GetComponent<UnityEngine.Experimental.Rendering.Universal.PixelPerfectCamera>();

        FieldEvents.LerpValues(150, 0, 1f, output =>
        {
            ppc.assetsPPU = Mathf.RoundToInt(output);
        });

        yield return new WaitForSeconds(.5f);

        defaultFaderAnimator.SetTrigger("Trigger2");

        yield return new WaitForSeconds(.5f);

        LoadScene(sceneToLoad);
        encounterValue = 0;
    }
}