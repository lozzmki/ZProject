using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
public class Cell : MonoBehaviour {

    //此单元格中容纳的
    public Transform container;

    [SerializeField]
    bool isDirt = true;

    public Transform    kuang;

    [SerializeField]
    private COLOR_TYPE _colorType;
    [SerializeField]
    private Color color_normal;
    [SerializeField]
    private Color color_debug;
    public Vector2     mapPos;


    public SCENEOBJ_TYPE sceneObj_type = SCENEOBJ_TYPE.NONE;

    public void ChangeColor(COLOR_TYPE  colorType) {
        this._colorType = colorType;
        switch (_colorType) {
            case COLOR_TYPE.COLOR_DEBUG:
                this.GetComponent<SpriteRenderer>().color = color_debug;
                break;
            case COLOR_TYPE.COLOR_NORMAL:
                this.GetComponent<SpriteRenderer>().color = color_normal;
                break;
            default:
                break;
        }
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    /// <summary>
    /// 初始化函數
    /// </summary>
    /// 
    public void Initilize()
    {
        //this.GetComponent<SpriteRenderer>().color = color_normal;
    }
    public void BuildBox_Editor()
    {
        BuildBox(sceneObj_type);
    }
    [SerializeField]
    private GameObject _building;
    public SceneObj GetBuilding
    {
        get {
            if (_building != null)
            {
                return _building.GetComponent<SceneObj>();
            }
            else
            {
                return null;
            }
             }
    }
    //在该格上建立方块
    public bool BuildBox(SCENEOBJ_TYPE type) {
        this.sceneObj_type = type;
        if (type == SCENEOBJ_TYPE.NONE)
        {
            DestroyImmediate(_building);
            GameObject.Find("CellContainer").GetComponent<CellManager>().setCellUnUsed((int)mapPos.x, (int)mapPos.y);
            return true;
        }
        Debug.Log("BEGIN BUILD: " + type);
        if (GameObject.Find("CellContainer").
            GetComponent<CellManager>().getIfUsed((int)mapPos.x, (int)mapPos.y)) {
            
            Debug.LogWarning("this cell already has static object");
            return false;
        }

        Transform obj = null;

        var rsc = GameObject.Find("CellContainer").GetComponent<ResrouceManager>();
        switch (sceneObj_type) {
            case SCENEOBJ_TYPE.ROCK:
                obj = Instantiate(rsc.P_ROCK.transform);
                break;
            case SCENEOBJ_TYPE.FLAG:
                obj = Instantiate(rsc.P_FLAG.transform);
                break;
            case SCENEOBJ_TYPE.TREE:
                obj = Instantiate(rsc.P_TREE.transform);
                break;
            case SCENEOBJ_TYPE.BOX:
                obj = Instantiate(rsc.P_BOX.transform);
                break;
            case SCENEOBJ_TYPE.DOOR:
                obj = Instantiate(rsc.P_DOOR.transform);
                break;
            case SCENEOBJ_TYPE.DICI:
                obj = Instantiate(rsc.P_DICI.transform);
                break;
            case SCENEOBJ_TYPE.WALL_1:
                obj = Instantiate(rsc.P_WALL_1.transform);
                break;
            case SCENEOBJ_TYPE.DOOR0:
                obj = Instantiate(rsc.P_DOOR0.transform);
                break;
            case SCENEOBJ_TYPE.WALL_CORNER_1:
                obj = Instantiate(rsc.P_WALL_CORNET_1.transform);
                break;
            default:return false; 
        }
        if(obj!=null) {
            _building = obj.gameObject;
            obj.transform.parent = GameObject.Find("BuildingContainer").transform;
            obj.transform.position = this.transform.position;
            obj.GetComponent<SceneObj>().m_FatherCell = this;
        }
        
        GameObject.Find("CellContainer").GetComponent<CellManager>().setCellUsed((int)mapPos.x, (int)mapPos.y);
        return true;
    }
    //floorMat
    private int m_FloorMatId = -1;
    public int GetMatId
    {
        get { return m_FloorMatId; }
    }
    public void ChangeFoorMat(int id)
    {
        if (id < 0)
        {
            m_FloorMatId = -1;
            this.GetComponent<MeshRenderer>().sharedMaterial = null;
        }
        var mat = GameObject.Find("CellContainer").GetComponent<ResrouceManager>().floorMats[id];
        m_FloorMatId = id;
        this.GetComponent<MeshRenderer>().sharedMaterial = mat;
    }
    
    //被點擊
    private void OnMouseDown() {
        Debug.Log("CLICK ME:" + this.transform);
        GameObject.Find("GameStone").GetComponent<CellTouchManager>().TouchCell(this);
    }
    /*
    private void OnMouseDown()
    {
        if(mapPos.y >= 29)
        {
            return;
        }
        if(EventSystem.current.IsPointerOverGameObject())
        {
            //是UI的时候，执行相关的UI操作
            Debug.Log("是UI");
            return;
        }
        GameObject.Find("GameStone").GetComponent<TotalTouchManager>().selectCell(transform);
    }*/
}

public enum SCENEOBJ_TYPE {
    NONE,DOOR0,ROCK,TREE,FLAG,BOX,DICI,DOOR,WALL_1,WALL_CORNER_1
}
public enum COLOR_TYPE{
    COLOR_NORMAL,COLOR_DEBUG
}

