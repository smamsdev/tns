using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyChanger : ToTrigger
{

    public int moneyChange;

    public override IEnumerator TriggerFunction()
    {
        PlayerPermanentStats playerPermanentStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>().playerPermanentStats;

        playerPermanentStats.smams += moneyChange;
        FieldEvents.HasCompleted.Invoke(this.gameObject);

        yield return null;
    }
}

