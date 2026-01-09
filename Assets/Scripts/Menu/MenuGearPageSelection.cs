using UnityEngine;
using UnityEngine.UI;

public class MenuGearPageSelection : Menu
{
    [SerializeField] GameObject equippedDisplayContainer, inventoryDisplayContainer, gearPropertiesDisplay;

    public MenuButtonHighlighted equippedHighlightedButton, inventoryHighlightedButton;
    public MenuGearEquipSubPage menuGearEquipSubPage;
    public MenuGearInventorySubPage menuGearInventorySubPage;

    public override void DisplayMenu(bool on)
    {
        displayContainer.SetActive(on);
    }

    private void WireButton(MenuButtonHighlighted button, GameObject container)
    {
        button.onHighlighted = () => container.SetActive(true);
        button.onUnHighlighted = () => container.SetActive(false);

        container.SetActive(false);
    }

    public override void EnterMenu()
    {
        menuGearInventorySubPage.playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>().playerInventory;

        displayContainer.SetActive(true);
        menuGearEquipSubPage.equipPageHeaderGO.SetActive(false);
        WireButton(equippedHighlightedButton, equippedDisplayContainer);
        WireButton(inventoryHighlightedButton, inventoryDisplayContainer);

        menuManagerUI.ClearThenDisplayMenu(this);
        gearPropertiesDisplay.SetActive(false);

        menuGearEquipSubPage.InitialiseEquipSlots();
        menuGearInventorySubPage.InstantiateUIInventorySlots();

        if (lastParentButtonSelected ==null) { lastParentButtonSelected = equippedHighlightedButton; }
        lastParentButtonSelected.button.Select();
    }

    public override void ExitMenu()
    {
        menuManagerUI.EnterMenu(menuManagerUI.main);
        menuManagerUI.menuUpdateMethod.lastParentButtonSelected.SetButtonNormalColor(Color.white);
        menuManagerUI.menuUpdateMethod.lastParentButtonSelected.button.Select();
    }

    public void EnterEquipSubPage()
    {
        equippedHighlightedButton.ButtonSelectedAndDisabled();
        menuGearInventorySubPage.displayContainer.SetActive(false);
        gearPropertiesDisplay.SetActive(true);
        menuManagerUI.EnterMenu(menuManagerUI.gearEquipSubPage);
    }

    public void EnterInventorySubPage()
    {
        if (menuGearInventorySubPage.playerInventory.inventorySO.gearInventory.Count > 0)
        {
            inventoryHighlightedButton.ButtonSelectedAndDisabled();
            menuGearEquipSubPage.displayContainer.SetActive(false);
            gearPropertiesDisplay.SetActive(true);
            menuManagerUI.EnterMenu(menuManagerUI.gearInventorySubPage);
        }
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuGearEquipSubPage.isEquipping = false;
            ExitMenu();
        }
    }
}
