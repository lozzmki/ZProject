using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControl : MonoBehaviour {

	void Update ()
    {
        this.transform.GetComponent<Light>().intensity = Random.Range(4f, 5f);
	}
}
