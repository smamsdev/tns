using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitChanger : ToTrigger
{
    public string newExitString;
    public LevelLoaderScript exitToChange; 

    public override IEnumerator DoAction()
    {
        exitToChange.sceneName = newExitString;

        FieldEvents.HasCompleted.Invoke(this.gameObject);
        yield return null;
    }
}
