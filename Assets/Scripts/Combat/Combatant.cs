using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Combatant : MonoBehaviour
{
    public string combatantName;

    public int attackBase;
    public int fendBase;
    public int attackPowerBase;
    public int maxHP;
    public int currentHP;

    public int attackTotal;
    public int fendTotal;

    public Vector2 forceLookDirection;
    public GameObject fightingPosition;

    public abstract void SelectMove();
}
