using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class ShowHeadAttackInfo : MonoBehaviour, ISelectHandler
{
    [SerializeField] CombatManager combatManager;

    public void OnSelect(BaseEventData eventData)
    {
        combatManager.enemies[combatManager.selectedEnemy].enemyUI.partsTargetDisplay.UpdateTargetDisplay(false, false, true);
    }
}
