using System.Collections;
using UnityEngine;

public class Locker : ToTrigger
{
    public InventorySO lockerInventorySO;
    public LockerMenuManager lockerMenuManager;

    public override IEnumerator TriggerFunction()
    {
        lockerMenuManager.gameObject.SetActive(true);
        //lockerMenuManager.lockerMainMenu.inventory = inventory;
        lockerMenuManager.lockerMainMenu.lockerInventorySO = lockerInventorySO;
        lockerMenuManager.OpenLocker();
        yield return null;
    }
}
