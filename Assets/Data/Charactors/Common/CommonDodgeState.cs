using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonDodgeState : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject _obj = animator.gameObject;
        if (_obj.transform.parent != null && _obj.transform.parent.gameObject.GetComponent<Entity>() != null)
            _obj = _obj.transform.parent.gameObject;
        _obj.layer = 12;

    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject _obj = animator.gameObject;
        if (_obj.transform.parent != null && _obj.transform.parent.gameObject.GetComponent<Entity>() != null)
            _obj = _obj.transform.parent.gameObject;

        Entity _entity = _obj.GetComponent<Entity>();
        if (_entity == null)
            return;

        _obj.transform.forward = _entity.m_MovingDirection;

        //if (!Physics.Raycast(animator.gameObject.transform.position, animator.gameObject.transform.forward, _entity.m_Properties[Entity.SPEED].d_Value * Time.deltaTime * 3.0F, 1 << 9))
        //animator.gameObject.transform.position += _entity.m_MovingDirection.normalized * _entity.m_Properties[Entity.SPEED].d_Value * Time.deltaTime * 3.0F;
        //animator.gameObject.GetComponent<Rigidbody>().velocity = _entity.m_MovingDirection.normalized * _entity.m_Properties[Entity.SPEED].d_Value * 3.0f;
        
        //_obj.GetComponent<Rigidbody>().MovePosition(_obj.transform.position + _entity.m_MovingDirection.normalized * _entity.m_Properties[Entity.SPEED].d_Value * Time.deltaTime*3.0f);
        Transceiver.SendSignal(new DSignal(_obj, _obj, "AddMove", _entity.m_MovingDirection.normalized * _entity.m_Properties[Entity.SPEED].d_Value * 3.0f));
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject _obj = animator.gameObject;
        if (_obj.transform.parent != null && _obj.transform.parent.gameObject.GetComponent<Entity>() != null)
            _obj = _obj.transform.parent.gameObject;
        _obj.layer = 8;

    }
}
