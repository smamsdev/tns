using UnityEditor;
using UnityEngine;

public class LockerMenuManager : MonoBehaviour
{
    public LockerMainMenu lockerMainMenu;
    public LockerBayMenu lockerBayMenu;
    public LockerGearMenu lockerGearMenu;


    [Header("Debug")]
    public Menu menuUpdateMethod;

    public void OpenLocker()
    {
        lockerMainMenu.InitializeMenu();
        menuUpdateMethod = lockerMainMenu;
        lockerMainMenu.DisplayMenu(true);
        lockerMainMenu.EnterMenu();
    }
    
    public void DisplaySubMenu(Menu menuToDisplay)
    {
        lockerBayMenu.DisplayMenu(false);
        lockerGearMenu.DisplayMenu(false);
    
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
