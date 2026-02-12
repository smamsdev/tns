using UnityEngine;
using UnityEngine.UI;

public class MenuGearPageSelection : PauseMenu
{
    [SerializeField] GameObject equippedDisplayContainer, inventoryDisplayContainer, gearPropertiesDisplay;

    public MenuButtonHighlighted equippedHighlightedButton, inventoryHighlightedButton;
    public MenuGearEquipSubPage menuGearEquipSubPage;
    public MenuGearInventorySubPage menuGearInventorySubPage;
    bool isInitialized = false;

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

        pauseMenuManager.ClearThenDisplayMenu(this);
        gearPropertiesDisplay.SetActive(false);

        if (!isInitialized)
        {
            menuGearEquipSubPage.InitialiseEquipSlots();
            menuGearInventorySubPage.InstantiateUIInventorySlots();
            isInitialized = true;
        }

        if (lastParentButtonSelected ==null) { lastParentButtonSelected = equippedHighlightedButton; }
        lastParentButtonSelected.button.Select();
    }

    public override void ExitMenu()
    {
        isInitialized = false;
        pauseMenuManager.EnterMenu(pauseMenuManager.main);
        pauseMenuManager.menuUpdateMethod.lastParentButtonSelected.SetButtonNormalColor(Color.white);
        pauseMenuManager.menuUpdateMethod.lastParentButtonSelected.button.Select();
    }

    public void EnterEquipSubPage()
    {
        equippedHighlightedButton.ButtonSelectedAndDisabled();
        menuGearInventorySubPage.displayContainer.SetActive(false);
        gearPropertiesDisplay.SetActive(true);
        pauseMenuManager.EnterMenu(pauseMenuManager.gearEquipSubPage);
    }

    public void EnterInventorySubPage()
    {
        if (menuGearInventorySubPage.playerInventory.inventorySO.gearInstanceInventory.Count > 0)
        {
            inventoryHighlightedButton.ButtonSelectedAndDisabled();
            menuGearEquipSubPage.displayContainer.SetActive(false);
            gearPropertiesDisplay.SetActive(true);
            pauseMenuManager.EnterMenu(pauseMenuManager.gearInventorySubPage);
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
