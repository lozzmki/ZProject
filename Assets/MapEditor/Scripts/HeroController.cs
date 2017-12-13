using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour {
    [SerializeField]
    Transform _hero;

    [SerializeField]
    float walk_speed;
	// Use this for initialization
	void Start () {
        
	}

    // Update is called once per frame
    void Update() {
        HandleRotateInput();
        HandleMoveInput();

        //TestGun
        if (Input.GetButton("Fire1")) {
            this.GetComponentInChildren<BigGun>().UseWeapon();
        }

        //StartCoroutine(testGun());


    }
    IEnumerator testGun() {
        while (true) {
            this.GetComponentInChildren<BigGun>().UseWeapon();
            yield return new WaitForSeconds(0.1f);
        }
    }
    void HandleRotateInput() {
        //rotate
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Vector3 point = Vector3.zero;
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            point = hit.point;
        }

        point.y = _hero.transform.position.y;

        _hero.LookAt(point);
    }
    void HandleMoveInput() {
        var left = Vector3.Cross(Vector3.up, _hero.forward);
        //move
        if (Input.GetKey(KeyCode.W)) {
            var newPos = _hero.position + _hero.forward * walk_speed * Time.deltaTime;
            _hero.position = newPos;
        } else if (Input.GetKey(KeyCode.S)) {
            var newPos = _hero.position - _hero.forward * walk_speed * Time.deltaTime;
            _hero.position = newPos;
        } else if (Input.GetKey(KeyCode.A)) {
            var newPos = _hero.position - left * walk_speed * Time.deltaTime;
            _hero.position = newPos;
        } else if (Input.GetKey(KeyCode.D)) {
            var newPos = _hero.position + left * walk_speed * Time.deltaTime;
            _hero.position = newPos;
        }
    }
}
