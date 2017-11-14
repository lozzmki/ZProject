using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour {

    public GameObject[] m_cSlots;//0-2 for primary; 3-5 for parts; 6 for armor;
    private int m_nPrimaryCursor = 0;
    private int m_nPartsCursor = 3;
    public GameObject m_ItemForPick;

	// Use this for initialization
	void Start () {
        m_cSlots = new GameObject[7];
        gameObject.GetComponent<Transceiver>().AddResolver("Pickup", PickUpItem);
        gameObject.GetComponent<Transceiver>().AddResolver("ChangePrimary", ChangePrimary);
        gameObject.GetComponent<Transceiver>().AddResolver("ChangeParts", ChangeParts);
        
    }
	
	// Update is called once per frame
	void Update () {
		//make the equipped one showup

        
	}

    private void EquipItem(GameObject itemObject)
    {
        Item item = itemObject.GetComponent<Item>();
        for (int _i = 0; _i < item.m_BonusList.Length; _i++)
            gameObject.GetComponent<Entity>().ApplyBonus(item.m_BonusList[_i]);
    }
    private void UnEquipItem(GameObject itemObject)
    {
        Item item = itemObject.GetComponent<Item>();
        for (int _i = 0; _i < item.m_BonusList.Length; _i++)
            gameObject.GetComponent<Entity>().RemoveBonus(item.m_BonusList[_i]);
    }
    private void MoveAway(GameObject itemObject)
    {
        //exile it...
        itemObject.transform.position += new Vector3(10000.0f, 0.0f, 0.0f);
        itemObject.GetComponent<Item>().m_bPicked = true;

    }
    private void MoveBack(GameObject itemObject, Vector3 postion)
    {
        //return it...
        itemObject.transform.position = postion;
        itemObject.GetComponent<Item>().m_bPicked = false;

    }

    public void PickUpItem(DSignal signal)
    {
        GameObject itemObject = m_ItemForPick;
        Item item = itemObject.GetComponent<Item>();
        bool _ifFull = true;
        if (item.m_bPicked) {
            //already picked
            m_ItemForPick = null;
            return;
        }
        if(item != null) {//check if the object is an Item.
            switch (item.m_Type) {
                case ItemType.ITEM_WEAPON:
                    for(int _i=0; _i<3; _i++) {
                        if(m_cSlots[_i] == null) {
                            m_cSlots[_i] = itemObject;
                            _ifFull = false;
                            break;
                        }
                    }
                    if (_ifFull) {
                        //full, swap it with the one in hand.
                        MoveBack(m_cSlots[m_nPrimaryCursor], itemObject.transform.position);
                        m_cSlots[m_nPrimaryCursor] = itemObject;
                        //remove bonuses
                        UnEquipItem(m_cSlots[m_nPrimaryCursor]);
                    }

                    break;
                case ItemType.ITEM_PARTS:
                    for (int _i = 3; _i < 6; _i++) {
                        if (m_cSlots[_i] == null) {
                            m_cSlots[_i] = itemObject;
                            _ifFull = false;
                            break;
                        }
                    }
                    if (_ifFull) {
                        //full, swap it with the one in hand.
                        MoveBack(m_cSlots[m_nPartsCursor], itemObject.transform.position);
                        m_cSlots[m_nPartsCursor] = itemObject;
                        //remove bonuses
                        UnEquipItem(m_cSlots[m_nPartsCursor]);
                    }

                    break;
                case ItemType.ITEM_ARMOR:
                    if (m_cSlots[6] == null) {
                        m_cSlots[6] = itemObject;
                    }
                    else {
                        MoveBack(m_cSlots[6], itemObject.transform.position);
                        m_cSlots[6] = itemObject;
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

    public Item GetWeapon()
    {
        GameObject _obj = m_cSlots[m_nPrimaryCursor];
        if (_obj == null)
            return null;
        else
            return _obj.GetComponent<Item>();
    }

    public Item GetParts()
    {
        GameObject _obj = m_cSlots[m_nPartsCursor];
        if (_obj == null)
            return null;
        else
            return _obj.GetComponent<Item>();
    }
}
