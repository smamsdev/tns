using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameChanger : ToTrigger
{
    public string nameToChange;
    [SerializeField] GameObject GameObjectToChange;

    public override IEnumerator DoAction()
    {
        GameObjectToChange.name = nameToChange;
        FieldEvents.HasCompleted.Invoke(this.gameObject);
        yield return null;
    }
}
