using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class VictoryRewards : MonoBehaviour
{
    public CombatManager combatManager;
    public GridLayoutGroup totalXPgridLayout;
    public TextMeshProUGUI[] totalXPTextElements;
    public Button totalXPButton;
    public int partyToLoop = 0;

    public TextMeshProUGUI XPValue;

    TextMeshProUGUI FindLongestText()
    {
        return totalXPTextElements.OrderByDescending(text => text.preferredWidth).First();
    }

    public void ShowTotalXP()
    {
        partyToLoop = 0;
        float preferredWidth = FindLongestText().preferredWidth;
        Vector2 newCellSize = totalXPgridLayout.cellSize;
        newCellSize.x = preferredWidth;
        totalXPgridLayout.cellSize = newCellSize;
        totalXPButton.Select();
    }

    public void CyclePartyMemberXPGain()
    {
        UpdateXPValue();

        if (partyToLoop < combatManager.allAlliesToTarget.Count)
        {
            if (!combatManager.allAlliesToTarget[partyToLoop].isEncounterSpawned)
            {
                Debug.Log(combatManager.allAlliesToTarget[partyToLoop].combatantName);
                combatManager.cameraFollow.transformToFollow = combatManager.allAlliesToTarget[partyToLoop].transform;
            }

            partyToLoop++;
        }

        else 
        {
        //close box
        }
    }

    void UpdateXPValue()
    {
        FieldEvents.LerpValues(0, 100, 1, UpdateXPValueTMP);
    }

    void UpdateXPValueTMP(int value)
    {
        XPValue.text = value.ToString();
    }
}
