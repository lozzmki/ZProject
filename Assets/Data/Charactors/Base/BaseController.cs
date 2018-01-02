using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class BaseController : MonoBehaviour {
    protected Animator m_Animator;
    protected Timer m_LockTimer;
    private bool m_bAutoLock;
    private bool m_bDodged;
	// Use this for initialization
	void Start () {
        m_Animator = gameObject.GetComponentInChildren<Animator>();
        m_LockTimer = new Timer(0.3f);
        m_bAutoLock = true;
        m_bDodged = false;
        Transceiver _trans = gameObject.GetComponent<Transceiver>();

        _trans.AddResolver("Joystick", OnJoystick);
        _trans.AddResolver("FireButton", OnFireButton);
        _trans.AddResolver("DodgeButton", OnDodgeButton);
    }

    private void Update()
    {
        m_LockTimer.Update();
        if(m_bAutoLock && m_LockTimer.Paused && (null == gameObject.GetComponent<Entity>().m_Target || !gameObject.GetComponent<Entity>().m_Target.activeInHierarchy)) {
            GetLockTarget();
            if (gameObject.GetComponent<Entity>().m_Target == null)
                m_bAutoLock = false;
        }
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
            _vDirection.y = 0.0f;
            Move(_vDirection);
        }
    }
    private void OnFireButton(DSignal signal)
    {
        bool _pressed = System.Convert.ToBoolean(signal._arg1);
        if (_pressed) {
            Fire();
            if (m_LockTimer.IfExpired) {
                GetLockTarget();
            }
            m_LockTimer.Reset();
            m_LockTimer.Paused = true;
            m_bAutoLock = true;
        }

        else {
            CeaseFire();
            m_LockTimer.Paused = false;
        }
            
    }

    private void OnDodgeButton(DSignal signal)
    {
        if (!m_bDodged) {
            m_bDodged = true;
            StartCoroutine(Dodge());
        }
    }

    private GameObject GetNearestObject(List<GameObject> list)
    {
        GameObject _obj = null;
        float _mag = 0.0f;
        for(int i=0; i<list.Count; i++) {
            RaycastHit _hit;
            if(Physics.Linecast(transform.position, list[i].transform.position, out _hit, 1<<8|1<<9, QueryTriggerInteraction.Ignore)) {
                if (_hit.collider.gameObject != list[i])
                    continue;
            }
            Vector3 _delta = list[i].transform.position - gameObject.transform.position;
            if (_obj == null) {
                _obj = list[i];
                _mag = _delta.magnitude;
            }
            else {
                float _tmp = _delta.magnitude;
                if (_mag > _tmp) {
                    _obj = list[i];
                    _mag = _tmp;
                }
            }
            
        }
        return _obj;
    }

    private void GetLockTarget()
    {
        Collider[] _hitColliders = Physics.OverlapSphere(gameObject.transform.position, 100.0f);
        List<GameObject> _front = new List<GameObject>(), _back = new List<GameObject>();
        for(int i=0; i<_hitColliders.Length; i++) {
            GameObject _obj = _hitColliders[i].gameObject;
            Entity _ty = _obj.GetComponent<Entity>();
            if (_ty == null || _obj == gameObject)
                continue;
            
            Vector3 _localPos = gameObject.transform.InverseTransformPoint(_obj.transform.position);
            if (_localPos.z > 0.0f)
                _front.Add(_obj);
            else
                _back.Add(_obj);
        }
        Entity _ety = gameObject.GetComponent<Entity>();
        if (_front.Count > 0) {
            _ety.m_Target = GetNearestObject(_front);
            if(_ety.m_Target == null) {
                _ety.m_Target = GetNearestObject(_back);
            }
        }
        else {
            if (_back.Count > 0) {
                _ety.m_Target = GetNearestObject(_back);
            }
            else
                _ety.m_Target = null;
        }
    }
    //common move
    virtual protected void Move(Vector3 vDirection)
    {
        //gameObject.transform.forward = vDirection;
        gameObject.GetComponent<Entity>().m_MovingDirection = vDirection;
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
                return;
            }
            Item _weapon = _inv.GetWeapon();
            if (_weapon == null)
                return;

            if (_weapon.m_WeaponType == WeaponType.WEAPON_MELEE) {
                m_Animator.SetBool("melee", true);
            }
            else {
                m_Animator.SetBool("range", true);
            }
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
    virtual protected IEnumerator Dodge()
    {
        m_Animator.SetBool("dodge", true);
        yield return new WaitForSeconds(0.1f);
        m_Animator.SetBool("dodge", false);
        m_bDodged = false;
    }
}
