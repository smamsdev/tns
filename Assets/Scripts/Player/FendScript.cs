using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class FendScript : MonoBehaviour
{
    public TextMeshProUGUI fendTextMeshProUGUI;
    public CombatManager combatManager;
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
                combatantToShow.fendDisplayOn = on;
            }
        }

        if (!on && combatantToShow.fendDisplayOn)
        {
            animator.Play("FendFade", 0, 0);
            //Debug.Log("off");
            combatantToShow.fendDisplayOn = false;
        }
    }

    public void ApplyAttackToFend(Combatant combatant, Combatant target)
    {
        var combatantToActLookDir = combatant.GetComponent<MovementScript>().lookDirection;
        var targetLookDir = target.GetComponent<MovementScript>().lookDirection;

        if (combatantToActLookDir == targetLookDir)
        {
            Debug.Log("baclkstadck!!!");  
        }

        combatantAttacking = combatant;
        this.target = target;
        attackRemainder = combatantAttacking.attackTotal - target.fendTotal;
        attackRemainder = combatantAttacking.attackTotal - target.fendTotal;
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
            if (attackRemainder > 0)
            {
                StartCoroutine(target.combatantUI.damageTakenDisplay.ShowDamageDisplayCoro(attackRemainder));
                target.UpdateHP(-attackRemainder);
            }

            var combatMovementInstanceGO = Instantiate(combatManager.combatMovementPrefab, this.transform);
            var combatMovementInstance = combatMovementInstanceGO.GetComponent<CombatMovement>();
            yield return (combatMovementInstance.MoveCombatantFixedTime(target.gameObject, stepBackPos, combatantAttacking.moveSelected.attackPushStrength, isReversingX: true));
            Destroy(combatMovementInstanceGO);
            yield break;
        }

        animator.Play("FendDeflect", 0, 0);

        float elapsedTime = 0f;
        float lerpDuration = 0.5f;

        int startNumber = target.fendTotal;

        int endValue = target.fendTotal - attack;

        while (elapsedTime < lerpDuration && target.fendTotal > 0)
        {
            float t = Mathf.Clamp01(elapsedTime / lerpDuration);

            target.fendTotal = Mathf.RoundToInt(Mathf.Lerp(startNumber, endValue, t));
            fendTextMeshProUGUI.text = target.fendTotal.ToString();

            elapsedTime += Time.deltaTime;

            if (target.fendTotal == 0)
            {
                animator.Play("FendBreak", 0, 0);
                fendTextMeshProUGUI.text = "";
                if (attackRemainder > 0)
                {
                    StartCoroutine(target.combatantUI.damageTakenDisplay.ShowDamageDisplayCoro(attackRemainder));
                    target.UpdateHP(-attackRemainder);
                }

                target.fendDisplayOn = false;
                yield return new WaitForSeconds(0.2f);

                var combatMovementInstanceGO = Instantiate(combatManager.combatMovementPrefab, this.transform);
                var combatMovementInstance = combatMovementInstanceGO.GetComponent<CombatMovement>();
                yield return (combatMovementInstance.MoveCombatantFixedTime(target.gameObject, stepBackPos, combatantAttacking.moveSelected.attackPushStrength, isReversingX: true));
                Destroy(combatMovementInstanceGO);
            }
            yield return null;
        }
    }
}
