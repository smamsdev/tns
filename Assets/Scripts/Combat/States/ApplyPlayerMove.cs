using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ApplyPlayerMove : State
{
    PlayerCombat player;

    public override IEnumerator StartState()
    {
        combatManager.combatMenuManager.DisableMenuState();
        player = combatManager.playerCombat;

        yield return ApplyGear();
        yield return ApplyMove();

        if (combatManager.enemies.Count == 0)
        {
            combatManager.SetState(combatManager.victory);
            yield break;
        }

        if (player.FendTotal > 0)
        {
            player.combatantUI.fendScript.ShowFendDisplay(player, true);
            yield return new WaitForSeconds(1f);
            player.combatantUI.fendScript.ShowFendDisplay(player, false);
            yield return new WaitForSeconds(.2f);

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

    IEnumerator ApplyGear()
    {
        var equipped = player.playerInventory.inventorySO.equippedGear;
        for (int i = equipped.Count - 1; i >= 0; i--)
        {
            GearSO gearSO = equipped[i];
            if (gearSO == null) continue;

            yield return gearSO.gearInstance.ApplyGear();

            if (!gearSO.isConsumable)
            {
                gearSO.gearInstance.turnsUntilConsumed = -1;
            }

            else
            {
                gearSO.gearInstance.turnsUntilConsumed--;
            }

            if (gearSO.gearInstance.turnsUntilConsumed == 0)
            {
                player.playerInventory.DestroyGearInstance(gearSO);
                player.playerInventory.GearConsumed(gearSO);
            }
        }
    }

    IEnumerator ApplyMove()
    {
        //reset narrator focus camera on allyToAct and wait
        combatManager.cameraFollow.transformToFollow = player.transform;

        var moveSelected = combatManager.playerCombat.moveSelected;
        moveSelected.LoadMoveReferences(player, combatManager);
        CombatEvents.UpdateNarrator(moveSelected.moveSO.MoveName);

        ApplyPotentialChange();
        yield return new WaitForSeconds(0.5f);
        moveSelected.CalculateMoveStats();

        //rock out
        yield return moveSelected.ApplyMove(player, player.targetCombatant);
        CombatEvents.UpdateNarrator("");
    }

    void ApplyPotentialChange()
    {
        CombatEvents.UpdatePlayerPot.Invoke(combatManager.playerCombat.moveSelected.CalculateAndReturnPotentialChange());
    }
}