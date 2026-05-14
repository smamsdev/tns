using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ApplyPlayerMove : State
{
    PlayerCombat playerCombat;

    public override IEnumerator StartState()
    {
        combatManager.combatMenuManager.DisableAllMenus();
        playerCombat = combatManager.playerCombat;

        yield return ApplyGearEffect();
        yield return ApplyMove();

        if (combatManager.enemies.Count == 0)
        {
            combatManager.SetState(combatManager.victory);
            yield break;
        }

        if (combatManager.allies.Count > 0 && combatManager.enemies.Count > 0)
        {
            combatManager.SetState(combatManager.allyMoveState);
            yield break;
        }

        else
        {
            combatManager.SetState(combatManager.enemyMoveState);
            yield break;
        }
    }

    IEnumerator ApplyGearEffect()
    {
        foreach (GearMonoBehaviour gearMonoBehaviour in playerCombat.gearBehaviours)
        {
            gearMonoBehaviour.ApplyGearEffect();
        }

        yield return null;
    }

    IEnumerator ApplyMove()
    {
        //reset narrator focus camera on allyToAct and wait
        combatManager.cameraFollow.transformToFollow = playerCombat.transform;
        combatManager.combatMenuManager.UpdateNarrator(playerCombat.moveSOSelected.MoveName);

        ApplyPotentialChange();
        yield return new WaitForSeconds(1f);
        combatManager.combatMenuManager.UpdateNarrator("");

        //rock out
        yield return playerCombat.currentMoveBehaviour.ApplyMove(playerCombat, playerCombat.targetCombatant);
        combatManager.combatMenuManager.UpdateNarrator("");
    }

    void ApplyPotentialChange()
    {
        CombatEvents.UpdatePlayerPot.Invoke(combatManager.playerCombat.currentMoveBehaviour.CalculateAndReturnPotentialChange());
    }
}