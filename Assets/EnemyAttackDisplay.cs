using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyAttackDisplay : MonoBehaviour
{
    [SerializeField] TextMeshPro textMeshPro;

    private void OnEnable()
    {
        CombatEvents.EnemyAttackPower += UpdateAttackDisplay;
    }

    private void OnDisable()
    {
        CombatEvents.EnemyAttackPower -= UpdateAttackDisplay;
    }

    void UpdateAttackDisplay(int value)

    { textMeshPro.text = value.ToString(); }
}
