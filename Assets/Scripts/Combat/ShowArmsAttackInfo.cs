using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShowArmsAttackInfo : MonoBehaviour, ISelectHandler
{
    [SerializeField] CombatManager combatManager;

    public void OnSelect(BaseEventData eventData)
    {
        var targetUI = combatManager.playerCombat.targetToAttack.combatantUI as EnemyUI;
        targetUI.partsTargetDisplay.UpdateTargetDisplay(false, true, false);
    }
}

