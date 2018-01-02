using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoFade : MonoBehaviour {

    public float m_fSetTime;
    float m_fFadePoint;
    public float m_fTime;
    float m_fSpeed;
    Vector3 m_vDirection;
	// Use this for initialization
	void Start () {
        m_fTime = m_fSetTime;
        m_fFadePoint = m_fSetTime*0.1f;
        m_fSpeed = Random.Range(2.0f, 6.0f);
        m_vDirection = new Vector3(Random.Range(0.0f, 0.5f), 1.0f, Random.Range(0.0f, 0.5f));
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Camera.main.transform.rotation;
        transform.position += m_vDirection *m_fSpeed* Time.deltaTime;
        m_vDirection += Vector3.down * 1.0f * Time.deltaTime;
        m_fTime -= Time.deltaTime;
        float _ratio;

        if(m_fTime > m_fFadePoint) {
            _ratio = 1.0f;
        }
        else {
            _ratio = m_fTime / m_fFadePoint;
        }

        if (_ratio < 0.0f) {
            Destroy(gameObject);
            return;
        }

        Vector3 _screenPt = Camera.main.WorldToScreenPoint(transform.position);
        //Debug.Log(_screenPt);

        gameObject.GetComponent<MeshRenderer>().material.SetFloat("_Alpha", _ratio);
        gameObject.GetComponent<MeshRenderer>().material.SetVector("_Position", new Vector4(_screenPt.x, _screenPt.y, _screenPt.z, 1.0f));


    }


}
