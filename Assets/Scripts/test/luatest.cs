using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class luatest : MonoBehaviour {
    float _p = 0.0f;

    public GameObject _testAI;

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

        GameObject _obj = ObjectDictionary.GetItemDic().CreateObject("ItemRangePattern",
                 gameObject.transform.position + new Vector3(5.0f * Random.value, 0.0f, 5.0f * Random.value),
                 Quaternion.AngleAxis(0,Vector3.up));

        ObjectDictionary.GetItemDic().CreateObject("ItemRangePattern",
                 gameObject.transform.position + new Vector3(5.0f * Random.value, 0.0f, 5.0f * Random.value),
                 Quaternion.AngleAxis(0, Vector3.up));

        ObjectDictionary.GetItemDic().CreateObject("ItemPartsPattern",
                 gameObject.transform.position + new Vector3(5.0f * Random.value, 0.0f, 5.0f * Random.value),
                 Quaternion.AngleAxis(0,Vector3.up));
        ObjectDictionary.GetItemDic().CreateObject("CubeAmmo",
                 gameObject.transform.position + new Vector3(5.0f * Random.value, 0.0f, 5.0f * Random.value),
                 Quaternion.AngleAxis(0,Vector3.up));
        ObjectDictionary.GetItemDic().CreateObject("ItemPartsPattern",
                 gameObject.transform.position + new Vector3(5.0f * Random.value, 0.0f, 5.0f * Random.value),
                 Quaternion.AngleAxis(0,Vector3.up));
        ObjectDictionary.GetItemDic().CreateObject("CubeAmmo",
                 gameObject.transform.position + new Vector3(5.0f * Random.value, 0.0f, 5.0f * Random.value),
                 Quaternion.AngleAxis(0,Vector3.up));
        ObjectDictionary.GetItemDic().CreateObject("ItemPartsPattern",
                 gameObject.transform.position + new Vector3(5.0f * Random.value, 0.0f, 5.0f * Random.value),
                 Quaternion.AngleAxis(0,Vector3.up));
        ObjectDictionary.GetItemDic().CreateObject("CubeAmmo",
                 gameObject.transform.position + new Vector3(5.0f * Random.value, 0.0f, 5.0f * Random.value),
                 Quaternion.AngleAxis(0,Vector3.up));


        //map test
        DCell W = new DCell(0, false, true, false);
        DCell R = new DCell();
        DCell D = new DCell(0, true, false);

        DRoomPreset _room1 = new DRoomPreset();
        _room1.d_Size = new DSizeN(7, 7);
        _room1.d_Data = new DCell[]
        {
            W,W,W,D,W,W,W,
            W,R,R,R,R,R,W,
            W,R,R,R,R,R,W,
            D,R,R,R,R,R,D,
            W,R,R,R,R,R,W,
            W,R,R,R,R,R,W,
            W,W,W,D,W,W,W,
        };

        DRoomPreset _room2 = new DRoomPreset();
        _room2.d_Size = new DSizeN(5, 13);
        _room2.d_Data = new DCell[]
        {
            W,W,W,D,W,
            W,R,R,R,W,
            W,R,R,R,W,
            W,R,R,R,W,
            W,R,R,R,W,
            D,R,R,R,W,
            W,R,R,R,D,
            W,R,R,R,W,
            W,R,R,R,W,
            W,R,R,R,W,
            W,R,R,R,W,
            D,R,R,R,W,
            W,W,D,W,W,
        };

        DRoomPreset _room3 = new DRoomPreset();
        _room3.d_Size = new DSizeN(13, 5);
        _room3.d_Data = new DCell[]
        {
            W,W,D,W,W,W,W,W,D,W,W,W,W,
            W,R,R,R,R,R,R,R,R,R,R,R,W,
            W,R,R,R,R,R,R,R,R,R,R,R,W,
            D,R,R,R,R,R,R,R,R,R,R,R,D,
            W,W,W,W,W,D,W,W,W,W,W,D,W,
        };

        RoomPresetsManager.GetInstance().AddPreset(_room1);
        RoomPresetsManager.GetInstance().AddPreset(_room2);
        RoomPresetsManager.GetInstance().AddPreset(_room3);

        Stage _stage = MapGenerator.NewMap(100);
        _stage.CreateStage();
    }

    // Update is called once per frame
    void Update () {
        if (_p >= 5.0f) {
            _p -= 5.0f;
            //             GameObject.Instantiate(ObjectManager.getInstance().getObject(_dic),
            //                 gameObject.transform.position + new Vector3(0.0f, 5.0f * Random.value, 0.0f),
            //                 Quaternion.AngleAxis(0,Vector3.up));
            //             GameObject.Instantiate(ObjectManager.getInstance().getObject(_dic2),
            //                 gameObject.transform.position + new Vector3(0.0f, 5.0f * Random.value, 0.0f),
            //                 Quaternion.AngleAxis(0,Vector3.up));
//             ObjectDictionary.GetItemDic().CreateObject("ItemArmorPattern",
//                 gameObject.transform.position + new Vector3(0.0f, 5.0f * Random.value, 0.0f),
//                 Quaternion.AngleAxis(0,Vector3.up));
        }
        else
            _p += Time.deltaTime;

	}
}

