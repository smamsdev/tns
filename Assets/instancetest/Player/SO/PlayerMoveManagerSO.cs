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
        //attack first

        if (firstMoveIs == 1 && secondMoveIs == 1)
        {

            //ui
            moveForNarrator = "Reckless Attack!";

            //pot
            CombatEvents.UpdatePlayerPotentialMoveCost.Invoke(0.2f,-20, false);


            //att
            CombatEvents.UpdatePlayerAttackMoveMod.Invoke(1, true);


            //def
            CombatEvents.UpdatePlayerFendMoveMod.Invoke(0, false);

        }

        if (firstMoveIs == 1 && secondMoveIs == 2)
        {

            //ui
            moveForNarrator = "Slip Attack!";

            //att
            CombatEvents.UpdatePlayerAttackMoveMod.Invoke(0.3f, true);


            //def 
            CombatEvents.UpdatePlayerFendMoveMod.Invoke(0.3f, true);

            // fendScript.ShowFendText();

            //pot null
            CombatEvents.UpdatePlayerPotentialMoveCost.Invoke(0.3f, -10, false);



        }

        if (firstMoveIs == 1 && secondMoveIs == 3)
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

        //def first

        if (firstMoveIs == 2 && secondMoveIs == 1)
        {

            //ui
            moveForNarrator = "Counter Attack!";


            //att
            CombatEvents.UpdatePlayerAttackMoveMod.Invoke(0.3f, true) ;

            //def
            CombatEvents.UpdatePlayerFendMoveMod.Invoke(0.5f, true);
            //fendScript.ShowFendText();

            //pot
            CombatEvents.UpdatePlayerPotentialMoveCost.Invoke(0.4f, -10, false);
        }

        if (firstMoveIs == 2 && secondMoveIs == 2)
        {
            //ui
            moveForNarrator = "Stubborn Fend";

            //att null
            CombatEvents.UpdatePlayerAttackMoveMod.Invoke(0,false);

            //def
            CombatEvents.UpdatePlayerFendMoveMod.Invoke(1, true);

            //  fendScript.ShowFendText();

            //pot
            CombatEvents.UpdatePlayerPotentialMoveCost.Invoke(0.2f, -15, false);
        }

        if (firstMoveIs == 2 && secondMoveIs == 3)
        {
            //ui
            moveForNarrator = "Pensive Fend!";

            //att null
            CombatEvents.UpdatePlayerAttackMoveMod.Invoke(0,false);

            //def
            CombatEvents.UpdatePlayerFendMoveMod.Invoke(0.5f, true);
            // fendScript.ShowFendText();
            // playerStats.fendMoveMod = Mathf.CeilToInt(playerStats.playerFendBase * 0.4f);

            //pot
            CombatEvents.UpdatePlayerPotentialMoveCost.Invoke(0.3f, 0, true);
        }

        //focus first

        if (firstMoveIs == 3 && secondMoveIs == 1)
        {
            //ui
            moveForNarrator = "Precise Attack!";

            //att
            CombatEvents.UpdatePlayerAttackMoveMod.Invoke(0.4f,true);

            //def
            CombatEvents.UpdatePlayerFendMoveMod.Invoke(0, false);

            //pot
            CombatEvents.UpdatePlayerPotentialMoveCost.Invoke(0.2f, 2, true);
        }

        if (firstMoveIs == 3 && secondMoveIs == 2)
        {
            //ui
            moveForNarrator = "Contrary Fend!";

            //att null
            CombatEvents.UpdatePlayerAttackMoveMod.Invoke(0,false);

            //def
            CombatEvents.UpdatePlayerFendMoveMod.Invoke(0.2f, true);
            // fendScript.ShowFendText();

            //pot
            CombatEvents.UpdatePlayerPotentialMoveCost.Invoke(0.4f, 2, true);
        }


        if (firstMoveIs == 3 && secondMoveIs == 3)
        {
            //ui
            moveForNarrator = "Zen Concentration!";

            //att null
            CombatEvents.UpdatePlayerAttackMoveMod.Invoke(0,false);

            //def
            CombatEvents.UpdatePlayerFendMoveMod.Invoke(0, false);

            //pot
            CombatEvents.UpdatePlayerPotentialMoveCost.Invoke(1.2f, 10, true);
        }

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

    //second move
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
