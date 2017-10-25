using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour {

    public GameObject[] m_cSlots;//0-2 for primary; 3-5 for parts; 6 for armor; 7 for melee;
    private int m_nPrimaryCursor = 0;
    private int m_nPartsCursor = 3;
	// Use this for initialization
	void Start () {
        m_cSlots = new GameObject[8];
        gameObject.GetComponent<Transceiver>().AddResolver("Pickup", PickUpItem);
        gameObject.GetComponent<Transceiver>().AddResolver("ChangePrimary", ChangePrimary);
        gameObject.GetComponent<Transceiver>().AddResolver("ChangeParts", ChangeParts);
        gameObject.GetComponent<Transceiver>().AddResolver("RangeFire", RangeFire);
    }
	
	// Update is called once per frame
	void Update () {
		//make the equipped one showup

	}

    private void EquipItem(GameObject itemObject)
    {
        Item item = itemObject.GetComponent<Item>();
        for (int _i = 0; _i < item.m_BonusList.Length; _i++)
            gameObject.GetComponent<EntityAttribute>().ApplyBonus(item.m_BonusList[_i]);
    }
    private void UnEquipItem(GameObject itemObject)
    {
        Item item = itemObject.GetComponent<Item>();
        for (int _i = 0; _i < item.m_BonusList.Length; _i++)
            gameObject.GetComponent<EntityAttribute>().RemoveBonus(item.m_BonusList[_i]);
    }
    private void MoveAway(GameObject itemObject)
    {
        //exile it...
        itemObject.transform.position += new Vector3(10000.0f, 0.0f, 0.0f);
//         itemObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
//         itemObject.GetComponent<Rigidbody>().useGravity = false;
    }
    private void MoveBack(GameObject itemObject, Vector3 postion)
    {
        //return it...
        itemObject.transform.position = postion;
//         itemObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
//         itemObject.GetComponent<Rigidbody>().useGravity = true;
    }

    public void PickUpItem(DSignal signal)
    {
        GameObject itemObject = (GameObject)signal._arg1;
        Item item = itemObject.GetComponent<Item>();
        if(item != null) {//check if the object is an Item.
            switch (item.m_Type) {
                case ItemType.ITEM_PRIMARY:
                    for(int _i=0; _i<3; _i++) {
                        if(m_cSlots[_i] == null) {
                            m_cSlots[_i] = itemObject;
                            break;
                        }
                    }
                    //full, swap it with the one in hand.
                    MoveBack(m_cSlots[m_nPrimaryCursor], itemObject.transform.position);

                    //remove bonuses
                    UnEquipItem(m_cSlots[m_nPrimaryCursor]);

                    break;
                case ItemType.ITEM_MELEE:

                    if(m_cSlots[7] == null) {
                        m_cSlots[7] = itemObject;
                    }
                    else {
                        m_cSlots[7].transform.position = itemObject.transform.position;
                        MoveBack(m_cSlots[7], itemObject.transform.position);
                        UnEquipItem(m_cSlots[7]);
                    }
                        

                    break;
                case ItemType.ITEM_PARTS:
                    for (int _i = 3; _i < 6; _i++) {
                        if (m_cSlots[_i] == null) {
                            m_cSlots[_i] = itemObject;
                            break;
                        }
                    }
                    //full, swap it with the one in hand.
                    MoveBack(m_cSlots[m_nPartsCursor], itemObject.transform.position);
                    //remove bonuses
                    UnEquipItem(m_cSlots[m_nPartsCursor]);

                    break;
                case ItemType.ITEM_ARMOR:
                    if (m_cSlots[6] == null) {
                        m_cSlots[6] = itemObject;
                    }
                    else {
                        m_cSlots[6].transform.position = itemObject.transform.position;
                        MoveBack(m_cSlots[6], itemObject.transform.position);
                        UnEquipItem(m_cSlots[6]);
                    }
                    break;
                case ItemType.ITEM_SUPPLY:
                    break;
                default:
                    break;
            }

            //move the itemObject away,todo
            MoveAway(itemObject);
               
            //apply additional bonuses
            EquipItem(itemObject);
        }
    }

    public void ChangePrimary(DSignal signal)
    {
        bool _bNext = System.Convert.ToBoolean(signal._arg1);
        int _step;
        if (_bNext)
            _step = 1;
        else
            _step = -1;
        int _cursor = m_nPrimaryCursor + _step;
        while (_cursor != m_nPrimaryCursor) {
            
            if (m_cSlots[_cursor] != null) {
                m_nPrimaryCursor = _cursor;
                return;
            }
            _cursor += _step;
            if (_cursor > 2) _cursor = 0;
            if (_cursor < 0) _cursor = 2;
        }
    }

    public void ChangeParts(DSignal signal)
    {
        bool _bNext = System.Convert.ToBoolean(signal._arg1);
        int _step;
        if (_bNext)
            _step = 1;
        else
            _step = -1;
        int _cursor = m_nPartsCursor + _step;
        while (_cursor != m_nPartsCursor) {

            if (m_cSlots[_cursor] != null) {
                m_nPartsCursor = _cursor;
                return;
            }
            _cursor += _step;
            if (_cursor > 5) _cursor = 3;
            if (_cursor < 3) _cursor = 5;
        }
    }

    //todo
    public void RangeFire(DSignal signal)
    {
        //merge primary with parts
        GameObject _ammo;
        if(m_cSlots[m_nPrimaryCursor] != null) {
            //check cooldown
            if (!m_cSlots[m_nPrimaryCursor].GetComponent<Item>().IfCooled)
                return;
            else
                m_cSlots[m_nPrimaryCursor].GetComponent<Item>().Attack();

            if (m_cSlots[m_nPartsCursor]) {
                string _ammoName = m_cSlots[m_nPartsCursor].GetComponent<Item>().m_Projectile;
                _ammo = Resources.Load<GameObject>("Prefabs/Projectiles/" + _ammoName);
                if(_ammo == null) {
                    Debug.Log("Ammo:Model is null");
                    _ammo = Instantiate(Resources.Load<GameObject>("Prefabs/Projectiles/Default"),/*todo*/gameObject.transform.position, Random.rotation);
                }
                else {
                    _ammo = Instantiate(_ammo,/*todo*/gameObject.transform.position, Random.rotation);
                }

                _ammo.transform.forward = gameObject.transform.forward;
                _ammo.GetComponent<Projectile>().m_Damage = m_cSlots[m_nPartsCursor].GetComponent<Item>().m_Damage;
                
            }
            else {
                //no ammo, use default
                _ammo = Instantiate(Resources.Load<GameObject>("Prefabs/Projectiles/Default"),/*todo*/gameObject.transform.position, Random.rotation);
            }

            _ammo.GetComponent<Projectile>().m_Master = gameObject;
            _ammo.GetComponent<Projectile>().m_Damage += m_cSlots[m_nPrimaryCursor].GetComponent<Item>().m_Damage;
            _ammo.GetComponent<Projectile>().m_Speed = 30.0f;
        }
    }
}
