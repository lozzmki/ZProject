using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ttest : MonoBehaviour, nglib.IEventListener {
    private float m_fTime;
	// Use this for initialization
	void Start () {
        nglib.EventDispatcher.getInstance().addListener(nglib.EventType.EVENT_DEFAULT, this);
        m_fTime = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        m_fTime += Time.deltaTime;
        if (m_fTime > 1.0f)
        {
            m_fTime = 0.0f;
            nglib.EventDispatcher.getInstance().fireEvent(new nglib.Event(nglib.EventType.EVENT_DEFAULT));
        }
	}

    public bool handleEvent(nglib.Event e)
    {

        
        switch (e._nType)
        {
            case nglib.EventType.EVENT_DEFAULT:
                Debug.Log("Received!");
                break;
        }

        return false;
    }
}
