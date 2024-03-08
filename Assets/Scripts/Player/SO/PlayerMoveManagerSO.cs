using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

[CreateAssetMenu]

public class PlayerMoveManagerSO : ScriptableObject
{
    public int firstMoveIs;
    public int secondMoveIs;

    public string moveForNarrator;


    public void CombineMoves()

    {
        if (firstMoveIs == 1 && secondMoveIs == 1)
        {
            OneOne();
        }

        if (firstMoveIs == 1 && secondMoveIs == 2)
        {
            OneTwo();
        }

        if (firstMoveIs == 1 && secondMoveIs == 3)
        {
            OneThree();
        }

        if (firstMoveIs == 2 && secondMoveIs == 1)
        {
            TwoOne();
        }

        if (firstMoveIs == 2 && secondMoveIs == 2)
        {
            TwoTwo();
        }

        if (firstMoveIs == 2 && secondMoveIs == 3)
        {
            TwoThree();
        }

        if (firstMoveIs == 3 && secondMoveIs == 1)
        {
            ThreeOne();
        }

        if (firstMoveIs == 3 && secondMoveIs == 2)
        {
            ThreeTwo();
        }

        if (firstMoveIs == 3 && secondMoveIs == 3)
        {
            ThreeThree();
        }
    }

    void OneOne()

    {
        //ui
        moveForNarrator = "Reckless Attack!";

        //pot
        CombatEvents.UpdatePlayerPotentialMoveCost.Invoke(0.2f, -20, false);

        //att
        CombatEvents.UpdatePlayerAttackMoveMod.Invoke(1, true);

        //def
        CombatEvents.UpdatePlayerFendMoveMod.Invoke(0, false);
    }

    void OneTwo()

    {
        //ui
        moveForNarrator = "Slip Attack!";

        //att
        CombatEvents.UpdatePlayerAttackMoveMod.Invoke(0.3f, true);

        //def 
        CombatEvents.UpdatePlayerFendMoveMod.Invoke(0.3f, true);

        //pot null
        CombatEvents.UpdatePlayerPotentialMoveCost.Invoke(0.3f, -10, false);
    }

    void OneThree()

    {
        //ui
        moveForNarrator = "Calculating Attack";

        //att
        CombatEvents.UpdatePlayerAttackMoveMod.Invoke(0.2f, true);

        //def
        CombatEvents.UpdatePlayerFendMoveMod.Invoke(0, false);

        //pot
        CombatEvents.UpdatePlayerPotentialMoveCost.Invoke(0.8f, 0, true);
    }

    void TwoOne()

    {
        //ui
        moveForNarrator = "Counter Attack!";

        //att
        CombatEvents.UpdatePlayerAttackMoveMod.Invoke(0.3f, true);

        //def
        CombatEvents.UpdatePlayerFendMoveMod.Invoke(0.5f, true);
        //fendScript.ShowFendText();

        //pot
        CombatEvents.UpdatePlayerPotentialMoveCost.Invoke(0.4f, -10, false);
    }

    void TwoTwo()

    {
        //ui
        moveForNarrator = "Stubborn Fend";

        //att null
        CombatEvents.UpdatePlayerAttackMoveMod.Invoke(0, false);

        //def
        CombatEvents.UpdatePlayerFendMoveMod.Invoke(1, true);

        //pot
        CombatEvents.UpdatePlayerPotentialMoveCost.Invoke(0.2f, -15, false);
    }

    void TwoThree()

    {
        //ui
        moveForNarrator = "Pensive Fend!";

        //att null
        CombatEvents.UpdatePlayerAttackMoveMod.Invoke(0, false);

        //def
        CombatEvents.UpdatePlayerFendMoveMod.Invoke(0.5f, true);

        //pot
        CombatEvents.UpdatePlayerPotentialMoveCost.Invoke(0.3f, 0, true);
    }

    void ThreeOne()

    {
        //ui
        moveForNarrator = "Precise Attack!";

        //att
        CombatEvents.UpdatePlayerAttackMoveMod.Invoke(0.4f, true);

        //def
        CombatEvents.UpdatePlayerFendMoveMod.Invoke(0, false);

        //pot
        CombatEvents.UpdatePlayerPotentialMoveCost.Invoke(0.2f, 2, true);
    }

    void ThreeTwo()

    {
        //ui
        moveForNarrator = "Contrary Fend!";

        //att null
        CombatEvents.UpdatePlayerAttackMoveMod.Invoke(0, false);

        //def
        CombatEvents.UpdatePlayerFendMoveMod.Invoke(0.2f, true);

        //pot
        CombatEvents.UpdatePlayerPotentialMoveCost.Invoke(0.4f, 2, true);
    }

    void ThreeThree()

    {
        //ui
        moveForNarrator = "Zen Concentration!";

        //att null
        CombatEvents.UpdatePlayerAttackMoveMod.Invoke(0, false);

        //def
        CombatEvents.UpdatePlayerFendMoveMod.Invoke(0, false);

        //pot
        CombatEvents.UpdatePlayerPotentialMoveCost.Invoke(1.2f, 10, true);
    }

    public void GearMove()

    {
        if (firstMoveIs == 0 && secondMoveIs == 0)

        moveForNarrator = "";
        GearMove();
    }

    public void FirstMoveIsAttack()

    {
        CombatEvents.UpdateFirstMoveDisplay.Invoke("Attack");
        firstMoveIs = 1;
    }

    public void FirstMoveIsDefend()

    {
        CombatEvents.UpdateFirstMoveDisplay.Invoke("Defend");
        firstMoveIs = 2;
    }

    public void FirstMoveIsFocus()

    {
        CombatEvents.UpdateFirstMoveDisplay.Invoke("Focus");
        firstMoveIs = 3;
    }

    public void SecondMoveIsAttack()

    {
        CombatEvents.UpdateSecondMoveDisplay.Invoke("Attack");
        secondMoveIs = 1;
    }


    public void SecondMoveIsDefend()

    {
        CombatEvents.UpdateSecondMoveDisplay.Invoke("Defend");
        secondMoveIs = 2;
    }

    public void SecondMoveIsFocus()

    {
        CombatEvents.UpdateSecondMoveDisplay.Invoke("Focus");
        secondMoveIs = 3;
    }


}
