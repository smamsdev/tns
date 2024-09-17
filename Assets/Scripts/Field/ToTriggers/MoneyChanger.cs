using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyChanger : ToTrigger
{

    public int moneyChange;

    public override IEnumerator DoAction()
    {
        PlayerPermanentStats playerPermanentStats = GameObject.Find("Menu").GetComponent<menuMain>().playerPermanentStats;
        
        playerPermanentStats.smams += moneyChange;
        FieldEvents.HasCompleted.Invoke(this.gameObject);

        yield return null;
    }
}

