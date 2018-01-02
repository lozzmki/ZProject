using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FloatingEffect))]
public abstract class PowerUp : MonoBehaviour {

    //float m_fT;
    bool m_bPicked = false;
    bool m_bEffect = false;
    GameObject player;
    const float c_fSpeed = 20.0f;

    public object m_arg;
	// Use this for initialization
	//void Start () {
    //    //m_fT = 0.0f;
	//}

	// Update is called once per frame
	void Update () {
        if (m_bPicked) {
            if (!m_bEffect)
                return;
            else {
                OnEffect(player);
                Destroy(gameObject);
            }
        }


        //m_fT += Time.deltaTime * 2.0f ;
        //transform.position = new Vector3(transform.position.x, Mathf.Sin(m_fT)/4.0f + 0.5f, transform.position.z);
        //transform.Rotate(Vector3.up, 180.0f * Time.deltaTime);
	}

    IEnumerator FlyTo()
    {
        Vector3 _delta;
        while (null != player) {
            _delta = player.transform.position - transform.position;
            if (_delta.magnitude > c_fSpeed * Time.deltaTime) {
                transform.position += _delta.normalized * c_fSpeed * Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }
            else {
                OnEffect(player);
                GetComponent<ParticleSystem>().Stop(true,ParticleSystemStopBehavior.StopEmitting);
                Destroy(gameObject, 1.0f);
                break;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (m_bPicked)
            return;

        if(other.tag == "Player") {
            //apply effect
            m_bPicked = true;
            Destroy(gameObject.GetComponent<Collider>());
            player = other.gameObject;
            FloatingEffect _ef = GetComponent<FloatingEffect>();
            if (_ef != null)
                _ef.Pause = true;
            StartCoroutine(FlyTo());
        }
    }

    protected abstract void OnEffect(GameObject player);

}
