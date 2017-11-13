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
/// <summary>
/// 一项属性
/// Origin为初始值，Fixed为固定修正，Ratio为比例修正，Value为数值存取接口
/// </summary>
[System.Serializable]
public struct DProperty
{
    public float d_fOrigin;
    public float d_fFixed;
    public float d_fRatio;
    public float d_Value
    {
        get
        {
            return d_fOrigin * (1.0f + d_fRatio / 100.0f) + d_fFixed;
        }
        set
        {
            d_fOrigin = value;
        }
    }
    public DProperty(float fVal = 0.0f)
    {
        d_fOrigin = fVal;
        d_fFixed = d_fRatio = 0.0f;
    }
}
/// <summary>
/// 实体类。
/// 包含各种实体通用的属性和方法
/// </summary>
public class Entity : MonoBehaviour {
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

    public DProperty[] m_Properties;
    private float m_fBurden;
    // Use this for initialization
    void Start () {
        m_Properties = new DProperty[6];
        m_Properties[SPEED].d_Value = 8;//for test

        gameObject.GetComponent<Transceiver>().AddResolver("Move", MoveTowards);
        gameObject.GetComponent<Transceiver>().AddResolver("Damage", Damage);
        //init from lua script
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MoveTowards(DSignal signal){
        Vector3 vDirection = (Vector3)signal._arg1;
        //Position
        gameObject.transform.position += vDirection.normalized * m_Properties[SPEED].d_Value * Time.deltaTime;
        //Rotation
        //         Vector3 _vTurn = gameObject.transform.InverseTransformDirection(vDirection);
        //         float _fAngle = Mathf.Atan2(_vTurn.x, _vTurn.z) * Mathf.Rad2Deg;
        //         float _fRotation = 3500.0f * Time.deltaTime;
        // 
        //         if (Mathf.Abs(_fAngle) < _fRotation)
        //             _fRotation = _fAngle;
        //         else {
        //             if (_fAngle < 0.0f)
        //                 _fRotation = -_fRotation;
        //         }
        //         gameObject.transform.Rotate(0.0f, _fRotation, 0.0f);
        gameObject.transform.forward = vDirection;
        
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

    public void Damage(DSignal signal)
    {
        m_fHp -= System.Convert.ToSingle(signal._arg1);
        if (m_fHp <= 0.0f) {
            Debug.Log(gameObject + " was killed by " + signal._sender);
            Destroy(gameObject);
        }
    }

}
