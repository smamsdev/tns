using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FendScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI fendTextMeshProUGUI;
    [HideInInspector] public CombatManager combatManager;
    [SerializeField] GameObject fendTextGO, fendIconGO;
    public Animator animator;
    Combatant combatantAttacking;
    Combatant target;

    public int attackRemainder;

    public int fend = 0;

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
        }
    }

    public void ApplyAttackToFend(Combatant combatant, Combatant target)

    {
        combatantAttacking = combatant;
        this.target = target;
        attackRemainder = combatantAttacking.attackTotal - fend;
        animator.SetTrigger("fendDeflect");
        combatManager.playerAnimator.SetTrigger("Pain");

        StartCoroutine(ApplyAttackToFendCoRo(combatantAttacking.attackTotal));
    }

    IEnumerator ApplyAttackToFendCoRo(int attack)

    {
        float combatantAttackingLookDirX = combatantAttacking.GetComponent<MovementScript>().lookDirection.x;
        var stepBackPos = new Vector2
            (target.transform.position.x + (combatantAttacking.moveSelected.attackPushStrength * combatantAttackingLookDirX),
            target.transform.position.y);

        if (fend == 0)
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

        int startNumber = fend;

        int endValue = fend - attack;

        while (elapsedTime < lerpDuration && fend > 0)
        {
            // Calculate the interpolation factor between 0 and 1 based on the elapsed time and duration
            float t = Mathf.Clamp01(elapsedTime / lerpDuration);

            // Lerp between the start and end values
            fend = Mathf.RoundToInt(Mathf.Lerp(startNumber, endValue, t));
            fendTextMeshProUGUI.text = fend.ToString();

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            if (fend == 0)
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

    public void UpdateFendText(int value)
    {
        fend = value;
        fendTextMeshProUGUI.text = fend.ToString();
    }

    void FendBreached()
    {
        animator.SetTrigger("fendBreak");
        fendTextMeshProUGUI.text = "";
        if (attackRemainder > 0)
        {
            if (target is PlayerCombat)

            {
                combatManager.CombatUIManager.playerDamageTakenDisplay.ShowPlayerDamageDisplay(attackRemainder);
                target.UpdateHP(-attackRemainder);
            }

            if (target is Enemy)
            { 
                var targetUI = target.GetComponentInChildren<EnemyUI>();
                target.UpdateHP(-attackRemainder);
                //targetUI.enemyDamageTakenDisplay.ShowEnemyDamageDisplay(attackRemainder);
            }

            if (target is Ally)
            {
                var targetUI = target.GetComponentInChildren<AllyUI>();
                target.UpdateHP(-attackRemainder);
               // targetUI.allyDamageTakenDisplay.ShowAllyDamageDisplay(attackRemainder);
            }
        }
    }

    public void ResetAllAnimationTriggers()
    {
        animator.ResetTrigger("fendAppear");
        animator.ResetTrigger("fendDeflect");
        animator.ResetTrigger("fendBreak");
        animator.ResetTrigger("fendFade");
    }
}
