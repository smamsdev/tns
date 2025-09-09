using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gear : MonoBehaviour
{
    [HideInInspector] public CombatManager combatManager;
    public int turnsUntilConsumed;

    public abstract IEnumerator ApplyGear();
}
