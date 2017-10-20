using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager {

    static ObjectManager _inst;
    public static ObjectManager getInstance()
    {
        if(_inst == null)
        {
            _inst = new ObjectManager();
        }
        return _inst;
    }
    private ObjectManager()
    {
        m_nNextID = 0;
        m_dicObjects = new Dictionary<int, GameObject>();
    }

    private int m_nNextID;
    private Dictionary<int, GameObject> m_dicObjects;

    public int addObject(GameObject obj)
    {
        m_dicObjects.Add(m_nNextID, obj);
        return m_nNextID++;
    }
    public void removeObject(int ID)
    {
        if (m_dicObjects.ContainsKey(ID))
        {
            m_dicObjects.Remove(ID);
        }
    }
    public GameObject getObject(int ID)
    {
        if (m_dicObjects.ContainsKey(ID))
        {
            return m_dicObjects[ID];
        }
        return null;
    }

    public Dictionary<int, GameObject> getDic()
    {
        return m_dicObjects;
    }

}
