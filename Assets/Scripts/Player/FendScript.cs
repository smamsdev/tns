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

    public void ShowFendDisplay(Combatant combatantToShow, bool on)
    {
        if (combatantToShow.FendTotal > 0)
        {
            if (on)
            {
                fendAnimator.Play("FendAppear");
                fendTextMeshProUGUI.text = combatantToShow.FendTotal.ToString();
            }

            else
            {
                fendAnimator.Play("FendAppearReverse");
            }
        }

        else
        {
            fendAnimator.Play("FendDefault");
        }
    }

    public IEnumerator ApplyAttackToCombatant(Combatant combatantApplying, Combatant target)
    {
        int backStabBonus = target.isBackstabbed ? combatantApplying.AttackTotal : 0;

        if (target.isBackstabbed)
        {
            backStabAnimator.Play("BackStabShowAndFade");
        }

        int attackCombinedTotal = combatantApplying.AttackTotal + backStabBonus;
        int attackRemainder = attackCombinedTotal - target.FendTotal;
        target.GetComponent<Animator>().Play("Pain");

        if (target.FendTotal == 0)
        {
            yield return PushBack();
            yield return DamageTaken();
            yield break;
        }

        fendAnimator.Play("FendDeflect", 0, 0);

        float elapsedTime = 0f;
        float lerpDuration = 1f;
        int startNumber = target.FendTotal;
        int endValue = target.FendTotal - attackCombinedTotal;

        while (elapsedTime < lerpDuration && target.FendTotal > 0)
        {
            float t = Mathf.Clamp01(elapsedTime / lerpDuration);
            target.FendTotal = Mathf.RoundToInt(Mathf.Lerp(startNumber, endValue, t));
            fendTextMeshProUGUI.text = target.FendTotal.ToString();
            elapsedTime += Time.deltaTime;

            if (target.FendTotal == 0)
            {
                fendAnimator.Play("FendBreak", 0, 0);
                fendTextMeshProUGUI.text = "";
                yield return PushBack();
                yield return DamageTaken();
            }

            yield return null;
        }

        yield return new WaitForSeconds(.5f);

        IEnumerator PushBack()
        {
            var stepBackPos = new Vector2
            (target.transform.position.x + (combatantApplying.moveSelected.moveSO.AttackPushStrength * combatantApplying.CombatLookDirX),
            target.transform.position.y);

            var combatMovementInstanceGO = Instantiate(combatManager.combatMovementPrefab, this.transform);
            var combatMovementInstance = combatMovementInstanceGO.GetComponent<CombatMovement>();
            yield return (combatMovementInstance.LerpPositionFixedTime(target.gameObject, stepBackPos, .5f));
            Destroy(combatMovementInstanceGO);

            yield break;
        }

        IEnumerator DamageTaken()
        {
            target.combatantUI.statsDisplay.ShowStatsDisplay(true);

            if (attackRemainder > 0)
            {
                StartCoroutine(target.combatantUI.damageTakenDisplay.ShowDamageDisplayCoro(attackRemainder));
                target.UpdateHP(-attackRemainder);
            }

            yield return new WaitForSeconds(1.5f);
            target.combatantUI.statsDisplay.ShowStatsDisplay(false);

            yield break;
        }
    }
}
