using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureSpawner : MonoBehaviour {

    private int m_nRarity;

	// Use this for initialization
	void Start () {
        //FIN:set rarity　or sth here
        m_nRarity = GetComponent<Transceiver>()._dataCache32;
    }

    private void OnTriggerEnter(Collider other)
    {
        //FIN:temporary hard coded
        if(other.tag == "Player") {
            //ObjectDictionary.GetItemDic().CreateObject("Cracker",
            //     gameObject.transform.position,
            //     Quaternion.identity);
            //test
            //FIN:load rarity in property
            ItemPool.CreateRandomItem((ItemPool.PoolType)m_nRarity, transform.position).transform.parent = transform.parent;
            Destroy(gameObject);
        }
    }
}
