using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Combatant : MonoBehaviour
{
    public string combatantName;
    public CombatantUI combatantUI;

    public int attackBase;
    public int fendBase;
    public int attackPowerBase;
    public int maxHP;
    private int currentHP;

    public int attackTotal;
    public int fendTotal;
    public int injuryPenalty;

    public Vector2 forceLookDirection;
    public GameObject fightingPosition;
    public Combatant targetToAttack;
    public Move moveSelected;

    public abstract void SelectMove();

    public abstract void UpdateHP(int value);

    public int CurrentHP
    {
        get { return currentHP; }
        set
        {
            currentHP = Mathf.Clamp(value, 0, 9999);
        }
    }

    public virtual void InitialiseCombatantStats()
    {
        CurrentHP = maxHP;
        combatantUI.statsDisplay.combatantHP = CurrentHP;
        combatantUI.statsDisplay.combatantHPTextMeshPro.text = "HP: " + CurrentHP.ToString();

        combatantUI.statsDisplay.combatant = this;
        combatantUI.statsDisplay.combatantHP = CurrentHP;
        combatantUI.statsDisplay.combatantNameTextMeshPro.text = combatantName;
    }
}
