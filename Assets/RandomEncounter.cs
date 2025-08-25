using UnityEngine;
using UnityEngine.SceneManagement;

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
            LoadScene(sceneToLoad);
            encounterValue = 0; 
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
}