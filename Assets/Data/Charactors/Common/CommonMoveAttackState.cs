using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonMoveAttackState : StateMachineBehaviour {
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        CommonMoveState.Move(animator);

        if (Globe.netMode)
        {
            GameObject obj = animator.gameObject;
            //CommonRangeAttackState.Net_RangeFire(obj);
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.GetBool("range"))
            return;

        CommonMoveState.Move(animator);

        GameObject obj = animator.gameObject;
        if (!Globe.netMode)
        {
            CommonRangeAttackState.RangeAttack(obj);
        }
    }

}

