using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    protected readonly CombatManagerV3 combatManagerV3;

    public State(CombatManagerV3 _combatManagerV3)

    { 
        combatManagerV3 = _combatManagerV3;

    }


    public virtual IEnumerator Start()

    {
        yield break;
    }

    public virtual void Update()

    {
  
    }

    public virtual IEnumerator Heal()

    {
        yield break;
    }


}
