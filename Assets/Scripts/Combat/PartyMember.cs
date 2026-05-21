using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static StatModifier;
using static UnityEngine.Rendering.DebugUI;

public class PartyMemberCombat : Ally
{
    public PartyMemberPermanentStats partyMemberPermanentStats;
    public Sprite portraitImage;

    private void OnEnable()
    {
        movementScript = GetComponent<MovementScript>();
    }

    public override void ChangeStat(StatModifier mod)
    {
//
    }
}
