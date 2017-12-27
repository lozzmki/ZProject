using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class BaseController : MonoBehaviour {
    protected Animator m_Animator;
	// Use this for initialization
	void Start () {
        m_Animator = gameObject.GetComponent<Animator>();
        Transceiver _trans = gameObject.GetComponent<Transceiver>();

        _trans.AddResolver("Joystick", OnJoystick);
        _trans.AddResolver("FireButton", OnFireButton);
    }

    private void OnJoystick(DSignal signal)
    {
        float _fHor = System.Convert.ToSingle(signal._arg1);
        float _fVer = System.Convert.ToSingle(signal._arg2);

        if (Mathf.Abs(_fHor) < 1e-4 && Mathf.Abs(_fVer) < 1e-4) {
            StopMoving();
        }
        else {
            Vector3 _vDirection;
            Transform m_Cam = Camera.main.transform;
            
            _vDirection = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized * _fVer + _fHor * m_Cam.right;
            Move(_vDirection);
        }
    }
    private void OnFireButton(DSignal signal)
    {
        bool _pressed = System.Convert.ToBoolean(signal._arg1);
        if (_pressed)
            Fire();
        else
            CeaseFire();
    }

    //common move
    virtual protected void Move(Vector3 vDirection)
    {
        gameObject.transform.forward = vDirection;
        m_Animator.SetBool("move", true);
    }

    virtual protected void StopMoving()
    {
        m_Animator.SetBool("move", false);
    }

    virtual protected void Fire()
    {
        Inventory _inv = gameObject.GetComponent<Inventory>();
        if(_inv != null){
            if (_inv.m_ItemForPick != null) {
                Transceiver.SendSignal(new DSignal(gameObject, gameObject, "Pickup"));
            }
            if (!Globe.netMode)
            {
                Item _weapon = _inv.GetWeapon();
                if (_weapon == null)
                    return;

                if (_weapon.m_WeaponType == WeaponType.WEAPON_MELEE)
                {
                    m_Animator.SetBool("melee", true);
                }
                else
                {
                    m_Animator.SetBool("range", true);
                }
            }
            m_Animator.SetBool("range", true);

        }
        else {
            m_Animator.SetBool("melee", true);
        }
    }

    virtual protected void CeaseFire()
    {
        m_Animator.SetBool("range", false);
        m_Animator.SetBool("melee", false);
    }
}
