using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneIterationUpdater : ToTrigger
{
    public SceneCombination sceneComboToChange;
    [HideInInspector] public int additiveIterationToChangeTo;

    public override IEnumerator TriggerFunction()
    {
        sceneComboToChange.activeAdditiveIteration = additiveIterationToChangeTo;
        return null;
    }
}
