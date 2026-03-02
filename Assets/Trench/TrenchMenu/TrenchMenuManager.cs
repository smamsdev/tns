using UnityEngine;

public class TrenchMenuManager : MonoBehaviour
{
    public TrenchManager trenchManager;
    public TrenchMenuState currentState;
    public TrenchMainState mainState;
    public TrenchStructureInventoryState inventoryState;
    public TrenchConstructState constructState;
    public InventorySO playerInventorySO;

    public void ChangeState(TrenchMenuState menuState)
    {
        menuState.EnterState();
        currentState = menuState;
    }

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerInventorySO = player.GetComponentInChildren<PlayerInventory>().inventorySO;

        InitiliazeMenu();


    }

    private void Update()
    {
        currentState.StateUpdate();
    }

    public void InitiliazeMenu()
    {
        foreach (TrenchFrontLine trenchFrontLine in trenchManager.frontLines)
        {
            constructState.ShowEmptySlots(trenchFrontLine.leftStructures,false);
            constructState.ShowEmptySlots(trenchFrontLine.rightStructures, false);
        }

        mainState.WireButtons();
        ChangeState(mainState);
    }
}
