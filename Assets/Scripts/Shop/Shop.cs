using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : ToTrigger
{
    public string shopName;
    public List<GearSO> shopGearInventory = new List<GearSO>();
    public ShopMenuManagerUI shopMenuManagerUI;
    public Animator containerAnimator;
    public GameObject shopContainerDisplay;

    private void OnEnable()
    {
        shopGearInventory.Sort((a, b) => a.name.CompareTo(b.name));
        shopContainerDisplay.SetActive(false);

        if (shopName == "")
            Debug.Log(this + " shop name is blank");

        shopMenuManagerUI.mainMenu.shopnameTMP.text = shopName;
    }

    public void OpenMenu()
    {
        shopContainerDisplay.SetActive(true);
        containerAnimator.Play("OpenShop");
    }

    public void CloseMenu() 
    {
        containerAnimator.Play("CloseShop");
    }

    public void DisableAnimator()
    {
        shopContainerDisplay.SetActive(false);
    }

    public override IEnumerator TriggerFunction()
    {
        OpenMenu();
        yield return null;
    }
}
