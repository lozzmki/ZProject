using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public Transform m_Player;
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
        float _fRotation = Input.GetAxis("RH") * m_TurnSpeed * Time.deltaTime;
        //this.transform.Rotate(0.0f,_fRotation,0.0f);
        if (m_Invert)
            m_Angle -= _fRotation;
        else
            m_Angle += _fRotation;
        float _fAngle = Mathf.Deg2Rad * m_Angle;
        Vector3 _vXOZ = new Vector3(Mathf.Cos(_fAngle), -1.0f, Mathf.Sin(_fAngle));
        _vXOZ.y = -1.0f;
        Vector3 _pos = m_Player.position - 10.0f * _vXOZ;// + m_Player.forward * -2.0f;
        this.transform.position = _pos;
        this.transform.LookAt(m_Player);
        //this.transform.forward = m_Player.forward;
    }
}
