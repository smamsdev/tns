using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GearMonoBehaviour : MonoBehaviour
{
    [HideInInspector] public CombatManager combatManager;
    public GearInstance gearInstance;

    public virtual IEnumerator GearEffectOnTurn()
    {
        yield return null;
    }
}
