using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Combatant : MonoBehaviour
{
    public string combatantName;
    public CombatantUI combatantUI;
    public MovementScript movementScript;

    public int attackBase;
    public int fendBase;
    public int maxHP;
    public bool fendDisplayOn;
    public bool isEncounterSpawned;

    public int CurrentHP
    {
        get 
        { 
            return currentHP;
        }
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
        StartCoroutine(UpdateHPCoRo(value));
    }

    public virtual IEnumerator UpdateHPCoRo(int value)
    {
        var newHPValue = CurrentHP + value;

        float elapsedTime = 0f;
        float lerpDuration = 1f;
        int valueToOutput;

        combatantUI.statsDisplay.HPTMPAnimator.SetTrigger("bump");

        while (elapsedTime < lerpDuration)
        {
            float t = Mathf.Clamp01(elapsedTime / lerpDuration);

            valueToOutput = Mathf.RoundToInt(Mathf.Lerp(CurrentHP, newHPValue, t));
            CurrentHP = valueToOutput;
            combatantUI.statsDisplay.UpdateHPDisplay(CurrentHP);

            if (CurrentHP <= 0)
            {
                Defeated();
                yield break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
    }

    public void Defeated()
    { 
        var combatantAnimator = this.GetComponent<Animator>();
        combatantAnimator.SetBool("Defeated", true);
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
