using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnemySelectButtonScript : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public EnemySelectMenuUI enemySelectMenuUI;
    public TextMeshProUGUI buttonText;
    public Combatant combatant;
    public Button button;

    public void OnSelect(BaseEventData eventData)
    {
        enemySelectMenuUI.HighlightEnemy(this);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        enemySelectMenuUI.DeselectEnemy(this);
    }

    public void OnButtonSelected()
    {
        enemySelectMenuUI.combatManager.enemySelectState.CombatantSelected(this);
    }
}
