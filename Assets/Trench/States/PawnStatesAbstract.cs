using UnityEngine;

public abstract class PawnState : MonoBehaviour
{
    public Pawn pawn;

    public abstract void EnterState();

    public abstract void PawnUpdate();
}
