using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class ControlScript : MonoBehaviour {
    public GameObject m_Actor;
    public float m_Speed = 30.0f;
    public float m_TurningSpeed = 50.0f;

    private int m_nState;
	// Use this for initialization
	void Start () {
        m_nState = 0;//idle;
	}
	
	// Update is called once per frame
	void Update () {
        float _fHor = Input.GetAxis("Horizontal");
        float _fVer = Input.GetAxis("Vertical");
        bool _bAttack = Input.GetKey(KeyCode.Joystick1Button0);//test
        
        //a mess
        if(m_nState == 0 || m_nState == 1) {
            Move(_fHor, _fVer);
            if (_bAttack)
            {
                Attack();
            }
        }

    }

    private void Move(float fHor, float fVer)
    {
        if (Mathf.Abs(fHor) < 1e-4 && Mathf.Abs(fVer) < 1e-4)
        {
            //m_Actor.GetComponent<playerControl>().Reset();
            m_nState = 0;
            return;
        }

        m_nState = 1;//moving

        Vector3 _vDirection;
        Transform m_Cam;

        if (Camera.main != null)
        {
            m_Cam = Camera.main.transform;
            _vDirection = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized * fVer + fHor * m_Cam.right;
        }
        else
        {
            _vDirection = new Vector3(fHor, 0.0f, fVer);
        }

        gameObject.transform.position += _vDirection.normalized * m_Speed * Time.deltaTime;
        //m_Actor.GetComponent<playerControl>().Run();

        Vector3 _vTurn = gameObject.transform.InverseTransformDirection(_vDirection);
        float _fAngle = Mathf.Atan2(_vTurn.x, _vTurn.z) * Mathf.Rad2Deg;
        float _fRotation = m_TurningSpeed * Time.deltaTime;

        if (Mathf.Abs(_fAngle) < _fRotation)
            _fRotation = _fAngle;
        else
        {
            if (_fAngle < 0.0f)
                _fRotation = -_fRotation;
        }
        gameObject.transform.Rotate(0.0f, _fRotation, 0.0f);
    }
    private void Attack()
    {
//         m_nState = 2;//Attacking
//         Debug.Log("Attack");
//         int _nSel = Random.Range(0, 3);
//         switch (_nSel)
//         {
//             case 0:
//                 m_Actor.GetComponent<playerControl>().Attack01();
//                 break;
//             case 1:
//                 m_Actor.GetComponent<playerControl>().Attack02();
//                 break;
//             default:
//                 m_Actor.GetComponent<playerControl>().Attack03();
//                 break;
//         }
    }

    public void AttackKF()
    {
        //key frame
//         Attribute _playerAttr = gameObject.GetComponent<Attribute>();
//         Debug.Assert(_playerAttr != null, "player needs attribute");
//         foreach(var _kv in ObjectManager.getInstance().getDic())
//         {
//             GameObject _obj = _kv.Value;
//             Attribute _attr = _obj.GetComponent<Attribute>();
//             if(_attr != null && _obj != this)
//             {
//                 Vector3 _vTarget = _obj.transform.position - gameObject.transform.position;
//                 Vector3 _vDirection = gameObject.transform.forward;
// 
//                 float _fDistance = _vTarget.magnitude;
//                 //float _fAngle = Vector3.Angle(_vDirection, _vTarget);
// 
// //                 if(_fDistance <= _playerAttr.Range && _fAngle < 45.0f)
// //                 {
// //                     //_attr.damage(_playerAttr.Attack - _attr.Armor);
// //                 }
//             }
//         }
    }
    public void AttackFinished()
    {
        m_nState = 0;
        //Debug.Log("finished!");
    }
}
