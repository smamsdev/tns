using Unity.VisualScripting;
using UnityEngine;

public class PawnDefeatStateBehaviour : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var pawn = animator.GetComponent<Pawn>();
        pawn.StartCoroutine(pawn.OnDefeated());
    }
}
