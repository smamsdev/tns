using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyFendScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI fendTextMeshProUGUI;
    [SerializeField] CombatManager combatManager;
    [SerializeField] GameObject enemyFendContainer;
    [SerializeField] GameObject fendIcon;
    public Animator animatorContainer;
    [SerializeField] Animator iconAnimator;

    public int attackRemainder;

    private void OnEnable()
    {
        CombatEvents.BattleMode += Init;
        CombatEvents.UpdateEnemyFendDisplay += UpdateFendDisplay;
    }

    private void OnDisable()
    {
        CombatEvents.BattleMode -= Init;
        CombatEvents.UpdateEnemyFendDisplay -= UpdateFendDisplay;
    }

    void Init(bool on)
    {
        enemyFendContainer.transform.position = new Vector2(combatManager.battleScheme.enemyFightingPosition.transform.position.x, combatManager.battleScheme.enemyFightingPosition.transform.position.y + 0.8f);
    }

    public void FendIconAnimationState(int state)

    {
        animatorContainer.SetInteger("deflectState", state);
    }

    public void ApplyPlayerAttackToFend(int attack)

    {
        attackRemainder = attack - combatManager.enemy.fend;

        if (combatManager.enemy.fend == 0)
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
        FendIconAnimationState(1);

        float elapsedTime = 0f;
        float lerpDuration = 0.5f;

        int startNumber = combatManager.enemy.fend;

        int endValue = combatManager.enemy.fend - attack;

        while (elapsedTime < lerpDuration && combatManager.enemy.fend > 0)
        {
            float t = Mathf.Clamp01(elapsedTime / lerpDuration);

            combatManager.enemy.fend = Mathf.RoundToInt(Mathf.Lerp(startNumber, endValue, t));
            fendTextMeshProUGUI.text = combatManager.enemy.fend.ToString();

            elapsedTime += Time.deltaTime;

            if (combatManager.enemy.fend == 0)
            {
               FendBreached();
               yield return null;
            }

            yield return null;
        }
    }

    void FendBreached()

    {
        iconAnimator.SetBool("fendbreak", true);
        fendTextMeshProUGUI.text = "";
        if (attackRemainder > 0)
        {
            combatManager.enemy.DamageTaken(attackRemainder);
        }
    }

    public void ShowHideFendDisplay(bool on)

    {
        if (on)

        {
            fendIcon.SetActive(true);
        }

        if (!on)
        {
            fendIcon.SetActive(false);
        }
    }

    public void UpdateFendDisplay(int fend)
    {
        if (fend > 0)
        {
            fendTextMeshProUGUI.text = fend.ToString();
            ShowHideFendDisplay(true);
        }
    }
}
