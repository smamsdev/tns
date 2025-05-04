using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Combatant : MonoBehaviour
{
    public string combatantName;
    public CombatantUI combatantUI;
    public Rigidbody2D rigidbody2D;

    public int attackBase;
    public int fendBase;
    public int maxHP;
    public bool fendDisplayOn;

    public int CurrentHP
    {
        get { return currentHP; }
        set
        {
            currentHP = Mathf.Clamp(value, 0, 9999);
        }
    }

    [SerializeField] int currentHP;

    public int attackTotal;
    public int fendTotal;
    public int injuryPenalty;

    public Vector2 forceLookDirection;
    public GameObject fightingPosition;
    public Combatant targetToAttack;
    public Move moveSelected;

    public abstract void SelectMove();

    public virtual void UpdateHP(int value)
    {
        CurrentHP += value;
        combatantUI.statsDisplay.UpdateHPDisplay(value);
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
