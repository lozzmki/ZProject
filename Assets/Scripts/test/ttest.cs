using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class testClass{
    public int aa;
   [System.NonSerialized] public int bb;
}

public class ttest : MonoBehaviour {
    public testClass tc;
    
	// Use this for initialization
	void Awake () {
        //tc = new testClass();
        //Debug.Log("newd");
	}
	
	// Update is called once per frame
	void Update () {

	}


}
