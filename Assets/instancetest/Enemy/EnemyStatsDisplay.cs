using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyStatsDisplay : MonoBehaviour
{

    TextMeshPro textMeshProHP;
    [HideInInspector] public int enemyHPUI;
    [SerializeField] GameObject enemyStats;

    private void Awake()
    {
        textMeshProHP = enemyStats.GetComponent<TextMeshPro>();
    }

    private void OnEnable()
    {
        CombatEvents.UpdateenemyHPUI += UpdateenemyHPText;
        CombatEvents.InitializeenemyHP += InitializeenemyHP;
    }

    private void OnDisable()
    {
        CombatEvents.UpdateenemyHPUI -= UpdateenemyHPText;
        CombatEvents.InitializeenemyHP += InitializeenemyHP;
    }

    void InitializeenemyHP(int value)
    {
        enemyHPUI = value;
        textMeshProHP.text = "Enemy HP: " + enemyHPUI;
    }

    public void UpdateenemyHPText(int value)
    {
        enemyHPUI = Mathf.Clamp(enemyHPUI - value, 0, 9999);

        if (enemyHPUI <= 0) 
        {
            textMeshProHP.text = "DEAD";
        }

        else
        {
            textMeshProHP.text = "Enemy HP: " + (enemyHPUI).ToString();
        }
    }

}
