using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryBar : MonoBehaviour {

	// Use this for initialization
	//void Start () {
	//	
	//}
	
	// Update is called once per frame
	//void Update () {
	//	
	//}

    public void IndicateValue(float main, float sub = -1.0f)
    {
        GetComponent<Image>().material.SetFloat("_Percent", main);
        if (sub < 0.0f)
            sub = main;
        GetComponent<Image>().material.SetFloat("_SubPercent", sub);
    }
}
