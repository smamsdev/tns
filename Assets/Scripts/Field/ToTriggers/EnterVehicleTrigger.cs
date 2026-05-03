using System.Collections;
using UnityEngine;

public class EnterVehicleTrigger : ToTrigger
{
    public VehicleInstance vehicleInstance;
    public GameObject optionalActorToEnter;

    public override IEnumerator TriggerFunction()
    {
        GameObject GOToEnter;

        CombatEvents.LockPlayerMovement();
        yield return new WaitForSeconds (.5f);


        if (optionalActorToEnter != null)
            GOToEnter = optionalActorToEnter;

        else
            GOToEnter = GameObject.FindGameObjectWithTag("Player");

        vehicleInstance.EnterVehicle(GOToEnter);

        CombatEvents.UnlockPlayerMovement();
        yield return null;
    }
}
