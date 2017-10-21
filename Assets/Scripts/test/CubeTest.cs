using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CubeTest : MonoBehaviour {
    public GameObject m_Prefab;
    public float m_Interval;
    private float m_fTime;
	// Use this for initialization
	void Start () {
        createCube();
        m_fTime = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        m_fTime += Time.deltaTime;
        if (m_fTime > m_Interval)
        {
            m_fTime -= m_Interval;
            createCube();
        }
    }

    public void createCube()
    {
        //GameObject _obj = Instantiate(m_Prefab, gameObject.transform.position + 10.0f * new Vector3(Random.value, 0.1f, Random.value), Random.rotation);
        //int _id = ObjectManager.getInstance().addObject(_obj);
        //_obj.GetComponent<Attribute>().m_ID = _id;
    }

}
