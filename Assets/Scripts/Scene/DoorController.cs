using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {

    public bool m_DoorState = false;
    public bool m_Locked = false;
    public float m_LiftHeight = 4.0f;
    public float m_LiftSpeed = 1.0f;
    private float m_CurrentHeight = 0.0f;
    private Timer m_CloseTimer;

	// Use this for initialization
	void Start () {
        m_CloseTimer = new Timer(2.0f);
	}
	
	// Update is called once per frame
	void Update () {
        if (m_DoorState) {
            if (m_CloseTimer.IfExpired) {
                m_DoorState = false;
                return;
            }
            if(m_CurrentHeight< m_LiftHeight) {
                gameObject.transform.position += Vector3.up * m_LiftSpeed * Time.deltaTime;
                m_CurrentHeight += Time.deltaTime * m_LiftSpeed;
            }
            else {
                m_CloseTimer.Update();
            }
        }
        else {
            if (m_CurrentHeight > 0.0f) {
                gameObject.transform.position += Vector3.down * m_LiftSpeed * Time.deltaTime;
                m_CurrentHeight -= Time.deltaTime * m_LiftSpeed;
            }
        }
	}
    private void OnTriggerEnter(Collider other)
    {
        if (m_DoorState || m_Locked)
            return;
        if(other.gameObject.GetComponent<Entity>() != null) {
            if (m_CloseTimer == null)
                return;
            m_CloseTimer.Reset();
            m_DoorState = true;
        }
    }
}
