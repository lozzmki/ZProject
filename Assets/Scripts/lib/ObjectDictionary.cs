using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDictionary{
    static ObjectDictionary s_ItemDic;
    //static ObjectDictionary s_ProjDic;

    Dictionary<string, GameObject> s_Dic;

    private ObjectDictionary()
    {
        s_Dic = new Dictionary<string, GameObject>();
    }

    public static ObjectDictionary GetItemDic()
    {
        if(s_ItemDic == null) {
            s_ItemDic = new ObjectDictionary();
        }
        return s_ItemDic;
    }

//     public static ObjectDictionary GetProjectileDic()
//     {
//         if (s_ProjDic == null) {
//             s_ProjDic = new ObjectDictionary();
//         }
//         return s_ProjDic;
//     }

    public void AddObject(string sName, GameObject obj)
    {
        if (!s_Dic.ContainsKey(sName)) {
            s_Dic.Add(sName, obj);
        }
        else {
            s_Dic[sName] = obj;
        }
    }

    public GameObject CreateObject(string sName, Vector3 pos, Quaternion rotation)
    {
        if (s_Dic.ContainsKey(sName)) {
            GameObject _obj = GameObject.Instantiate(s_Dic[sName], pos, rotation);
            Transceiver.SendSignal(new DSignal(null, s_Dic[sName], "Duplicate", _obj));
            return _obj;
        }
        return null;
    }
}
