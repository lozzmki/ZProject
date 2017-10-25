using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class luatest : MonoBehaviour {
    float _p = 0.0f;

    

	// Use this for initialization
	void Start () {

        //         GameObject obj = new GameObject("ItemPattern");
        //         obj.AddComponent<Item>();
        //         obj.GetComponent<Item>().m_LuaScript = "ItemRangePattern";
        //         _dic = ObjectManager.getInstance().addObject(obj);
        // 
        // 
        //         GameObject oobj = new GameObject("ItemPattern2");
        //         oobj.AddComponent<Item>();
        //         oobj.GetComponent<Item>().m_LuaScript = "ItemMeleePattern";
        //         _dic2 = ObjectManager.getInstance().addObject(oobj);

        //Debug.Log(Resources.Load("Meshes/Shell"));

        GameObject _item = ObjectDictionary.GetItemDic().CreateObject("ItemRangePattern",
                 gameObject.transform.position + new Vector3(0.0f, 5.0f * Random.value, 0.0f),
                 Random.rotation);
        Transceiver.SendSignal(new DSignal(null, gameObject, "Pickup", _item));

        _item = ObjectDictionary.GetItemDic().CreateObject("ItemPartsPattern",
                 gameObject.transform.position + new Vector3(0.0f, 5.0f * Random.value, 0.0f),
                 Random.rotation);
        Transceiver.SendSignal(new DSignal(null, gameObject, "Pickup", _item));
    }

    // Update is called once per frame
    void Update () {
        if (_p >= 5.0f) {
            _p -= 5.0f;
            //             GameObject.Instantiate(ObjectManager.getInstance().getObject(_dic),
            //                 gameObject.transform.position + new Vector3(0.0f, 5.0f * Random.value, 0.0f),
            //                 Random.rotation);
            //             GameObject.Instantiate(ObjectManager.getInstance().getObject(_dic2),
            //                 gameObject.transform.position + new Vector3(0.0f, 5.0f * Random.value, 0.0f),
            //                 Random.rotation);
//             ObjectDictionary.GetItemDic().CreateObject("ItemArmorPattern",
//                 gameObject.transform.position + new Vector3(0.0f, 5.0f * Random.value, 0.0f),
//                 Random.rotation);
        }
        else
            _p += Time.deltaTime;

	}
}

