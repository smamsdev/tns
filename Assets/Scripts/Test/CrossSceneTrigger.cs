using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSceneTrigger : ToTrigger
{
    [SerializeField] int triggerReferenceID;

    public override IEnumerator TriggerFunction()
    {
        yield return CrossSceneReferences.triggerReferences[triggerReferenceID].Triggered();
    }
}
