using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour {

    public GameObject[] m_cSlots;//0-2 for primary; 3-5 for parts; 6 for armor;
    public int m_Coins;
    private int m_nPrimaryCursor = 0;
    private int m_nPartsCursor = 3;
    public GameObject m_ItemForPick;
    public ChipSet m_Chipset;
    private List<GameObject> m_ChipPocket;
    private const int c_nPocketSize = 25;

	// Use this for initialization
	void Start () {
        m_cSlots = new GameObject[7];
        m_Chipset = new ChipSet();
        m_ChipPocket = new List<GameObject>(c_nPocketSize);
        gameObject.GetComponent<Transceiver>().AddResolver("Pickup", PickUpItem);
        gameObject.GetComponent<Transceiver>().AddResolver("ChangePrimary", ChangePrimary);
        gameObject.GetComponent<Transceiver>().AddResolver("ChangeParts", ChangeParts);
        
    }
	
	//// Update is called once per frame
	void Update () {
        //make the equipped ones update
        UpdateItem(m_cSlots[m_nPrimaryCursor]);//current weapon
        UpdateItem(m_cSlots[m_nPartsCursor]);//current parts
        UpdateItem(m_cSlots[6]);//current armor
    }

    private void UpdateItem(GameObject itemObject)
    {
        if(null != itemObject) {
            itemObject.GetComponent<Item>().ActiveUpdate();
        }
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
        itemObject.transform.parent = null;
        itemObject.GetComponent<Item>().m_bPicked = true;

    }
    private void MoveBack(GameObject itemObject, Vector3 postion)
    {
        //return it...
        itemObject.transform.position = postion;
        itemObject.transform.parent = GameManager.CurrentStage._stage.transform;
        itemObject.GetComponent<Item>().m_bPicked = false;

    }

    public bool PickUp(GameObject itemObject)
    {
        Item item = itemObject.GetComponent<Item>();
        bool _ifFull = true;
        if (item.m_bPicked) {
            //already picked
            //m_ItemForPick = null;
            return false;
        }
        if (item != null) {//check if the object is an Item.
            switch (item.m_Type) {
                case ItemType.ITEM_WEAPON:
                    for (int _i = 0; _i < 3; _i++) {
                        if (m_cSlots[_i] == null) {
                            m_cSlots[_i] = itemObject;
                            _ifFull = false;
                            break;
                        }
                    }
                    if (_ifFull) {
                        //full, swap it with the one in hand.
                        MoveBack(m_cSlots[m_nPrimaryCursor], itemObject.transform.position);
                        m_cSlots[m_nPrimaryCursor] = itemObject;
                        //apply additional bonuses
                        EquipItem(itemObject);
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
                        //apply additional bonuses
                        EquipItem(itemObject);
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
                        EquipItem(itemObject);
                        UnEquipItem(m_cSlots[6]);
                    }
                    break;
                case ItemType.ITEM_CHIP:
                    if(m_ChipPocket.Count < c_nPocketSize) {
                        m_ChipPocket.Add(itemObject);
                        //MoveAway(itemObject);
                    }
                    break;
                default:
                    break;
            }

            //move the itemObject away,fin
            MoveAway(itemObject);
            //item.MarkItem(false);
            return true;
        }
        return false;
    }

    public void PickUpItem(DSignal signal)
    {
        Item _item = m_ItemForPick.GetComponent<Item>();
        if (_item.m_bForSale) {
            if (m_Coins >= _item.m_Price) {
                m_Coins -= _item.m_Price;
                _item.m_bForSale = false;
            }
            else
                return;
        }

        PickUp(m_ItemForPick);
        m_ItemForPick = null;
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
                UnEquipItem(m_cSlots[m_nPrimaryCursor]);
                EquipItem(m_cSlots[_cursor]);
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
                UnEquipItem(m_cSlots[m_nPartsCursor]);
                EquipItem(m_cSlots[_cursor]);
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
