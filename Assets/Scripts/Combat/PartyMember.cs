using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMember : Ally
{
    public AllyPermanentStats partyMemberPermanentStats;

    private void OnEnable()
    {
        movementScript = GetComponent<MovementScript>();
    }

    public override void InitialiseCombatantStats()
    {
        CurrentHP = partyMemberPermanentStats.currentHP;
    }
}
