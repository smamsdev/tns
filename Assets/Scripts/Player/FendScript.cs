using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FendScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI fendTextMeshProUGUI;
    [SerializeField] CombatManager combatManager;
    [SerializeField] GameObject fendContainer;
    [SerializeField] GameObject fendIcon;
    public Animator animatorContainer;
    [SerializeField] Animator iconAnimator;


    public int attackRemainder;

    public int fend = 0;

    private void OnEnable()
    {
        CombatEvents.BattleMode += Init;
    }

    private void OnDisable()
    {
        CombatEvents.BattleMode -= Init;
    }

    void Init(bool on)
    {
        ShowHideFendDisplay(false);

        fendContainer.transform.position = new Vector2(combatManager.battleScheme.playerFightingPosition.transform.position.x, combatManager.battleScheme.playerFightingPosition.transform.position.y + 0.8f);
    }

    public void ShowHideFendDisplay(bool on)

    {
        if (on && (combatManager.playerMoveManager.firstMoveIs == 2 || combatManager.playerMoveManager.secondMoveIs == 2))

        {
            fendIcon.SetActive(true);
        }

        if (!on)
        {
            fendIcon.SetActive(false);
        }
    }

    public void ApplyEnemyAttackToFend(int attack)

    {
        attackRemainder = attack - fend;
        StartCoroutine(ApplyEnemyAttackToFendCoRo(attack));
    }

    IEnumerator ApplyEnemyAttackToFendCoRo(int attack)

    {
        if (fend == 0)
        {
            FendBreached();
            yield return null;
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
                yield return null;
            }

            yield return null;
        }
    }

    public void FendIconAnimationState(int state)

    {
        animatorContainer.SetInteger("deflectState", state);
    }

    public void UpdateFendText(int value)
    {
            fend = value;
            fendTextMeshProUGUI.text = fend.ToString();
    }

    void FendBreached()

    {
        iconAnimator.SetBool("fendbreak", true);
        fendTextMeshProUGUI.text = "";
        if (attackRemainder > 0)
        {
            combatManager.combatUIScript.playerDamageTakenDisplay.ShowPlayerDamageDisplay(attackRemainder);
        }
    }
    
}
