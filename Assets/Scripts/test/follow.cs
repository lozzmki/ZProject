using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow : MonoBehaviour {
    public Transform m_Player;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 _pos = m_Player.position + new Vector3(0.0f, 10.0f, -10.0f);// + m_Player.forward * -2.0f;
        this.transform.position = _pos;
        this.transform.LookAt(m_Player);
        //this.transform.forward = m_Player.forward;
	}
}
