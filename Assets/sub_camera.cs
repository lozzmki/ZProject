using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sub_camera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Camera.main.transform.rotation;
        transform.position = -0.1f * transform.forward;
	}
}
