using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonMoveState : StateMachineBehaviour {

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject _obj = animator.gameObject;
        if (_obj.transform.parent != null && _obj.transform.parent.gameObject.GetComponent<Entity>()!=null)
            _obj = _obj.transform.parent.gameObject;

        Entity _entity = _obj.GetComponent<Entity>();
        if (_entity == null)
            return;

        //         if (animator.GetBool("range"))
        //             return;
        _obj.transform.forward = _entity.m_MovingDirection;

        //if(!Physics.Raycast(animator.gameObject.transform.position, animator.gameObject.transform.forward, _entity.m_Properties[Entity.SPEED].d_Value * Time.deltaTime, 1<<9))
        //_obj.GetComponent<Rigidbody>().velocity = _entity.m_MovingDirection.normalized * _entity.m_Properties[Entity.SPEED].d_Value;
        //_obj.GetComponent<Rigidbody>().MovePosition(_obj.transform.position + _entity.m_MovingDirection.normalized * _entity.m_Properties[Entity.SPEED].d_Value * Time.deltaTime);
        //_obj.GetComponent<Rigidbody>().AddForce(_entity.m_MovingDirection.normalized * _entity.m_Properties[Entity.SPEED].d_Value * Time.deltaTime, ForceMode.VelocityChange);
        Transceiver.SendSignal(new DSignal(_obj, _obj, "AddMove", _entity.m_MovingDirection.normalized * _entity.m_Properties[Entity.SPEED].d_Value));

        //animator.gameObject.transform.position += _entity.m_MovingDirection.normalized * _entity.m_Properties[Entity.SPEED].d_Value * Time.deltaTime;
    }
}
