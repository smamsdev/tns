using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyFendScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI fendTextMeshProUGUI;
    public CombatManager combatManager;
    [SerializeField] GameObject fendGameObject;
    [SerializeField] GameObject fendTextGameObject;
    public Animator enemyFendAnimator;
    Enemy enemy;

    int attackRemainder;

    public void ApplyPlayerAttackToFend(int attack, Vector2 playerLookDirection, float attackPushStrength)

    {
        enemy = combatManager.enemy[combatManager.selectedEnemy];
        var enemyAnimator = enemy.GetComponent<Animator>();

        attackRemainder = attack - combatManager.enemy[combatManager.selectedEnemy].fendTotal;
        enemyFendAnimator.SetTrigger("fendDeflect");

        enemyAnimator.SetTrigger("Pain");

        StartCoroutine(ApplyPlayerAttackToFendCoroutine(attack, playerLookDirection, attackPushStrength));    
    }

    IEnumerator ApplyPlayerAttackToFendCoroutine(int attack, Vector2 playerLookDirection, float attackPushStrength)

    {
        var stepBackPos = new Vector3 (enemy.enemyFightingPosition.transform.position.x + (attackPushStrength * playerLookDirection.x),enemy.enemyFightingPosition.transform.position.y);

        if (combatManager.enemy[combatManager.selectedEnemy].fendTotal == 0)
        {
            FendBreached();
            yield return new WaitForSeconds(0.2f);
            yield return (combatManager.combatMovement.MoveCombatantFixedTime(enemy.gameObject, stepBackPos, isReversing: true));

            yield return null;
        }

        float elapsedTime = 0f;
        float lerpDuration = 0.5f;

        int startNumber = combatManager.enemy[combatManager.selectedEnemy].fendTotal;

        int endValue = combatManager.enemy[combatManager.selectedEnemy].fendTotal - attack;

        while (elapsedTime < lerpDuration && combatManager.enemy[combatManager.selectedEnemy].fendTotal > 0)
        {
            float t = Mathf.Clamp01(elapsedTime / lerpDuration);

            combatManager.enemy[combatManager.selectedEnemy].fendTotal = Mathf.RoundToInt(Mathf.Lerp(startNumber, endValue, t));
            fendTextMeshProUGUI.text = combatManager.enemy[combatManager.selectedEnemy].fendTotal.ToString();

            elapsedTime += Time.deltaTime;

            if (combatManager.enemy[combatManager.selectedEnemy].fendTotal == 0)
            {
               FendBreached();
               yield return new WaitForSeconds(0.2f);

                var combatMovementInstanceGO = Instantiate(combatManager.combatMovementPrefab, this.transform);
                var combatMovementInstance = combatMovementInstanceGO.GetComponent<CombatMovement>(); 
                yield return (combatMovementInstance.MoveCombatantFixedTime(enemy.gameObject, stepBackPos));
                Destroy(combatMovementInstanceGO);

               yield return null;
            }

            yield return null;
        }
    }

    void FendBreached()

    {
        enemyFendAnimator.SetTrigger("fendBreak");
        fendTextMeshProUGUI.text = "";
        if (attackRemainder > 0)
        {
            enemy.DamageTaken(attackRemainder, combatManager.selectedPlayerMove.damageToPartsMultiplier);
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
            enemyFendAnimator.SetTrigger("fendAppear");
        }

        else
        {
            fendGameObject.SetActive(false);
        }
    }

    public void ResetAllAnimationTriggers()

    {
        enemyFendAnimator.ResetTrigger("fendAppear");
        enemyFendAnimator.ResetTrigger("fendDeflect");
        enemyFendAnimator.ResetTrigger("fendBreak");
        enemyFendAnimator.ResetTrigger("fendFade");
    }
}
