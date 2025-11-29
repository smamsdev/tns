using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ToTrigger : MonoBehaviour
{
    public virtual IEnumerator Triggered()
    {
        yield return TriggerFunction();
        TriggerComplete();
    }

    public abstract IEnumerator TriggerFunction();

    protected virtual void TriggerComplete()
    {
        FieldEvents.HasCompleted.Invoke(this.gameObject);
    }
}
