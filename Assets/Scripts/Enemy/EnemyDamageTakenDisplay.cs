using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyDamageTakenDisplay : MonoBehaviour
{

    [SerializeField] Animator animator;
    public TextMeshProUGUI EnemyDamageTakenTextMeshProUGUI;

    private void OnEnable()
    {
        CombatEvents.UpdateEnemyHPUI += ShowEnemyDamageDisplay;
    }

    private void OnDisable()
    {
        CombatEvents.UpdateEnemyHPUI -= ShowEnemyDamageDisplay;
    }

    void Start()
    {
        EnemyDamageTakenTextMeshProUGUI.enabled = false;
    }

     public void ShowEnemyDamageDisplay(int remainder)
    {
        animator.SetInteger("animState", 1);
        EnemyDamageTakenTextMeshProUGUI.enabled = true;
        StartCoroutine(ShoweEnemyDamageDisplayCoroutine(remainder));
    }


    IEnumerator ShoweEnemyDamageDisplayCoroutine(int damage)

    {
        float elapsedTime = 0f;
        float lerpDuration = 0.5f;
        int startNumber = 1;
        int endValue = damage;
        int valueToDisplay;

        while (elapsedTime < lerpDuration)
        {
            // Calculate the interpolation factor between 0 and 1 based on the elapsed time and duration
            float t = Mathf.Clamp01(elapsedTime / lerpDuration);

            // Lerp between the start and end values
            valueToDisplay = Mathf.RoundToInt(Mathf.Lerp(startNumber, endValue, t));
            EnemyDamageTakenTextMeshProUGUI.text = valueToDisplay.ToString();

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            yield return null;
        }

    }


    public void DisableEnemyDamageDisplay()

    {
        animator.SetInteger("animState", 0);
        EnemyDamageTakenTextMeshProUGUI.enabled = false;
    }

   }
