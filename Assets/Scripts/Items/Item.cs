using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum ItemType
{
    ITEM_PRIMARY,
    ITEM_MELEE,
    ITEM_PARTS,
    ITEM_ARMOR,
}

public enum ShotType
{
    SHOT_SINGLE,
    SHOT_AUTO,
    SHOT_SPREAD,
}


/// <summary>
/// 物品的模板类
/// </summary>
public class Item : MonoBehaviour {
    public string m_ItemName = "Default Item Name";
    public ItemType m_Type;
    public float m_AttackSpeed;
    public float m_EnergyCost;
    public float m_Damage;
    public float m_Armor;
    public GameObject m_Projectile;
    public float m_Weight;
    public string m_LuaScript="";
    public DAttributeBonus[] m_BonusList;

    private static int _p = -1;
	// Use this for initialization
	void Start () {
        _p++;
        //loading script
        if (m_LuaScript == "")
            return;
        //init from lua script,the content path shall be modified,todo.
        SLua.LuaSvr.getInstance().callFunction("Data/Items/"+m_LuaScript, "LoadProperty");
        Debug.Log(_p);
    }
	
	// Update is called once per frame
	void Update () {
        

    }

    public void Use()
    {
        //run the script
    }
}
