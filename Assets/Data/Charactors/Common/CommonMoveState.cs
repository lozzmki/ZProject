using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonMoveState : StateMachineBehaviour {

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Entity _entity = animator.gameObject.GetComponent<Entity>();
        if (_entity == null)
            return;

//         if (animator.GetBool("range"))
//             return;

        animator.gameObject.transform.position += animator.gameObject.transform.forward.normalized
            * _entity.m_Properties[Entity.SPEED].d_Value * Time.deltaTime;
    }
}
