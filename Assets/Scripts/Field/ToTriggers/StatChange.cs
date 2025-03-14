using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatChange : ToTrigger
{
    public int fendBaseStatChange;
    public int attackBaseStatChange;
    public int focusBaseStatChange;
    public int maxPotentialStatChange;
    public int maxHpStatChange;

    PlayerPermanentStats permanentStatsSO;

    public override IEnumerator DoAction()
    {
        permanentStatsSO = GameObject.FindWithTag("CombatManager").GetComponent<PlayerCombat>().playerPermanentStats;

        permanentStatsSO.fendBase += fendBaseStatChange;
        permanentStatsSO.attackPowerBase += attackBaseStatChange;
        permanentStatsSO.playerFocusbase += focusBaseStatChange;
        permanentStatsSO.maxPotential += maxPotentialStatChange;
        permanentStatsSO.maxHP += maxHpStatChange;

        FieldEvents.HasCompleted.Invoke(this.gameObject);

        yield return null;
    }
}
