using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{

    public virtual IEnumerator StartState()

    {
        yield break;
    }

    public virtual void StateUpdate()
    {
        
    }

}
