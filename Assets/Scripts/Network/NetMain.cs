using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBEngine;

public class NetMain : KBEMain {
    public static NetMain instance;

    static NetMain()
    {
        GameObject netClient = new GameObject("NetClient");
        instance = netClient.AddComponent<NetMain>();
        DontDestroyOnLoad(netClient);
    }

    public void Load()
    {
        Debug.Log("Load Network Client application");
    }
}

