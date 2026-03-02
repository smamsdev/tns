using UnityEngine;

public abstract class TrenchMenuState : MonoBehaviour
{
    public TrenchManager trenchManager;
    public TrenchMenuManager menuManager;

    public abstract void EnterState();

    public abstract void ExitState();

    public abstract void StateUpdate();

}
