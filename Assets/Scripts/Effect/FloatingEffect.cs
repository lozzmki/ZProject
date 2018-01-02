using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingEffect : MonoBehaviour {

    public float m_fHeight = 0.8f;
    public bool Pause { get; set; } 
    float m_fFloat = 0.0f;

	// Use this for initialization
	void Start () {
        StartCoroutine(Floating());
	}
	
	// Update is called once per frame
	//void Update () {
    //    if (!Pause) {
    //        m_fFloat += Time.deltaTime;
    //        if (m_fFloat > Mathf.PI)
    //            m_fFloat -= Mathf.PI * 2.0f;
    //        gameObject.transform.position = new Vector3(0.0f, 0.5f * Mathf.Sin(m_fFloat) + m_fHeight, 0.0f) + Vector3.Scale(gameObject.transform.position, new Vector3(1, 0, 1));
    //        gameObject.transform.Rotate(Vector3.up, 80.0f * Time.deltaTime);
    //    }
    //}

    IEnumerator Floating()
    {
        while (true) {
            if(!Pause) {
                m_fFloat += Time.deltaTime;
                if (m_fFloat > Mathf.PI)
                    m_fFloat -= Mathf.PI * 2.0f;
                gameObject.transform.position = new Vector3(0.0f, 0.5f * Mathf.Sin(m_fFloat) + m_fHeight, 0.0f) + Vector3.Scale(gameObject.transform.position, new Vector3(1, 0, 1));
                gameObject.transform.Rotate(Vector3.up, 120.0f * Time.deltaTime);
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
