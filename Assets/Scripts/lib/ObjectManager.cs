using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager {

    static ObjectManager _inst;
    public static ObjectManager GetInstance()
    {
        if(_inst == null)
        {
            _inst = new ObjectManager();
        }
        return _inst;
    }
    private ObjectManager()
    {
        m_dicObjects = new Dictionary<int, GameObject>();
    }
    private Dictionary<int, GameObject> m_dicObjects;

    public void AddObject(GameObject obj)
    {
        int _key = obj.GetInstanceID();
        if (m_dicObjects.ContainsKey(_key))
            m_dicObjects.Remove(_key);

        m_dicObjects.Add(_key, obj);
    }
    public void DestroyObject(int ID)
    {
        if (m_dicObjects.ContainsKey(ID))
        {
            GameObject.Destroy(m_dicObjects[ID]);
            m_dicObjects.Remove(ID);
        }
    }
    public void DestroyObject(GameObject obj)
    {
        int _key = obj.GetInstanceID();
        if (m_dicObjects.ContainsKey(_key)) {
            GameObject.Destroy(m_dicObjects[_key]);
            m_dicObjects.Remove(_key);
        }
    }
    public GameObject GetObject(int ID)
    {
        if (m_dicObjects.ContainsKey(ID))
        { 
            return m_dicObjects[ID];
        }
        return null;
    }

    public Dictionary<int, GameObject> GetDic()
    {
        return m_dicObjects;
    }

}
