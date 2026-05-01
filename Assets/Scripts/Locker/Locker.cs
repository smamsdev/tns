using System.Collections;
using UnityEngine;

public class Locker : ToTrigger
{
    public InventorySO lockerInventorySO;
    public LockerMenuManager lockerMenuManager;
    public string CacheName;

    private void Awake()
    {
        lockerMenuManager.lockerMainMenu.cacheNameTMP.text = CacheName;
    }

    public override IEnumerator TriggerFunction()
    {
        lockerMenuManager.gameObject.SetActive(true);
        lockerMenuManager.lockerMainMenu.lockerInventorySO = lockerInventorySO;
        lockerMenuManager.OpenLocker();
        yield return null;
    }
}
