using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour {

    public GameObject m_Player;
    public float m_TurnSpeed = 300.0f;
    public float m_Angle = 0.0f;
    public bool m_Invert = true;
    
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayer();
        UpdateCamera();
    }

    private void UpdateCamera()
    {
        float _fRotation = Input.GetAxis("RH") * m_TurnSpeed * Time.deltaTime;
        if (m_Invert)
            m_Angle -= _fRotation;
        else
            m_Angle += _fRotation;
        float _fAngle = Mathf.Deg2Rad * m_Angle;
        Vector3 _vXOZ = new Vector3(Mathf.Cos(_fAngle), -1.0f, Mathf.Sin(_fAngle));
        _vXOZ.y = -1.0f;
        Vector3 _pos = m_Player.transform.position - 10.0f * _vXOZ;// + m_Player.forward * -2.0f;
        this.transform.position = _pos;
        this.transform.LookAt(m_Player.transform);
    }
    private void UpdatePlayer() {
        //Movement
        float _fHor = Input.GetAxis("Horizontal");
        float _fVer = Input.GetAxis("Vertical");
        if (Mathf.Abs(_fHor) < 1e-4 && Mathf.Abs(_fVer) < 1e-4) {

        }
        else {
            Vector3 _vDirection;
            Transform m_Cam = gameObject.transform;

            _vDirection = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized * _fVer + _fHor * m_Cam.right;

            Transceiver.SendSignal(new DSignal(gameObject, m_Player, "Move", _vDirection));
        }
        //Fire
        bool _bRangeFire = Input.GetButtonDown("Attack");

        if (_bRangeFire) {
            if (m_Player.GetComponent<Inventory>().m_ItemForPick != null) {
                Transceiver.SendSignal(new DSignal(gameObject, m_Player, "Pickup"));
            }
            Transceiver.SendSignal(new DSignal(gameObject, m_Player, "RangeFire"));
        }
        else {
            if (Input.GetButton("Attack")) {
                Transceiver.SendSignal(new DSignal(gameObject, m_Player, "RangeFire"));
            }
        }

        
    }
}
