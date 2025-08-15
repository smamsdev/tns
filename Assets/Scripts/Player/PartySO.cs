using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class PartySO : ScriptableObject
{
    public List<PartyMemberSO> partyMembers = new List<PartyMemberSO>();
}