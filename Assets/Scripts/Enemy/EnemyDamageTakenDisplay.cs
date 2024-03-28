using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyDamageTakenDisplay : MonoBehaviour
{

    [SerializeField] Animator animator;
    public TextMeshProUGUI EnemyDamageTakenTextMeshProUGUI;

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
            float t = Mathf.Clamp01(elapsedTime / lerpDuration);

            valueToDisplay = Mathf.RoundToInt(Mathf.Lerp(startNumber, endValue, t));
            EnemyDamageTakenTextMeshProUGUI.text = valueToDisplay.ToString();

            elapsedTime += Time.deltaTime;

            yield return null;
        }

    }

    public void DisableEnemyDamageDisplay()

    {
        animator.SetInteger("animState", 0);
        //EnemyDamageTakenTextMeshProUGUI.enabled = false;
    }

   }
