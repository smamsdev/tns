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
    [SerializeField] Animator fendAnimator, backStabAnimator;
    Combatant combatantAttacking;
    Combatant target;
    public int attackRemainder;
    public int backStabBonus;

    public void ShowFendDisplay(Combatant combatantToShow, bool on)
    {
        if (on)
        {
            if (combatantToShow.fendTotal > 0)
            {
                fendAnimator.Play("FendAppear", 0 , 0);
                combatantToShow.fendDisplayOn = on;
            }
        }

        if (!on && combatantToShow.fendDisplayOn)
        {
            fendAnimator.Play("FendFade", 0, 0);
            //Debug.Log("off");
            combatantToShow.fendDisplayOn = false;
        }
    }

    public void ApplyAttackToFend(Combatant combatant, Combatant target)
    {
        var combatantToActLookDir = combatant.GetComponent<MovementScript>().lookDirection;
        var targetLookDir = target.GetComponent<MovementScript>().lookDirection;
        combatantAttacking = combatant;
        this.target = target;
        var combatantAnimator = target.GetComponent<Animator>();

        if (combatantToActLookDir == targetLookDir)
        {
            backStabBonus = combatantAttacking.attackTotal;
        }

        else
        {
            backStabBonus = 0;
        }

        attackRemainder = (combatantAttacking.attackTotal + backStabBonus) - target.fendTotal;
        combatantAnimator.SetTrigger("Pain");

        StartCoroutine(ApplyAttackToFendCoRo(combatantAttacking.attackTotal + backStabBonus));
    }

    IEnumerator ApplyAttackToFendCoRo(int attack)
    {
        float combatantAttackingLookDirX = combatantAttacking.GetComponent<MovementScript>().lookDirection.x;
        var stepBackPos = new Vector2
            (target.transform.position.x + (combatantAttacking.moveSelected.attackPushStrength * combatantAttackingLookDirX),
            target.transform.position.y);

        if (backStabBonus > 0)
        {
            backStabAnimator.Play("BackStabShowAndFade");
        }

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

        fendAnimator.Play("FendDeflect", 0, 0);


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
                fendAnimator.Play("FendBreak", 0, 0);
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
