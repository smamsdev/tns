using UnityEditor;
using UnityEngine;

public class DropMenuManager : MonoBehaviour
{
    public DropMainMenu dropMainMenu;
    public DropSelectMenu dropSelectMenu;
    public DropGearMenu dropGearMenu;

    [Header("Debug")]
    public Menu menuUpdateMethod;

    public void OpenDropMenu()
    {
        dropMainMenu.DisplayMenu(true);
        dropMainMenu.InitializeMenu();
        menuUpdateMethod = dropMainMenu;
        dropMainMenu.EnterMenu();
    }
    
    public void DisplaySubMenu(Menu menuToDisplay)
    {
        dropSelectMenu.DisplayMenu(false);
        dropGearMenu.DisplayMenu(false);
    
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
