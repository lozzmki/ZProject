using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonRangeAttackState : StateMachineBehaviour {

    public static void RangeAttack(GameObject _obj)
    {
        GameObject _proj;
        Item _weapon = _obj.GetComponent<Inventory>().GetWeapon();
        Item _parts = _obj.GetComponent<Inventory>().GetParts();
        float _baseDmg = 0.0f;

        GameObject _tar = _obj.GetComponent<Entity>().m_Target;
        Vector3 _fac = _obj.transform.forward;
        //if target exist
        if(null != _tar) {
            _fac = _tar.transform.position - _obj.transform.position;
            _obj.transform.forward = Vector3.Scale(_fac, new Vector3(1, 0, 1));
        }


        

        if (_weapon != null) {
            if (!_weapon.IfCooled)
                return;

            //check cost
            float _cost = _weapon.m_EnergyCost;
            if (_obj.GetComponent<Entity>().CanAfford(_cost)) {
                Transceiver.SendSignal(new DSignal(_obj, _obj, "CostEnergy", _cost));
            }
            else {
                return;
            }

            _weapon.Attack();

            _baseDmg += _weapon.m_Damage;

            if (_parts) {
                string _ammoName = _parts.m_Projectile;
                _proj = Resources.Load<GameObject>("Prefabs/Projectiles/" + _ammoName);
                if (_proj == null) {
                    Debug.Log("Ammo:Model is null");
                    _proj = Instantiate(Resources.Load<GameObject>("Prefabs/Projectiles/Default"),
                            _obj.transform.position, Quaternion.AngleAxis(0.0f, Vector3.up));
                }
                else {
                    _proj = Instantiate(_proj, _obj.transform.position, Quaternion.AngleAxis(0.0f, Vector3.up));
                }

                _proj.GetComponent<Projectile>().m_Damage = _parts.m_Damage;
                _baseDmg += _parts.m_Damage;
            }
            else {
                //no ammo, use default
                _proj = Instantiate(Resources.Load<GameObject>("Prefabs/Projectiles/Default"), _obj.transform.position, Quaternion.AngleAxis(0.0f, Vector3.up));
            }


            _proj.transform.parent = _obj.transform.parent;
            _proj.transform.forward = _fac;
            _proj.GetComponent<Projectile>().m_Master = _obj;
            _proj.GetComponent<Projectile>().m_Damage += (_baseDmg + _obj.GetComponent<Entity>().m_Properties[Entity.RANGE_POWER].d_Value);
            _proj.GetComponent<Projectile>().m_Speed = 30.0f;
            _proj.GetComponent<Projectile>().m_Weapon = _weapon;
            _proj.GetComponent<Projectile>().m_Parts = _parts;

            GameObject _extra;
            switch (_weapon.m_WeaponType) {

                case WeaponType.WEAPON_AUTO:
                    //no more
                    break;
                case WeaponType.WEAPON_SPREAD:
                    //four more
                    //TODO:Fire a cone collider
                    for (int _i = 0; _i < 4; _i++) {
                        _extra = Instantiate(_proj);
                        _extra.transform.parent = _obj.transform.parent;
                        _extra.transform.Rotate(0, 2.0f - 1.0f * _i, 0);
                    }
                    _proj.transform.Rotate(0, -2.0f, 0);
                    break;
                //undefined
                //                 case WeaponType.WEAPON_ARC:
                //                     break;
                //                 case WeaponType.WEAPON_LASER:
                //                     break;
                default:
                    break;
            }

        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject _obj = animator.gameObject;
        if (_obj.transform.parent != null && _obj.transform.parent.gameObject.GetComponent<Entity>() != null)
            _obj = _obj.transform.parent.gameObject;
        RangeAttack(_obj);
    }
}
