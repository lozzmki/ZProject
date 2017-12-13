using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 相机跟随一个目标，角度不变，只变更x和z坐标
/// </summary>
public class CamTarget : MonoBehaviour {

    [SerializeField]
    private Transform _Target;

    private Vector3 _TargetPos;
    [SerializeField]
    private float smoothTime = 0.5f;

    [SerializeField]
    public Vector2 offset;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
	}
    void FixedUpdate() {
        _TargetPos = _Target.transform.position;
        _TargetPos.y = this.transform.position.y;
        _TargetPos.x += offset.x;
        _TargetPos.z += offset.y;

        
        this.transform.position = Vector3.Lerp(this.transform.position, _TargetPos, smoothTime * Time.deltaTime);
        
    }
}
