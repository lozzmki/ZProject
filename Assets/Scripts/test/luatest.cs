using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class luatest : MonoBehaviour {
    float _p = 0.0f;
    int _dic = 0;
    
	// Use this for initialization
	void Start () {
        GameObject obj = new GameObject("ItemPattern");
        obj.AddComponent<Item>();
        obj.GetComponent<Item>().m_LuaScript = "ItemTest2";
        _dic = ObjectManager.getInstance().addObject(obj);
        
	}
	
	// Update is called once per frame
	void Update () {
        if (_p >= 5.0f) {
            _p -= 5.0f;
            GameObject.Instantiate(ObjectManager.getInstance().getObject(_dic),
                gameObject.transform.position + new Vector3(0.0f, 5.0f * Random.value, 0.0f),
                Random.rotation);
        }
        else
            _p += Time.deltaTime;

	}
}

