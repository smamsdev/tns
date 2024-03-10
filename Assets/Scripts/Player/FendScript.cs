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
    [SerializeField] Animator animator;
    [SerializeField] Animator iconAnimator;


    public int attackRemainder;

    public int fend = 0;

    private void OnEnable()
    {
        CombatEvents.BattleMode += Init;
        CombatEvents.AnimatorTrigger += TriggerAnimation;
    }

    private void OnDisable()
    {
        CombatEvents.BattleMode -= Init;
        CombatEvents.AnimatorTrigger -= TriggerAnimation;
    }

    void Init(bool on)
    {
        ShowHideFendDisplay(false);

        fendContainer.transform.position = new Vector2(combatManager.battleScheme.playerFightingPosition.transform.position.x, combatManager.battleScheme.playerFightingPosition.transform.position.y + 0.8f);
    }

    void TriggerAnimation(string trigger)

    {
        animator.SetTrigger(trigger);
    }

    public void ShowHideFendDisplay(bool on)

    {
        if (on && (combatManager.playerMoveManager.firstMoveIs == 2 || combatManager.playerMoveManager.secondMoveIs == 2))

        {
            fendContainer.SetActive(true);
        }

        if (!on)
        {
            fendContainer.SetActive(false);
        }
    }

    public void ApplEnemyAttackToFend(int attack)

    {
        StartCoroutine(ApplEnemyAttackToFendCoRo(attack));
        attackRemainder = attack - fend;
    
    }

    IEnumerator ApplEnemyAttackToFendCoRo(int attack)

    {
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

    public void UpdateFendText(int value)
    {
            fend = value;
            fendTextMeshProUGUI.text = fend.ToString();
    }

    void FendBreached()

    {
        iconAnimator.SetBool("fendbreak", true);
        fendTextMeshProUGUI.text = "";
        combatManager.combatUIScript.playerDamageTakenDisplay.ShowPlayerDamageDisplay(attackRemainder);
    }
    

}
