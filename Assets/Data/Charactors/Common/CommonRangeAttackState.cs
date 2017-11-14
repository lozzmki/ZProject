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

        if (_weapon != null) {
            if (!_weapon.IfCooled)
                return;
            _weapon.Attack();

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
                _baseDmg = _parts.m_Damage;
            }
            else {
                //no ammo, use default
                _proj = Instantiate(Resources.Load<GameObject>("Prefabs/Projectiles/Default"), _obj.transform.position, Quaternion.AngleAxis(0.0f, Vector3.up));
            }

            

            _proj.transform.forward = _obj.transform.forward;
            _proj.GetComponent<Projectile>().m_Master = _obj;
            _proj.GetComponent<Projectile>().m_Damage += (_baseDmg + _obj.GetComponent<Entity>().m_Properties[Entity.RANGE_POWER].d_Value);
            _proj.GetComponent<Projectile>().m_Speed = 30.0f;

            GameObject _extra;
            switch (_weapon.m_WeaponType) {

                case WeaponType.WEAPON_AUTO:
                    //no more
                    break;
                case WeaponType.WEAPON_SPREAD:
                    //four more
                    for (int _i = 0; _i < 4; _i++) {
                        _extra = Instantiate(_proj);
                        _extra.transform.Rotate(0, 20.0f - 10.0f * _i, 0);
                    }
                    _proj.transform.Rotate(0, -20.0f, 0);
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
        RangeAttack(animator.gameObject);
    }
}
