using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ItemPool.CreateRandomItem(ItemPool.PoolType.POOL_ALL, transform.position + new Vector3(DCell.CELL_BORDER_LENGTH, 0, 0)).GetComponent<Item>().m_bForSale = true;
        ItemPool.CreateRandomItem(ItemPool.PoolType.POOL_ALL, transform.position + new Vector3(0, 0, DCell.CELL_BORDER_LENGTH)).GetComponent<Item>().m_bForSale = true;
        ItemPool.CreateRandomItem(ItemPool.PoolType.POOL_ALL, transform.position + new Vector3(-DCell.CELL_BORDER_LENGTH, 0, 0)).GetComponent<Item>().m_bForSale = true;
        ItemPool.CreateRandomItem(ItemPool.PoolType.POOL_ALL, transform.position + new Vector3(0, 0, -DCell.CELL_BORDER_LENGTH)).GetComponent<Item>().m_bForSale = true;
    }
	
	// Update is called once per frame
	//void Update () {
	//	
	//}
}
