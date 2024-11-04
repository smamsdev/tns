using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitChanger : ToTrigger
{
    public string newExitIDString;
    public LevelLoaderScript exitToChange; 

    public override IEnumerator DoAction()
    {
        exitToChange.sceneID = newExitIDString;

        FieldEvents.HasCompleted.Invoke(this.gameObject);
        yield return null;
    }
}
