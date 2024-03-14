using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyStatsDisplay : MonoBehaviour
{
    [SerializeField] CombatManager combatManager;
    [SerializeField] TextMeshProUGUI enemyHPtextMeshProHP;
    [SerializeField] TextMeshProUGUI enemyNameMeshProHP;
    [SerializeField] GameObject enemyStatsDisplay;
    [SerializeField] RectTransform enemyHudRect;
    [SerializeField] Animator animator;
    Enemy enemy;
    int enemyHP;

    private void OnEnable()
    {
        CombatEvents.UpdateEnemyHPUI += UpdateEnemyHPDisplay;
        CombatEvents.InitializeEnemyHP += InitializeEnemyHP;
    }

    private void OnDisable()
    {
        CombatEvents.UpdateEnemyHPUI -= UpdateEnemyHPDisplay;
        CombatEvents.InitializeEnemyHP += InitializeEnemyHP;
    }

    void InitializeEnemyHP()
    {
        enemyStatsDisplay.SetActive(true);
        enemyHudRect.transform.position = combatManager.battleScheme.enemyFightingPosition.transform.position;

        enemy = combatManager.battleScheme.enemyGameObject.GetComponent<Enemy>();

        enemyNameMeshProHP.text = enemy.name;
        enemyHP = enemy.enemyHP; 
        enemyHPtextMeshProHP.text = "HP: " + enemyHP;
    }

    public void UpdateEnemyHPDisplay(int newHPValue)
    {
        if (enemy.enemyHP <= 0) 
        {
            enemyHPtextMeshProHP.text = "DEAD";
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
            enemyHPtextMeshProHP.text = "HP: " + valueToOutput.ToString();
            enemyHP = valueToOutput;

            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }

}
