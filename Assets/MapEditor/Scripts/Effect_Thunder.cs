using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 闪电效果
/// </summary>
/// 
//[ExecuteInEditMode]
public class Effect_Thunder : MonoBehaviour {
    LineRenderer _lineRender;

    public Transform startPoint;
    public Transform target;
    [SerializeField]
    private Vector3 offset;
    //决定闪电密度
    [SerializeField]
    private float _density = 1;
    //決定閃電離散程度
    [SerializeField]
    private float _displacement = 15;
    private List<Vector3> _linePosList;
    // Use this for initialization
    void Start () {
        _lineRender = this.GetComponent<LineRenderer>();
        _linePosList = new List<Vector3>();
	}

	// Update is called once per frame
	void Update () {
        if(Time.timeScale != 0) {
            _linePosList.Clear();

            Vector3 startPos = Vector3.zero, endPos = Vector3.zero;
            startPos = startPoint.transform.position + offset;
            endPos = target.transform.position + offset;

            collectPos(startPos, endPos, _displacement);
            _linePosList.Add(endPos);

            _lineRender.numPositions=( _linePosList.Count);
            for(int i = 0; i < _linePosList.Count; i++) {
                _lineRender.SetPosition(i, _linePosList[i]);
            }

        }   	
	}

    void collectPos(Vector3 startPos,Vector3 endPos,float displace) {
        if (displace < _density) {
            _linePosList.Add(startPos);
        }else {
            float midX = (startPos.x + endPos.x) / 2;
            float midY = (startPos.y + endPos.y) / 2;
            float midZ = (startPos.z + endPos.z) / 2;

            midX += (float)(UnityEngine.Random.value - 0.5) * displace;
            midY += (float)(UnityEngine.Random.value - 0.5) * displace;
            midZ += (float)(UnityEngine.Random.value - 0.5) * displace;

            Vector3 midPos = new Vector3(midX, midY, midZ);

            collectPos(startPos, midPos, displace / 2);
            collectPos(midPos, endPos, displace / 2);
        }


    }
}
