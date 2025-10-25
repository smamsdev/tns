using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Combatant : MonoBehaviour
{
    public string combatantName;
    public CombatantUI combatantUI;
    public MovementScript movementScript;
    public GameObject fightingPosition;
    public bool isBackstabbed;
    public int CombatLookDirX
    {
        get => _combatLookDirX;
        set
        {
            _combatLookDirX = value;
            movementScript.animator.SetFloat("CombatLookDirX", value);
        }
    }

    [SerializeField] private int _combatLookDirX;

    [Header("Stats")]

    public int attackBase;
    public int fendBase;
    public int maxHP;
    [Tooltip("Set by code. Leave as 0.")]
    public int attackTotal;
    [Tooltip("Set by code. Leave as 0.")]
    public int fendTotal = 0;
    public int CurrentHP
    {
        get
        {
            return currentHP;
        }
        set
        {
            currentHP = Mathf.Clamp(value, 0, maxHP);
        }
    }

    [SerializeField] int currentHP;

    private void OnEnable()
    {
        movementScript = GetComponent<MovementScript>();
    }


    [Header("Moves")]
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

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
    }
}
