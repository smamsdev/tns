using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : ToTrigger
{
    public string shopName;
    public List<GearSO> shopGearInventory = new List<GearSO>();
    public ShopMenuManager shopMenuManager;
    public GameObject shopContainerDisplay;

    private void OnEnable()
    {
        shopGearInventory.Sort((a, b) => a.name.CompareTo(b.name));

        for (int i = 1; i < shopGearInventory.Count; i++)
        {
            if (shopGearInventory[i] == shopGearInventory[i - 1])
            {
                Debug.LogWarning(
                    $"Duplicate GearSO in shop inventory: {shopGearInventory[i].name}",
                    this
                );
            }
        }

        shopContainerDisplay.SetActive(false);

        if (shopName == "")
            Debug.Log(this + " shop name is blank");

        shopMenuManager.mainMenu.shopnameTMP.text = shopName;
        shopMenuManager.gameObject.SetActive(false);
    }

    public override IEnumerator TriggerFunction()
    {
        shopMenuManager.gameObject.SetActive(true);
        shopMenuManager.OpenShop();
        yield return null;
    }
}
