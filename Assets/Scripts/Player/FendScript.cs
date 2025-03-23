using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class FendScript : MonoBehaviour
{
    public TextMeshProUGUI fendTextMeshProUGUI;
    [HideInInspector] public CombatManager combatManager;
    [SerializeField] GameObject fendTextGO, fendIconGO;
    [SerializeField] Animator animator;
    Combatant combatantAttacking;
    Combatant target;
    public int attackRemainder;

    public void ShowFendDisplay(Combatant combatantToShow, bool on)
    {
        if (on)
        {
            if (combatantToShow.fendTotal > 0)
            {
                animator.Play("FendAppear", 0 , 0);
            }
            combatantToShow.fendDisplayOn = on;
            Debug.Log("on");
        }

        if (!on)
        {
            animator.Play("FendFade", 0, 0);
            Debug.Log("off");
            combatantToShow.fendDisplayOn = false;
        }
    }

    public void ApplyAttackToFend(Combatant combatant, Combatant target)
    {
        combatantAttacking = combatant;
        this.target = target;
        attackRemainder = combatantAttacking.attackTotal - target.fendTotal;
        animator.Play("FendDeflect", 0, 0);
        var combatantAnimator = target.GetComponent<Animator>();
        combatantAnimator.SetTrigger("Pain");

        StartCoroutine(ApplyAttackToFendCoRo(combatantAttacking.attackTotal));
    }

    IEnumerator ApplyAttackToFendCoRo(int attack)
    {
        float combatantAttackingLookDirX = combatantAttacking.GetComponent<MovementScript>().lookDirection.x;
        var stepBackPos = new Vector2
            (target.transform.position.x + (combatantAttacking.moveSelected.attackPushStrength * combatantAttackingLookDirX),
            target.transform.position.y);

        if (target.fendTotal == 0)
        {
            FendBreached();
            yield return new WaitForSeconds(0.2f);

            var combatMovementInstanceGO = Instantiate(combatManager.combatMovementPrefab, this.transform);
            var combatMovementInstance = combatMovementInstanceGO.GetComponent<CombatMovement>();
            yield return (combatMovementInstance.MoveCombatantFixedTime(target.gameObject, stepBackPos, combatantAttacking.moveSelected.attackPushStrength, isReversing: true));
            Destroy(combatMovementInstanceGO);
        }

        float elapsedTime = 0f;
        float lerpDuration = 0.5f;

        int startNumber = target.fendTotal;

        int endValue = target.fendTotal - attack;

        while (elapsedTime < lerpDuration && target.fendTotal > 0)
        {
            // Calculate the interpolation factor between 0 and 1 based on the elapsed time and duration
            float t = Mathf.Clamp01(elapsedTime / lerpDuration);

            // Lerp between the start and end values
            target.fendTotal = Mathf.RoundToInt(Mathf.Lerp(startNumber, endValue, t));
            fendTextMeshProUGUI.text = target.fendTotal.ToString();

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            if (target.fendTotal == 0)
            {
                FendBreached();
                yield return new WaitForSeconds(0.2f);

                var combatMovementInstanceGO = Instantiate(combatManager.combatMovementPrefab, this.transform);
                var combatMovementInstance = combatMovementInstanceGO.GetComponent<CombatMovement>();
                yield return (combatMovementInstance.MoveCombatantFixedTime(target.gameObject, stepBackPos, combatantAttacking.moveSelected.attackPushStrength, isReversing: true));
                Destroy(combatMovementInstanceGO);
            }

            yield return null;
        }
        animator.Play("FendFade", 0, 0);
        Debug.Log("ticker off");
    }

    void FendBreached()
    {
        animator.Play("FendBreak", 0, 0);
        fendTextMeshProUGUI.text = "";
        if (attackRemainder > 0)
        {
            StartCoroutine(target.combatantUI.damageTakenDisplay.ShowDamageDisplayCoro(attackRemainder));
            target.UpdateHP(-attackRemainder);
        }
    }
}
