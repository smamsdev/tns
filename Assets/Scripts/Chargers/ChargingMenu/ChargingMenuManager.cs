using UnityEngine;

public class ChargingMenuManager : MonoBehaviour
{
    [Header("CHARGING MENUS")]

    public ChargingMainMenu chargingMainMenu;
    public ChargingSlotMenu ChargingSlotSelectMenu;
    public ChargingEquipmentSelectMenu chargingEquipmentSelectMenu;
    public GameObject subMenuPanel;

    [Header("Debug")]
    public ChargingMenu menuUpdateMethod;

    public void OpenChargingStation()
    {
        chargingMainMenu.InitializeChargingStationMenu();
        menuUpdateMethod = chargingMainMenu;
        chargingMainMenu.DisplayMenu(true);
        chargingMainMenu.EnterMenu();
    }

    public void DisplaySubMenu(ChargingMenu menuToDisplay)
    {
        ChargingSlotSelectMenu.DisplayMenu(false);
        chargingEquipmentSelectMenu.DisplayMenu(false);

        menuToDisplay.DisplayMenu(true);
    }

    public void EnterMenu(ChargingMenu chargingMenu)
    {
        menuUpdateMethod = chargingMenu;
        chargingMenu.EnterMenu();
    }

    void Update()
    {
        StateUpdate(menuUpdateMethod);
    }

    void StateUpdate(ChargingMenu menuUpdateMethod)
    {
        menuUpdateMethod.StateUpdate();
    }
}
