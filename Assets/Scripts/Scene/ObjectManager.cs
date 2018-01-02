using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager {

    public static Dictionary<int, GameObject> _s_Dic;
    public static void Init()
    {
        _s_Dic = new Dictionary<int, GameObject>();
    }

    public static void Add(string sName, GameObject obj)
    {
        _s_Dic.Add(sName.GetHashCode(), obj);
    }
    public static GameObject Get(int key)
    {
        if (_s_Dic.ContainsKey(key))
            return _s_Dic[key];
        return null;
    }
}
