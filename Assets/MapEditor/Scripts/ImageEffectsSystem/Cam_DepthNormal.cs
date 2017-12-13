using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam_DepthNormal : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
        this.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.DepthNormals;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
