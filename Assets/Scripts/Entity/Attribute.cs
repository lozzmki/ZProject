using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//for inspector
public enum AttrType
{
    MAX_HP,
    MAX_EN,
    MELEE_POWER,
    RANGE_POWER,
    ARMOR,
    SPEED
}

[System.Serializable]
public struct DAttributeBonus
{
    public AttrType d_nType;
    public float d_fValue;
    public bool d_bIsRatio;
    public DAttributeBonus(AttrType nType, float fValue, bool bIsRatio = false)
    {
        d_nType = nType;
        d_fValue = fValue;
        d_bIsRatio = bIsRatio;
    }
}

[System.Serializable]
public struct DProperty
{
    public float d_fOrigin;
    public float d_fFixed;
    public float d_fRatio;
    public float d_Value {
        get {
            return d_fOrigin * (1.0f + d_fRatio) + d_fFixed;
        }
        set {
            d_fOrigin = value;
        }
    }
    public DProperty(float fVal = 0.0f)
    {
        d_fOrigin = fVal;
        d_fFixed = d_fRatio = 0.0f;
    }
}

public class Attribute : MonoBehaviour {
    public string m_EntityName = "Default Entity Name";
    //current value
    public float m_fHp;
    public float m_fEnergy;

    public int m_Coins;
    public float m_MaxCarrying;

    //tags, for coding..
    public const int MAX_HP = 0;
    public const int MAX_EN = 1;
    public const int MELEE_POWER = 2;
    public const int RANGE_POWER = 3;
    public const int ARMOR = 4;
    public const int SPEED = 5;

    [HideInInspector] public DProperty[] m_Properties;
    private float m_fBurden;
    private void Start()
    {
        m_Properties = new DProperty[6];
        //init from lua script
        
    }
    private void Update()
    {
       

    }


    public void ApplyBonus(DAttributeBonus dBonus)
    {
        if (dBonus.d_bIsRatio) {
            m_Properties[(int)dBonus.d_nType].d_fRatio += dBonus.d_fValue;
        }
        else {
            m_Properties[(int)dBonus.d_nType].d_fFixed += dBonus.d_fValue;
        }
    }
    public void RemoveBonus(DAttributeBonus dBonus)
    {
        if (dBonus.d_bIsRatio) {
            m_Properties[(int)dBonus.d_nType].d_fRatio -= dBonus.d_fValue;
        }
        else {
            m_Properties[(int)dBonus.d_nType].d_fFixed -= dBonus.d_fValue;
        }
    }
}
