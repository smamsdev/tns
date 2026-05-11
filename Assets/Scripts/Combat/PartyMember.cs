using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberCombat : Ally
{
    public PartyMemberPermanentStats partyMemberPermanentStats;
    public Texture portraitImage;

    private void OnEnable()
    {
        movementScript = GetComponent<MovementScript>();
    }
}
