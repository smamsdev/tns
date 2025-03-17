using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyStatsDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI combatantNameTextMeshPro;
    [SerializeField] TextMeshProUGUI enemyHPTextMeshPro;
    public GameObject enemyStatsDisplayGameObject;
    [SerializeField] Animator animator;
    Enemy enemy;
    int enemyHP;

    public void ShowEnemyStatsDisplay(bool on)

    { 
     if (on) { enemyStatsDisplayGameObject.SetActive(true); }
     if (!on) { enemyStatsDisplayGameObject.SetActive(false);}  
    }

    public void InitializeEnemyStatsUI(Enemy _enemy)
    {
        enemy = _enemy;
        combatantNameTextMeshPro.text = enemy.combatantName;
        enemyHP = enemy.currentHP;
        enemyHPTextMeshPro.text = "HP: " + enemyHP;
    }

    public void UpdateEnemyHPDisplay(int value)
    {
        if (enemy.currentHP <= 0) 
        {
            enemyHPTextMeshPro.text = "DEAD";
        }

        else
        {
            StartCoroutine(UpdateEnemyHPDisplayCoroutine(enemyHP, value));
        }
    }

    IEnumerator UpdateEnemyHPDisplayCoroutine(int enemyHP, int value)

    {
        animator.SetTrigger("bump");
        var newHPValue = enemyHP + value;

        float elapsedTime = 0f;
        float lerpDuration = 1f;
        int valueToOutput;

        while (elapsedTime < lerpDuration)
        {
            float t = Mathf.Clamp01(elapsedTime / lerpDuration);

            valueToOutput = Mathf.RoundToInt(Mathf.Lerp(enemyHP, newHPValue, t));
            enemyHPTextMeshPro.text = "HP: " + valueToOutput.ToString();
            enemyHP = valueToOutput;

            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }

}
