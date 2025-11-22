using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTriggerReference : MonoBehaviour
{
    [SerializeField] ToTrigger trigger;
    [SerializeField] int referenceID;

    void Awake()
    {
        UpdateCrossReferenceTrigger();
    }

    void UpdateCrossReferenceTrigger()
    {
        if (CrossSceneReferences.triggerReferences.ContainsKey(referenceID))
        {
            Debug.LogError($"Duplicate referenceID '{referenceID}' detected on '{name}'. ");
            return;
        }

        CrossSceneReferences.triggerReferences.Add(referenceID, trigger);
    }
}
