using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ShowBodyAttackInfo : MonoBehaviour, ISelectHandler
{
    [SerializeField] CombatManager combatManager;

    public void OnSelect(BaseEventData eventData)
    {
        combatManager.enemy[combatManager.selectedEnemy].enemyUI.partsTargetDisplay.UpdateTargetDisplay(true, false, false);
    }
}
