using UnityEngine;
using UnityEngine.UI;

public class MenuGearPageSelection : Menu
{
    [SerializeField] Button firstButtonToSelect;
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
        button.onHighlighed = () => container.SetActive(true);
        button.onUnHighlighed = () => container.SetActive(false);

        container.SetActive(false);
    }

    public override void EnterMenu()
    {
        WireButton(equippedHighlightedButton, equippedDisplayContainer);
        WireButton(inventoryHighlightedButton, inventoryDisplayContainer);

        menuManagerUI.ClearThenDisplayMenu(this);
        gearPropertiesDisplay.SetActive(false);

        menuGearEquipSubPage.InitialiseEquipSlots();
        menuGearInventorySubPage.InstantiateUIInventorySlots();
        firstButtonToSelect.Select();
    }

    public override void ExitMenu()
    {
        menuManagerUI.EnterMenu(menuManagerUI.main);
        menuManagerUI.menuUpdateMethod.lastParentButtonSelected.SetButtonNormalColor(Color.white);
        menuManagerUI.menuUpdateMethod.lastParentButtonSelected.button.Select();
    }

    public void EnterEquipSubPage()
    {
        menuGearInventorySubPage.displayContainer.SetActive(false);
        gearPropertiesDisplay.SetActive(true);
        menuManagerUI.EnterMenu(menuManagerUI.gearEquipSubPage);
    }

    public void EnterInventorySubPage()
    {
        menuGearEquipSubPage.displayContainer.SetActive(false);
        gearPropertiesDisplay.SetActive(true);
        menuManagerUI.EnterMenu(menuManagerUI.gearInventorySubPage);
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu();
        }
    }
}
