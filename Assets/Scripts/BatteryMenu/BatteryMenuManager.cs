using UnityEngine;

public class BatteryMenuManager : MonoBehaviour
{
    public BatteryMainMenu batteryMainMenu;
    public BatteryEquipMenu batteryEquipMenu;

    [Header("Debug")]
    public Menu menuUpdateMethod;

    private void Start()
    {
        OpenBatteryMenu();
    }

    public void OpenBatteryMenu()
    {
        batteryMainMenu.DisplayMenu(true);
        batteryMainMenu.InitializeMenu();
        menuUpdateMethod = batteryMainMenu;
        batteryMainMenu.EnterMenu();
    }

    public void DisplaySubMenu(Menu menuToDisplay)
    {
        batteryEquipMenu.DisplayMenu(false);

        menuToDisplay.DisplayMenu(true);
    }

    public void EnterMenu(Menu chargingMenu)
    {
        menuUpdateMethod = chargingMenu;
        chargingMenu.EnterMenu();
    }

    void Update()
    {
        StateUpdate(menuUpdateMethod);
    }

    void StateUpdate(Menu menuUpdateMethod)
    {
        menuUpdateMethod.StateUpdate();
    }
}
