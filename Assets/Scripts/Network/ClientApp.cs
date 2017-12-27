using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBEngine;

public class ClientApp : KBEMain {
    public static ClientApp instance;

    static ClientApp()
    {
        GameObject netClient = new GameObject("NetClient");
        instance = netClient.AddComponent<ClientApp>();
        DontDestroyOnLoad(netClient);
    }

    public void Load()
    {
        Debug.Log("Load Network Client application");
    }
}

