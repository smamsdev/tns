using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberCombat : Ally
{
    public PartyMemberSO partyMemberSO;
    public  Texture portraitImage;

    private void OnEnable()
    {
        movementScript = GetComponent<MovementScript>();
    }
}
