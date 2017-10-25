using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ttest : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.position += gameObject.transform.forward.normalized * Time.deltaTime * 10.0f;
	}


}
