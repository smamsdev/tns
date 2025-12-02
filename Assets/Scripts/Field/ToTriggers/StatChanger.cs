using System.Collections;
using UnityEngine;
using static Shift;

public class StatChanger : ToTrigger
{
    [SerializeField] StatToChange statType;
    [SerializeField] int value;
    public PartyMemberSO optionalPartyMemberStats;

    public enum StatToChange
    {
        Smams,
        MaxPotential,
        FocusBase,
        AttackBase,
        FendBase,
        MaxHP,
        XP
    }

    public override IEnumerator TriggerFunction()
    {
        if (optionalPartyMemberStats == null)
        {
            optionalPartyMemberStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>().playerPermanentStats;
        }

        PlayerPermanentStats playerPermanentStats = optionalPartyMemberStats as PlayerPermanentStats;

        switch (statType)
        {
            case StatToChange.AttackBase:
                optionalPartyMemberStats.AttackBase += value;
                break;
            case StatToChange.FendBase:
                optionalPartyMemberStats.FendBase += value;
                break;
            case StatToChange.MaxHP:
                optionalPartyMemberStats.MaxHP += value;
                break;
            case StatToChange.XP:
                optionalPartyMemberStats.XP += value;
                break;

            // Player stats
            case StatToChange.Smams:
                playerPermanentStats.Smams += value;
                break;
            case StatToChange.MaxPotential:
                playerPermanentStats.MaxPotential += value;
                break;
            case StatToChange.FocusBase:
                playerPermanentStats.FocusBase += value;
                break;

            default:
                throw new System.ArgumentOutOfRangeException();
        }

        yield return null;
    }
}
