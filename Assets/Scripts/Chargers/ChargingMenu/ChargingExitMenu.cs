using UnityEngine;

public class ChargingExitMenu : ChargingMenu
{
    public override void DisplayMenu(bool on)
    {
        displayContainer.SetActive(on);
    }

    public override void EnterMenu()
    {
        throw new System.NotImplementedException();
    }

    public override void ExitMenu()
    {
        throw new System.NotImplementedException();
    }

    public override void StateUpdate()
    {
        throw new System.NotImplementedException();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
