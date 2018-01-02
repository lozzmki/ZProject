using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour {

    //public GameObject m_Player;
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
        //if (m_Player != null)//TODO:shouldn't be like this!!!!!!
        UpdatePlayer();
        //UpdateCamera();
        
    }

    //private void UpdateCamera()
    //{
    //    float _fRotation = Input.GetAxis("RH") * m_TurnSpeed * Time.deltaTime;
    //    if (m_Invert)
    //        m_Angle -= _fRotation;
    //    else
    //        m_Angle += _fRotation;
    //    float _fAngle = Mathf.Deg2Rad * m_Angle;
    //    Vector3 _vXOZ = new Vector3(Mathf.Cos(_fAngle), -1.0f, Mathf.Sin(_fAngle));
    //    _vXOZ.y = -1.0f;
    //    if(m_Player != null) {//TODO:shouldn't be like this!!!!!!
    //        Vector3 _pos = m_Player.transform.position - 10.0f * _vXOZ;// + m_Player.forward * -2.0f;
    //        this.transform.position = _pos;
    //        this.transform.LookAt(m_Player.transform);
    //    }
    //}
    private void UpdatePlayer() {
        //Movement
        float _fHor = Input.GetAxis("Horizontal");
        float _fVer = Input.GetAxis("Vertical");
        Transceiver.SendSignal(new DSignal(gameObject, GameManager.Player, "Joystick", _fHor, _fVer));

        //Fire
        if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.J)) {
            Transceiver.SendSignal(new DSignal(gameObject, GameManager.Player, "FireButton", true));
        }
        else if (Input.GetKeyUp(KeyCode.Joystick1Button0) || Input.GetKeyUp(KeyCode.J)) {
            Transceiver.SendSignal(new DSignal(gameObject, GameManager.Player, "FireButton", false));
        }

        //Dodge
        if(Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.K)) {
            Transceiver.SendSignal(new DSignal(gameObject, GameManager.Player, "DodgeButton"));
        }

        //FullMap
        if(Input.GetKeyDown(KeyCode.Joystick1Button8) || Input.GetKeyDown(KeyCode.M)) {
            EventDispatcher.FireEvent("FullMap");
        }


        //test
        if (Input.GetKeyDown(KeyCode.Joystick1Button9) || Input.GetKeyDown(KeyCode.T)) {
            //EventDispatcher.FireEvent("FullMap");
            GameManager.NextLevel();
        }
    }
}
