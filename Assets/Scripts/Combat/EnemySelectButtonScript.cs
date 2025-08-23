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
        selectEnemyMenuScript.HighlightEnemy(combatant);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selectEnemyMenuScript.DeselectEnemy(combatant);
    }

    private void OnEnable()
    {
        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        selectEnemyMenuScript.combatManager.enemySelectState.CombatantSelected(combatant);
    }
}
