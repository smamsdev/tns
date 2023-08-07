using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyStatsDisplay : MonoBehaviour
{

    TextMeshPro textMeshProHP;
    public int enemyHP;

    private void Awake()
    {
        textMeshProHP = GetComponent<TextMeshPro>();
    }

    private void OnEnable()
    {
        CombatEvents.UpdateEnemyHP += UpdateEnemyHPText;
        CombatEvents.InitializeEnemyHP += InitializeEnemyHP;

    }

    private void OnDisable()
    {
        CombatEvents.UpdateEnemyHP -= UpdateEnemyHPText;
        CombatEvents.InitializeEnemyHP += InitializeEnemyHP;
    }

    void InitializeEnemyHP(int value)

    { enemyHP = value;
        textMeshProHP.text = "Enemy HP: " + enemyHP;
    }

    public void UpdateEnemyHPText(int value)
    {
        enemyHP -= value;
        textMeshProHP.text = "Enemy HP: " + (enemyHP).ToString();
    }

}
