using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 管理各種資源
/// </summary>
public class ResrouceManager : MonoBehaviour {

    //SCENEOBJ
    public GameObject P_ROCK;
    public GameObject P_TREE;
    public GameObject P_FLAG;
    public GameObject P_BOX;
    public GameObject P_DOOR;
    public GameObject P_DICI;
    public GameObject P_WALL_1;
    public GameObject P_WALL_CORNET_1;
    public GameObject P_DOOR0;

    public SCENEOBJ_TYPE NameToSceneObj(string name)
    {
        switch (name)
        {
            case "ROCK":return SCENEOBJ_TYPE.ROCK;
            case "TREE":return SCENEOBJ_TYPE.TREE;
            case "FLAG":return SCENEOBJ_TYPE.FLAG;
            case "BOX1":return SCENEOBJ_TYPE.BOX;
            case "DOOR1":return SCENEOBJ_TYPE.DOOR;
            case "DICI":return SCENEOBJ_TYPE.DICI;
            case "WALL_1":return SCENEOBJ_TYPE.WALL_1;
            case "WALL_CORNER_1": return SCENEOBJ_TYPE.WALL_CORNER_1;
            case "DOOR0":return SCENEOBJ_TYPE.DOOR0;
            default:
                Debug.LogError("can't find the resources by name:" + name);
                return SCENEOBJ_TYPE.NONE;
        }
    }
    //FLOOR MAT
    public Material[] floorMats;

    //UI
    public GameObject P_TouchedTag;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
