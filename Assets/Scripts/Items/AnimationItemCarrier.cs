using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationItemCarrier : MonoBehaviour {

    [HideInInspector] public GameObject item;

	// Use this for initialization
	void Start () {
        float _fAngle = Random.Range(0.0f, 360.0f);
        Vector3 _vForce = new Vector3(Mathf.Cos(_fAngle)* Random.value,  1.0f, Mathf.Sin(_fAngle)* Random.value);

        GetComponent<Rigidbody>().velocity = _vForce*5.0f;
        StartCoroutine(Wait());
	}

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1.5f);
        GetComponent<ParticleSystem>().Stop();
        item.transform.position = transform.position;
        item.transform.parent = transform.parent;
        Destroy(gameObject, 1.0f);
    }
}
