using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;



public class Preloader : MonoBehaviour {

	[RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        CheckUpdates();
        LoadObjects();
        LoadNewObjects();

        //test
        StartGame();
        //test end
    }

    static void LoadObjects()
    {
        //not tested on real device yet

        ItemPool.InitPool();
        //Items
        DirectoryInfo _info = new DirectoryInfo("Assets/Scripts/Lua/Items");
        FileInfo[] _files = _info.GetFiles("*.txt");
        for (int i = 0; i < _files.Length; i++) {
            string _sName = _files[i].Name.Split('.')[0];
            GameObject obj = new GameObject(_sName);
            obj.AddComponent<SphereCollider>().isTrigger = true;
            obj.GetComponent<SphereCollider>().radius = 1.0f;
            obj.AddComponent<FloatingEffect>();
            obj.AddComponent<Item>().m_LuaScript = _sName;
            obj.GetComponent<Item>().InitFromLuaFile();
            //ObjectDictionary.GetItemDic().AddObject(_sName, obj);
            ItemPool.AddItem(obj);
            obj.transform.position += new Vector3(0, 0, 10000);
            
        }

        //load Object presets
        ObjectManager.Init();
        //SceneObjects
        GameObject[] _objs = Resources.LoadAll<GameObject>("Prefabs/SceneObjects");
        for(int i= 0; i<_objs.Length; i++) { 
            ObjectManager.Add(_objs[i].name, _objs[i]);
        }
        //Mobs
        _objs = Resources.LoadAll<GameObject>("Prefabs/Mobs");
        for (int i = 0; i < _objs.Length; i++) {
            ObjectManager.Add(_objs[i].name, _objs[i]);
        }

        //RoomPresets
        _info = new DirectoryInfo("Assets/Data/RoomPresets");
        _files = _info.GetFiles("*.rmp");
        for(int i=0; i<_files.Length; i++) {
            RoomPresetsManager.LoadPreset(_files[i].Name);
        }

        //Projectiles,to modify..........
//         _info = new DirectoryInfo("Assets/Resources/Prefabs/Projectiles");
//         _files = _info.GetFiles("*.prefab");
//         for (int i = 0; i < _files.Length; i++) {
//             string _sName = _files[i].Name.Split('.')[0];
//             GameObject _obj = (GameObject)Resources.Load("Prefabs/Projectiles/" + _sName);
//             _obj = Instantiate(_obj);
//             ObjectDictionary.GetProjectileDic().AddObject(_sName, _obj);
//         }
    }

    static void CheckUpdates()
    {
        //download new assets
    }

    static void LoadNewObjects()
    {
        //load the scripts from updates
    }

    static void StartGame()
    {
        GameManager.Init();
        GameManager.NextLevel();
    }
}
