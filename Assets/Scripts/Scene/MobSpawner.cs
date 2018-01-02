using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour {

    private GameObject m_Prefab;

	// Use this for initialization
	void Start () {
        //FIN:temporary hard coded
        //m_Prefab = Resources.Load<GameObject>("Prefabs/Mobs/Mob");
        m_Prefab = ObjectManager.Get(GetComponent<Transceiver>()._dataCache32);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player") {
            //Spawn Enemy
            if(m_Prefab != null) {
                GameObject _obj = Instantiate(m_Prefab);
                //TODO:random skill sets
                _obj.transform.position = gameObject.transform.position;
                _obj.transform.parent = gameObject.transform.parent;
            }
            Destroy(gameObject);

        }
    }
}
