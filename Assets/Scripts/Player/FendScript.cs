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
        if (combatantToShow.fendTotal > 0)
        {
            if (on)
            {
                fendAnimator.Play("FendAppear");
                fendTextMeshProUGUI.text = combatantToShow.fendTotal.ToString();
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

    public IEnumerator ApplyAttackToCombatant(Combatant combatant, Combatant target)
    {
        int backStabBonus = target.isBackstabbed ? combatant.attackTotal : 0;

        if (target.isBackstabbed)
        {
            backStabAnimator.Play("BackStabShowAndFade");
        }

        int attackCombinedTotal = combatant.attackTotal + backStabBonus;
        int attackRemainder = attackCombinedTotal - target.fendTotal;
        target.GetComponent<Animator>().Play("Pain");

        if (target.fendTotal == 0)
        {
            yield return PushBack();
            yield return DamageTaken();
            yield break;
        }

        fendAnimator.Play("FendDeflect", 0, 0);

        float elapsedTime = 0f;
        float lerpDuration = 1f;
        int startNumber = target.fendTotal;
        int endValue = target.fendTotal - attackCombinedTotal;

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
                yield return PushBack();
                yield return DamageTaken();
            }

            yield return null;
        }

        IEnumerator PushBack()
        {
            var stepBackPos = new Vector2
            (target.transform.position.x + (combatant.moveSOSelected.attackPushStrength * combatant.CombatLookDirX),
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
