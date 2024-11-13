using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FendScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI fendTextMeshProUGUI;
    public CombatManager combatManager;
    [SerializeField] GameObject playerFendContainer;
    [SerializeField] GameObject playerFendIcon, playerFendText;
    public Animator animator;
    Vector2 enemyLookDir;
    float attackPushStrength;

    public int attackRemainder;

    public int fend = 0;

    private void OnEnable()
    {
        CombatEvents.ApplyEnemyAttackToFend += ApplyEnemyAttackToFend;
    }

    private void OnDisable()
    {
        CombatEvents.ApplyEnemyAttackToFend -= ApplyEnemyAttackToFend;
    }

    public void ShowFendDisplay(bool on)
    {
        if (on && combatManager.playerCombatStats.playerFend > 0)

        {
            playerFendIcon.SetActive(true);
            playerFendText.SetActive(true);
            animator.SetTrigger("fendAppear");
        }

        if (!on)
        {
            playerFendIcon.SetActive(false);
            playerFendText.SetActive(false);
        }
    }

    public void ApplyEnemyAttackToFend(int attack, Vector2 enemyLookDirection, float _attackPushStrength)

    {
        attackRemainder = attack - fend;
        animator.SetTrigger("fendDeflect");
        combatManager.playerAnimator.SetTrigger("Pain");
        enemyLookDir = enemyLookDirection;
        attackPushStrength = _attackPushStrength;

        StartCoroutine(ApplyEnemyAttackToFendCoRo(attack));
    }

    IEnumerator ApplyEnemyAttackToFendCoRo(int attack)

    {
        var stepBackPos = new Vector2
            (combatManager.battleScheme.playerFightingPosition.transform.position.x + (attackPushStrength * enemyLookDir.x),
            combatManager.battleScheme.playerFightingPosition.transform.position.y);

        if (fend == 0)
        {
            FendBreached();
            yield return new WaitForSeconds(0.2f);
            yield return (combatManager.combatMovement.MoveCombatantFixedTime(combatManager.player.gameObject, stepBackPos, isReversing: true));
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
                yield return (combatManager.combatMovement.MoveCombatantFixedTime(combatManager.player.gameObject, stepBackPos, isReversing: true));
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
            combatManager.CombatUIManager.playerDamageTakenDisplay.ShowPlayerDamageDisplay(attackRemainder);
            CombatEvents.UpdatePlayerHP.Invoke(-attackRemainder);
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
