using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AllyFendScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI fendTextMeshProUGUI;
    public CombatManager combatManager;
    [SerializeField] GameObject fendGameObject;
    [SerializeField] GameObject fendTextGameObject;
    public Animator allyFendAnimator;
    Ally ally;
    Animator enemyAnimator;

    int attackRemainder;

    public void ApplyPlayerAttackToFend(int attack, Vector2 playerLookDirection, float attackPushStrength)

    {
       // ally = combatManager.ally[combatManager.selectedEnemy];
        enemyAnimator = ally.GetComponent<Animator>();

        attackRemainder = attack - combatManager.enemies[combatManager.selectedEnemy].fendTotal;
        allyFendAnimator.SetTrigger("fendDeflect");

        enemyAnimator.SetTrigger("Pain");

        StartCoroutine(ApplyPlayerAttackToFendCoroutine(attack, playerLookDirection, attackPushStrength));
    }

    IEnumerator ApplyPlayerAttackToFendCoroutine(int attack, Vector2 playerLookDirection, float attackPushStrength)

    {
        var stepBackPos = new Vector3(ally.fightingPosition.transform.position.x + (attackPushStrength * playerLookDirection.x), ally.fightingPosition.transform.position.y);

        if (combatManager.enemies[combatManager.selectedEnemy].fendTotal == 0)
        {
            FendBreached();
            yield return new WaitForSeconds(0.2f);

            var combatMovementInstanceGO = Instantiate(combatManager.combatMovementPrefab, this.transform);
            var combatMovementInstance = combatMovementInstanceGO.GetComponent<CombatMovement>();
            yield return (combatMovementInstance.MoveCombatantFixedTime(ally.gameObject, stepBackPos, attackPushStrength, isReversing: true));
            Destroy(combatMovementInstanceGO);

            enemyAnimator.SetTrigger("CombatIdle");


            yield return null;
        }

        float elapsedTime = 0f;
        float lerpDuration = 0.5f;

        int startNumber = combatManager.enemies[combatManager.selectedEnemy].fendTotal;

        int endValue = combatManager.enemies[combatManager.selectedEnemy].fendTotal - attack;

        while (elapsedTime < lerpDuration && combatManager.enemies[combatManager.selectedEnemy].fendTotal > 0)
        {
            float t = Mathf.Clamp01(elapsedTime / lerpDuration);

            combatManager.enemies[combatManager.selectedEnemy].fendTotal = Mathf.RoundToInt(Mathf.Lerp(startNumber, endValue, t));
            fendTextMeshProUGUI.text = combatManager.enemies[combatManager.selectedEnemy].fendTotal.ToString();

            elapsedTime += Time.deltaTime;

            if (combatManager.enemies[combatManager.selectedEnemy].fendTotal == 0)
            {
                FendBreached();

                var combatMovementInstanceGO = Instantiate(combatManager.combatMovementPrefab, this.transform);
                var combatMovementInstance = combatMovementInstanceGO.GetComponent<CombatMovement>();
                yield return (combatMovementInstance.MoveCombatantFixedTime(ally.gameObject, stepBackPos, attackPushStrength));
                Destroy(combatMovementInstanceGO);

                enemyAnimator.SetTrigger("CombatIdle");

                yield return null;
            }

            enemyAnimator.SetTrigger("CombatIdle");
            yield return null;
        }
    }

    void FendBreached()

    {
        allyFendAnimator.SetTrigger("fendBreak");
        fendTextMeshProUGUI.text = "";
        if (attackRemainder > 0)
        {
            ally.DamageTaken(attackRemainder);
        }
    }

    public void ShowFendDisplay(bool on)

    {
        if (on)

        {
            fendGameObject.SetActive(true);
            fendTextGameObject.SetActive(true);
        }

        if (!on)
        {
            fendGameObject.SetActive(false);
            fendTextGameObject.SetActive(false);

            Debug.Log("do you really need this");
        }
    }

    public void UpdateFendDisplay(int fend)
    {
        if (fend > 0)
        {
            fendTextMeshProUGUI.text = fend.ToString();
            ShowFendDisplay(true);
            allyFendAnimator.SetTrigger("fendAppear");
        }

        else
        {
            fendGameObject.SetActive(false);
        }
    }

    public void ResetAllAnimationTriggers()

    {
        allyFendAnimator.ResetTrigger("fendAppear");
        allyFendAnimator.ResetTrigger("fendDeflect");
        allyFendAnimator.ResetTrigger("fendBreak");
        allyFendAnimator.ResetTrigger("fendFade");
    }
}
