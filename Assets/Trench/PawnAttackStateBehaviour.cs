using UnityEngine;

public class PawnAttackStateBehaviour : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Pawn>().OnApplyAttack();
    }
}
