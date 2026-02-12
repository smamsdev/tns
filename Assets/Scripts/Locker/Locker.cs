using System.Collections;
using UnityEngine;

public class Locker : ToTrigger
{
    public InventorySO inventory;
    public LockerMenuManager lockerMenuManager;

    public override IEnumerator TriggerFunction()
    {
        lockerMenuManager.gameObject.SetActive(true);
        //lockerMenuManager.lockerMainMenu.inventory = inventory;
        lockerMenuManager.lockerMainMenu.inventory = inventory;
        lockerMenuManager.OpenLocker();
        yield return null;
    }
}
