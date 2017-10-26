using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {
	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Transceiver>().AddResolver("Move",MoveTowards);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MoveTowards(DSignal signal){
        Vector3 vDirection = (Vector3)signal._arg1;
        //Position
        gameObject.transform.position += vDirection.normalized * gameObject.GetComponent<EntityAttribute>().m_Properties[EntityAttribute.SPEED].d_Value * Time.deltaTime;
        //Rotation
        //         Vector3 _vTurn = gameObject.transform.InverseTransformDirection(vDirection);
        //         float _fAngle = Mathf.Atan2(_vTurn.x, _vTurn.z) * Mathf.Rad2Deg;
        //         float _fRotation = 3500.0f * Time.deltaTime;
        // 
        //         if (Mathf.Abs(_fAngle) < _fRotation)
        //             _fRotation = _fAngle;
        //         else {
        //             if (_fAngle < 0.0f)
        //                 _fRotation = -_fRotation;
        //         }
        //         gameObject.transform.Rotate(0.0f, _fRotation, 0.0f);
        gameObject.transform.forward = vDirection;
   }

 }
