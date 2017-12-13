using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 地图编辑器所用的的摄像机
/// 鼠标触控或键盘上下左右操控
/// 鼠标滚轮可以控制缩放
/// </summary>
public class CamMap : MonoBehaviour {
    [SerializeField]
    private Vector3 _targetPoint;
    [SerializeField]
    private float _smooth = 0.5f;
    [SerializeField]
    private float _moveSpeed = 2f;
    [SerializeField]
    private float _maxCamHigh;
    [SerializeField]
    private float _minCamHigh;
    
    // Use this for initialization
	void Start () {
        _targetPoint = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        _HandleMouseInput();
        _HandleKeyBoardInput();
        //update position
        this.transform.position = Vector3.Lerp(this.transform.position, _targetPoint, _smooth * Time.deltaTime);
    }
    void _HandleKeyBoardInput() {
        var left = Vector3.Cross(Vector3.up, transform.forward);
        //move
        if (Input.GetKey(KeyCode.W)) {
            _targetPoint += transform.forward * _moveSpeed * Time.deltaTime;
            
        } else if (Input.GetKey(KeyCode.S)) {
            _targetPoint -= transform.forward * _moveSpeed * Time.deltaTime;
            
        } else if (Input.GetKey(KeyCode.A)) {
            _targetPoint -= left * _moveSpeed * Time.deltaTime;
            
        } else if (Input.GetKey(KeyCode.D)) {
            _targetPoint += left * _moveSpeed * Time.deltaTime;
        }
    }
    void _HandleMouseInput() {
        /*
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                _targetPoint = hit.point;
            }
            _targetPoint.y = this.transform.position.y;
        }*/

        var highDelta = Input.GetAxis("Mouse ScrollWheel") * 30;
        _targetPoint.y -= highDelta * _moveSpeed * Time.deltaTime;
        if (highDelta > 0) {
            if (_targetPoint.y > _maxCamHigh) {
                _targetPoint.y = _maxCamHigh;
            }
        }else if (highDelta < 0) {
            if (_targetPoint.y < _minCamHigh) {
                _targetPoint.y = _minCamHigh;
            }
        }
    }
}
