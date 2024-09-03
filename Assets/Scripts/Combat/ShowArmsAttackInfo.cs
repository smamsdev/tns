using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShowArmsAttackInfo : MonoBehaviour, ISelectHandler
{
    [SerializeField] CombatManager combatManager;

    public void OnSelect(BaseEventData eventData)
    {
        combatManager.enemy[combatManager.selectedEnemy].enemyUI.partsTargetDisplay.UpdateTargetDisplay(false, true, false);
    }
}

