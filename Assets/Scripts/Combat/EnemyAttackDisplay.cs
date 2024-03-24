using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyAttackDisplay : MonoBehaviour
{
    [SerializeField]  TextMeshProUGUI EnemyAttackDamageTextMeshProUI;
    [SerializeField] GameObject attackDisplayTextGO;

    private void OnEnable()
    {
        CombatEvents.UpdateEnemyAttackDisplay += UpdateEnemyAttackDisplay;
        attackDisplayTextGO.SetActive(false);
    }

    private void OnDisable()
    {
        CombatEvents.UpdateEnemyAttackDisplay -= UpdateEnemyAttackDisplay;
    }

    public void UpdateEnemyAttackDisplay(int value)

    { 
        if (value >= 0) 
        {
            attackDisplayTextGO.SetActive(false);
        }

        attackDisplayTextGO.SetActive(true);
        EnemyAttackDamageTextMeshProUI.text = value.ToString();
    }

    public void ShowAttackDisplay(bool on)

    { 
        if (on) { attackDisplayTextGO.SetActive(true); }
        if (!on) { attackDisplayTextGO.SetActive(false); }
    }
}
