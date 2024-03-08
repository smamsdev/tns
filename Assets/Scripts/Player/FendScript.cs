using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FendScript : MonoBehaviour
{
    TextMeshProUGUI fendTextMeshProUGUI;
    [SerializeField] CombatManager combatManager;
    [SerializeField] GameObject fendTextObj;
    [SerializeField] GameObject fendIcon;
    [SerializeField] Animator animator;

    public int fend = 0;

    private void OnEnable()
    {
        CombatEvents.BattleMode += Init;
        CombatEvents.AnimatorTrigger += TriggerAnimation;
        CombatEvents.UpdateFendDisplay += UpdateFendText;
        CombatEvents.ATTACKTOAPPLY += EnemyAttack;
        CombatEvents.ShowHideFendDisplay += ShowHideFendDisplay;
    }

    private void OnDisable()
    {
        CombatEvents.BattleMode -= Init;
        CombatEvents.AnimatorTrigger -= TriggerAnimation;
        CombatEvents.ATTACKTOAPPLY -= EnemyAttack;
        CombatEvents.UpdateFendDisplay += UpdateFendText;
        CombatEvents.ShowHideFendDisplay -= ShowHideFendDisplay;
    }

    void Init(bool on)
    {
        ShowHideFendDisplay(false);
        fendTextMeshProUGUI = fendTextObj.GetComponent<TextMeshProUGUI>();
        this.transform.position = new Vector2(combatManager.battleScheme.playerFightingPosition.transform.position.x, combatManager.battleScheme.playerFightingPosition.transform.position.y + 0.8f);
    }

    void TriggerAnimation(string trigger)

    {
        animator.SetTrigger(trigger);
    }

    void UpdateFendText(int value)
    {
        if (value <= 0) { fendTextMeshProUGUI.text = "SMASHED!"; }

        else
        {
            fend = value;
            fendTextMeshProUGUI.text = fend.ToString();
        }
    }

    void ShowHideFendDisplay(bool on)

    {
        if (on && (combatManager.playerMoveManager.firstMoveIs == 2 || combatManager.playerMoveManager.secondMoveIs == 2))

        {
            fendTextObj.SetActive(true);
            fendIcon.SetActive(true);
        }

        if (!on)
        {
            fendTextObj.SetActive(false);
            fendIcon.SetActive(false);
        }
    }

    void EnemyAttack(int attack)

    {
    StartCoroutine(EnemyAttackCoRo(attack));
    
    }

    IEnumerator EnemyAttackCoRo(int attack)

    {
        float elapsedTime = 0f;
        float lerpDuration = 0.5f;

        int startNumber = fend;

        int endValue = fend - attack;

        while (elapsedTime < lerpDuration)
        {
            // Calculate the interpolation factor between 0 and 1 based on the elapsed time and duration
            float t = Mathf.Clamp01(elapsedTime / lerpDuration);

            // Lerp between the start and end values
            fend = Mathf.RoundToInt(Mathf.Lerp(startNumber, endValue, t));
            fendTextMeshProUGUI.text = fend.ToString();

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            yield return null;
        }

    }


}
