using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonMeleeAttackState : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        //TODO:start charging
    }


    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Entity _entity = animator.gameObject.GetComponent<Entity>();
        GameObject _obj = animator.gameObject;
        Item _weapon = _obj.GetComponent<Inventory>().GetWeapon();

        GameObject _tar = _entity.m_Target;
        Vector3 _fac;
        //if target exist
        if (null != _tar) {
            _fac = _tar.transform.position - _obj.transform.position;
            _obj.transform.forward = _fac;
        }


        if (_weapon != null) {
            if (!_weapon.IfCooled)
                return;

            _weapon.Attack();

            //GameObject _proj = GameObject.CreatePrimitive(PrimitiveType.Sphere);// = new GameObject();
            GameObject _proj = new GameObject();
            _proj.AddComponent<SphereCollider>().radius = 1.0f;
            _proj.GetComponent<SphereCollider>().isTrigger = true;
            _proj.AddComponent<Projectile>().m_nType = ProjectileType.PROJECTILE_MELEE;
            _proj.GetComponent<Projectile>().m_Master = _obj;
            _proj.GetComponent<Projectile>().m_fLife = 1.5f;
            _proj.GetComponent<Projectile>().m_MaxTargetNum = -1;
            _proj.GetComponent<Projectile>().m_Damage = _weapon.m_Damage + _entity.m_Properties[Entity.MELEE_POWER].d_Value;
            _proj.GetComponent<Projectile>().m_HitEffect = new List<GameObject>();
            _proj.transform.position = _obj.transform.position + _obj.transform.forward.normalized * 1.0f;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        //TODO:release charged attack
    }
}
