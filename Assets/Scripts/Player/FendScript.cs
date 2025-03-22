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
    public Animator animator;
    Combatant combatantAttacking;
    Combatant target;
    public int attackRemainder;

    public void ShowFendDisplay(bool on)
    {
        if (on)
        {
            fendTextGO.SetActive(true);
            fendIconGO.SetActive(true);
            animator.SetTrigger("fendAppear");
        }

        if (!on)
        {
            fendTextGO.SetActive(false);
            fendIconGO.SetActive(false);
            animator.SetTrigger("fendFade");
        }
    }

    public void ApplyAttackToFend(Combatant combatant, Combatant target)
    {
        combatantAttacking = combatant;
        this.target = target;
        attackRemainder = combatantAttacking.attackTotal - target.fendTotal;
        animator.SetTrigger("fendDeflect");
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
    }

    void FendBreached()
    {
        animator.SetTrigger("fendBreak");
        animator.ResetTrigger("fendAppear");
        fendTextMeshProUGUI.text = "";
        if (attackRemainder > 0)
        {
            StartCoroutine(target.combatantUI.damageTakenDisplay.ShowDamageDisplayCoro(attackRemainder));
            target.UpdateHP(-attackRemainder);
        }
    }

    public void ResetAllFendAnimationTriggers()
    {
        animator.ResetTrigger("fendAppear");
        animator.ResetTrigger("fendDeflect");
        animator.ResetTrigger("fendBreak");
        animator.ResetTrigger("fendFade");
    }
}
