using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testClass{
    public int aa;
}

public class ttest : MonoBehaviour {
    public testClass tc;
    
	// Use this for initialization
	void Awake () {
        tc = new testClass();
        Debug.Log("newd");
	}
	
	// Update is called once per frame
	void Update () {

	}


}
