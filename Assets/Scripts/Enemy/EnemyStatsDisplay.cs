using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyStatsDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI enemyNameTextMeshPro;
    [SerializeField] TextMeshProUGUI enemyHPTextMeshPro;
    [SerializeField] GameObject enemyStatsDisplayGameObject;
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
        enemyNameTextMeshPro.text = enemy.name;
        enemyHP = enemy.enemyHP;
        enemyHPTextMeshPro.text = "HP: " + enemyHP;
    }

    public void UpdateEnemyHPDisplay(int newHPValue)
    {
        if (enemy.enemyHP <= 0) 
        {
            enemyHPTextMeshPro.text = "DEAD";
        }

        else
        {
            StartCoroutine(UpdateEnemyHPDisplayCoroutine(enemyHP, newHPValue));
        }
    }

    IEnumerator UpdateEnemyHPDisplayCoroutine(int _enemyHP, int newHPValue)

    {
        animator.SetTrigger("bump");

        float elapsedTime = 0f;
        float lerpDuration = 1f;
        int valueToOutput;

        while (elapsedTime < lerpDuration)
        {
            float t = Mathf.Clamp01(elapsedTime / lerpDuration);

            valueToOutput = Mathf.RoundToInt(Mathf.Lerp(_enemyHP, newHPValue, t));
            enemyHPTextMeshPro.text = "HP: " + valueToOutput.ToString();
            enemyHP = valueToOutput;

            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }

}
