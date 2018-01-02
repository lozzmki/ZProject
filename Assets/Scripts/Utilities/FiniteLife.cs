using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteLife : MonoBehaviour {

    public float LifeTime = 1.0f;
    private Timer m_life;

	// Use this for initialization
	void Start () {
        m_life = new Timer(LifeTime);
	}
	
	// Update is called once per frame
	void Update () {
        m_life.Update();
        if (m_life.IfExpired) {
            Destroy(gameObject);
        }

	}
}
