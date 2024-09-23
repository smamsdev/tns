using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyFendScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI fendTextMeshProUGUI;
    [SerializeField] CombatManager combatManager;
    [SerializeField] GameObject fendGameObject;
    [SerializeField] GameObject fendTextGameObject;
    public Animator enemyFendAnimator;

    int attackRemainder;

    private void OnEnable()
    {
        combatManager = GetComponentInParent<CombatManager>();
    }

    public void ApplyPlayerAttackToFend(int attack)

    {
        attackRemainder = attack - combatManager.enemy[combatManager.selectedEnemy].fendTotal;
        enemyFendAnimator.SetTrigger("fendDeflect");

        if (combatManager.enemy[combatManager.selectedEnemy].fendTotal == 0)
        {
            FendBreached();
            return;
        }

        else
        {
            StartCoroutine(ApplyPlayerAttackToFendCoroutine(attack));
        }   
    }

    IEnumerator ApplyPlayerAttackToFendCoroutine(int attack)

    {
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
               yield return null;
            }

            yield return null;
        }
    }

    void FendBreached()

    {
        var enemy = combatManager.enemy[combatManager.selectedEnemy];
        var enemyMovementScript = enemy.GetComponent<ActorMovementScript>();

        enemyMovementScript.actorRigidBody2d.bodyType = RigidbodyType2D.Dynamic;

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
