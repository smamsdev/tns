using UnityEditor;
using UnityEngine;

public class LockerMenuManager : MonoBehaviour
{
    public LockerMainMenu lockerMainMenu;
    public LockerCacheMenu lockerCacheMenu;
    public LockerGearMenu lockerGearMenu;


    [Header("Debug")]
    public Menu menuUpdateMethod;

    public void OpenLocker()
    {
        lockerMainMenu.DisplayMenu(true);
        lockerMainMenu.InitializeMenu();
        menuUpdateMethod = lockerMainMenu;
        lockerMainMenu.EnterMenu();
    }
    
    public void DisplaySubMenu(Menu menuToDisplay)
    {
        lockerCacheMenu.DisplayMenu(false);
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
