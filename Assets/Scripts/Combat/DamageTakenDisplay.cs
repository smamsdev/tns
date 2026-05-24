using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTakenDisplay : MonoBehaviour
{
    [SerializeField] Animator animator;
    public TextMeshProUGUI DamageTakenTextMeshProUGUI, backStabText;

    public IEnumerator ShowDamageDisplayCoro(int change, Combatant combatant)
    {
        if (change == 0)
            yield break;

        float lerpDuration = .75f;
        int startValue = 1;
        int deltaToDisplay;

        if (change > 0)
        {
            animator.Play("HealingTaken");
            deltaToDisplay = Mathf.Min(change, combatant.MaxHP - combatant.CurrentHP);
        }
        else
        {
            animator.Play("DamageTaken");
            deltaToDisplay = change;
        }

        yield return FieldEvents.LerpValuesCoRo(startValue, deltaToDisplay, lerpDuration, UpdateTMP);

        yield return new WaitForSeconds(0.5f);
    }

    void UpdateTMP(float output)
    {
        string changeDisplay = Mathf.Abs(Mathf.RoundToInt(output)).ToString();

        DamageTakenTextMeshProUGUI.text = changeDisplay;
    }
}
