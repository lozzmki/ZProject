using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellTouchManager : MonoBehaviour {
    [SerializeField]
    private Cell _lastTouchedCell;
    public Cell getTouchedCell {
        get { return _lastTouchedCell; }
    }
    [SerializeField]
    private float _tagHigh = 2f;

    private GameObject _tagObj;

   
	// Use this for initialization
	void Start () {
        _tagObj = Instantiate(GameObject.Find("GameStone").GetComponent<ResrouceManager>().P_TouchedTag);
        _tagObj.transform.position = new Vector3(-1000, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {

        _PosSelect();

        _checkIfBuild();
    }
    public void beginPosSelect() {
        _posSelectTag = true;
    }
    public void endPosSelect() {
        _posSelectTag = false;
    }
    public void _PosSelect() {
        if (!_posSelectTag) {
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit)) {
            var cell = hit.collider.gameObject.GetComponent<Cell>();
            if (cell != null) {
                _TouchCell(cell);
            }
        }
    }
    //对外接口
    public void TouchCell(Cell cell) {
        if (!_posSelectTag) {
            _TouchCell(cell);
        }
    }
    //对内接口
    public void _TouchCell(Cell cell) {
        if (!_lastTouchedCell) {
            _lastTouchedCell = cell;
            _MoveTag();
        }/*
        if(cell == _lastTouchedCell) {
            _lastTouchedCell = null;
            _MoveTag(true);
            return;
        }*/
        else if (cell != _lastTouchedCell) {
            _lastTouchedCell = cell;
            _MoveTag();
        }
        _updateVirtualObjPos(cell);
    }
    void _MoveTag(bool isCancel = false) {
        if (isCancel) {
            _tagObj.transform.position = new Vector3(-1000, 0, 0);
            return;
        }
        _tagObj.transform.position = new Vector3(_lastTouchedCell.transform.position.x,
                                                _tagHigh,
                                                _lastTouchedCell.transform.position.z);
     }



    //Cell Build 函数
  

    [SerializeField]
    private GameObject _lastVirtualObj;
    //virtual 相关
    [SerializeField]
    //允许放置物品的TAG
    private bool _posSelectTag = false;
    //当前选中的物件类型
    private SCENEOBJ_TYPE to_build_type;
    public void TouchedCellBuild(SCENEOBJ_TYPE type) {
        beginPosSelect();
        virtualObj(type);
        to_build_type = type;
    }
    void _updateVirtualObjPos(Cell cell) {
        if (_lastVirtualObj) {
            _lastVirtualObj.transform.position = cell.transform.position;
        }
    }
    void _checkIfBuild() {
        if(_posSelectTag && Input.GetMouseButtonDown(0)) {
            _lastTouchedCell.GetComponent<Cell>().BuildBox(to_build_type);
            endPosSelect();
            DestroyImmediate(_lastVirtualObj);
            _lastVirtualObj = null;
        }
    }
    public void virtualObj(SCENEOBJ_TYPE type) {
        if (_lastVirtualObj != null) {
            DestroyImmediate(_lastVirtualObj);
        }
        var rsc = GameObject.Find("GameStone").GetComponent<ResrouceManager>();
        switch (type) {
            case SCENEOBJ_TYPE.BOX:
                _lastVirtualObj = Instantiate(rsc.P_BOX);
                break;
            case SCENEOBJ_TYPE.DICI:
                _lastVirtualObj = Instantiate(rsc.P_DICI);
                break;
            case SCENEOBJ_TYPE.FLAG:
                _lastVirtualObj = Instantiate(rsc.P_FLAG);
                break;
            case SCENEOBJ_TYPE.DOOR:
                _lastVirtualObj = Instantiate(rsc.P_DOOR);
                break;
            case SCENEOBJ_TYPE.ROCK:
                _lastVirtualObj = Instantiate(rsc.P_ROCK);
                break;
            case SCENEOBJ_TYPE.TREE:
                _lastVirtualObj = Instantiate(rsc.P_TREE);
                break;
            default:
                break;
        }
    }
    

    public void TouchedCellBuild_Tree() {
        Debug.Log("BUILD TREE");
        TouchedCellBuild(SCENEOBJ_TYPE.TREE);
    }
    public void TouchedCellBuild_Rock() {
        TouchedCellBuild(SCENEOBJ_TYPE.ROCK);
    }
    public void TouchedCellBuild_Flag() {
        TouchedCellBuild(SCENEOBJ_TYPE.FLAG);
    }
    public void TouchedCellBuild_Box() {
        TouchedCellBuild(SCENEOBJ_TYPE.BOX);
    }
    public void TouchedCellBuild_Door() {
        TouchedCellBuild(SCENEOBJ_TYPE.DOOR);
    }
    public void TouchedCellBuild_Dici() {
        TouchedCellBuild(SCENEOBJ_TYPE.DICI);
    }
}
