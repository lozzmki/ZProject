using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    [HideInInspector]public float m_Damage = 0;
    [HideInInspector]public float m_Speed = 30.0f;
    [HideInInspector]public GameObject m_Master;
    private float m_fLife = 10.0f;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        m_fLife -= Time.deltaTime;
        if(m_fLife < 0.0f) {
            Destroy(gameObject);
        }

        //movement
        gameObject.transform.position += gameObject.transform.forward.normalized * m_Speed * Time.deltaTime;
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject != m_Master && other.gameObject.GetComponent<Entity>() != null) {
            Destroy(gameObject);
            //damage
            Transceiver.SendSignal(new DSignal(m_Master, other.gameObject, "Damage", m_Damage));
        }
    }
}
