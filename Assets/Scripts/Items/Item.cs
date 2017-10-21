using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



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

public class Item : MonoBehaviour {
    [HideInInspector]public string m_ItemName = "Default Item Name";
    [HideInInspector]public ItemType m_Type;
    [HideInInspector]public float m_AttackSpeed;
    [HideInInspector]public float m_EnergyCost;
    [HideInInspector]public float m_Damage;
    [HideInInspector]public float m_Armor;
    [HideInInspector]public GameObject m_Projectile;
    public float m_Weight;
    public DAttributeBonus[] m_BonusList;
	// Use this for initialization
	void Start () {
        Debug.Assert(m_Projectile.GetComponent<Projectile>() != null, "m_Projectile is not a projectile.");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
