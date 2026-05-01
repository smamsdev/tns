using System.Collections;
using UnityEngine;

public class EnterVehicleTrigger : ToTrigger
{
    public VehicleInstance vehicleInstance;
    public GameObject optionalActorToEnter;

    public override IEnumerator TriggerFunction()
    {
        GameObject GOToEnter;

        if (optionalActorToEnter != null)
            GOToEnter = optionalActorToEnter;

        else
            GOToEnter = GameObject.FindGameObjectWithTag("Player");

        vehicleInstance.EnterVehicle(GOToEnter);
        yield return null;
    }
}
