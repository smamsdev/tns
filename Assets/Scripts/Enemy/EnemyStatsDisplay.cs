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
    Enemy enemy;

    private void OnEnable()
    {
        CombatEvents.UpdateEnemyHPUI += UpdateenemyHPText;
        CombatEvents.InitializeEnemyHP += InitializeEnemyHP;
    }

    private void OnDisable()
    {
        CombatEvents.UpdateEnemyHPUI -= UpdateenemyHPText;
        CombatEvents.InitializeEnemyHP += InitializeEnemyHP;
    }

    void InitializeEnemyHP()
    {
        enemyStatsDisplay.SetActive(true);
        enemyHudRect.transform.position = combatManager.battleScheme.enemyFightingPosition.transform.position;

        enemy = combatManager.battleScheme.enemyGameObject.GetComponent<Enemy>();

        enemyNameMeshProHP.text = enemy.name;
        enemyHPtextMeshProHP.text = "HP: " + enemy.enemyHP;
    }

    public void UpdateenemyHPText(int value)
    {
        enemyHPtextMeshProHP.text = "Enemy HP: " + enemy.enemyHP;

        if (enemy.enemyHP <= 0) 
        {
            enemyHPtextMeshProHP.text = "DEAD";
        }

        else
        {
            enemyHPtextMeshProHP.text = "Enemy HP: " + enemy.enemyHP;
        }
    }

}
