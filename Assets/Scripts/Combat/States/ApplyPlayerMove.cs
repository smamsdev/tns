using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ApplyPlayerMove : State
{
    PlayerCombat playerCombat;
    public string dynamicMoveName;

    public override IEnumerator StartState()
    {
        playerCombat = combatManager.playerCombat;

        combatManager.combatMenuManager.DisableAllMenus();
        playerCombat.currentMoveBehaviour.LoadMoveReferences(playerCombat, combatManager);
        playerCombat.currentMoveBehaviour.CalculateMoveStats();

        yield return GearEffectOnTurn();
        yield return ApplyMove();

       // if (combatManager.enemies.Count == 0)
       // {
       //     combatManager.SetState(combatManager.victoryState);
       //     yield break;
       // }

        if (combatManager.allies.Count > 0)
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

    IEnumerator GearEffectOnTurn()
    {
        foreach (GearMonoBehaviour gearMonoBehaviour in playerCombat.gearBehaviours)
        {
            if (gearMonoBehaviour != null)
                yield return gearMonoBehaviour.GearEffectOnTurn();
        }

        yield return null;
    }

    IEnumerator ApplyMove()
    {
        //reset narrator focus camera on allyToAct and wait
        combatManager.cameraFollow.transformToFollow = playerCombat.transform;

        if (string.IsNullOrWhiteSpace(dynamicMoveName))
            combatManager.combatMenuManager.UpdateNarrator(playerCombat.currentMoveBehaviour.moveSO.MoveName);

        else
        {
            combatManager.combatMenuManager.UpdateNarrator(dynamicMoveName);
            dynamicMoveName = null;
        }

        ApplyPotentialChange();
        yield return new WaitForSeconds(1f);
        combatManager.combatMenuManager.UpdateNarrator("");

        //rock out
        yield return playerCombat.currentMoveBehaviour.ApplyMove(playerCombat, playerCombat.targetCombatant);
        combatManager.combatMenuManager.UpdateNarrator("");
    }

    void ApplyPotentialChange()
    {
        playerCombat.UpdatePlayerPot(playerCombat.currentMoveBehaviour.CalculateAndReturnPotentialChange());
    }
}