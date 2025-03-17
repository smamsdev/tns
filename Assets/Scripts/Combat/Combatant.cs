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
    public int injuryPenalty;

    public Vector2 forceLookDirection;
    public GameObject fightingPosition;
    public Combatant targetToAttack;
    public Move moveSelected;

    public abstract void SelectMove();

    public abstract void UpdateHP(int value);

}
