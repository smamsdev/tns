using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnemySelectButtonScript : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public SelectEnemyMenuScript selectEnemyMenuScript;
    public TextMeshProUGUI buttonText;
    public Combatant combatant;
    public Button button;

    public void OnSelect(BaseEventData eventData)
    {
        selectEnemyMenuScript.HighlightEnemy(this);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selectEnemyMenuScript.DeselectEnemy(this);
    }

    public void OnButtonSelected()
    {
        selectEnemyMenuScript.combatManager.enemySelectState.CombatantSelected(this);
    }
}
