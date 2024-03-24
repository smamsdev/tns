using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FendScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI fendTextMeshProUGUI;
    [SerializeField] CombatManager combatManager;
    [SerializeField] GameObject playerFendContainer;
    [SerializeField] GameObject playerFendIcon, playerFendText;
    public Animator animator;

    public int attackRemainder;

    public int fend = 0;

    private void Start()
    {
        ShowFendDisplay(false);

        playerFendContainer.transform.position = new Vector2(combatManager.battleScheme.playerFightingPosition.transform.position.x + 0.02f, combatManager.battleScheme.playerFightingPosition.transform.position.y + 0.8f);
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

    public void ApplyEnemyAttackToFend(int attack)

    {
        attackRemainder = attack - fend;
        animator.SetTrigger("fendDeflect");
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
            combatManager.combatUIScript.playerDamageTakenDisplay.ShowPlayerDamageDisplay(attackRemainder);
            CombatEvents.UpdatePlayerHP.Invoke(-attackRemainder);
        }
    }
    
}
