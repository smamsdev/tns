using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMemberCombat : Ally
{
    public PartyMemberSO partyMemberSO;
    //
    private void OnEnable()
    {
        movementScript = GetComponent<MovementScript>();
    }
}
