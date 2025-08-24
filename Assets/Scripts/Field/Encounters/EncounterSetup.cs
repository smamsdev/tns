using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterSetup : ToTrigger
{
    public EncounterProfile[] encounterProfiles;

    public override IEnumerator DoAction()
    {
        Encounter encounterToTrigger = SelectEncounter();
        StartCoroutine(encounterToTrigger.DoAction());
        yield return null;
    }

    Encounter SelectEncounter()
    {
        int encounterWeightingTotal = 0;

        foreach (EncounterProfile profile in encounterProfiles)
        {
            if (profile.encounterWeighting > 0)
            {
                encounterWeightingTotal += profile.encounterWeighting;
            }
        }

        if (encounterWeightingTotal == 0)
        {
            Debug.LogError("No valid moves available to select!");
            return null;
        }

        int randomValue = Random.Range(1, encounterWeightingTotal + 1);

        foreach (EncounterProfile profile in encounterProfiles)
        {
            if (profile.encounterWeighting == 0) continue;

            if (randomValue > profile.encounterWeighting)
            {
                randomValue -= profile.encounterWeighting;
            }
            else
            {
                return profile.encounter;
            }
        }

        Debug.LogError("Failed to select an encounter! Random value was " + randomValue);
        return null;
    }
}

[System.Serializable]

public class EncounterProfile
{
    public Encounter encounter;
    public int encounterWeighting;
}
