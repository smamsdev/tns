using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class PartySO : ScriptableObject
{
    public List<PartyMemberPermanentStats> partyMembers = new List<PartyMemberPermanentStats>();
}