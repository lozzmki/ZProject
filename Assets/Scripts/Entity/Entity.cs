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

    public BuffManager m_Buffs;

    //tags, for coding..
    public const int MAX_HP = 0;
    public const int MAX_EN = 1;
    public const int MELEE_POWER = 2;
    public const int RANGE_POWER = 3;
    public const int ARMOR = 4;
    public const int SPEED = 5;

    public DProperty[] m_Properties;
    private float m_fBurden;

    private EntityInterface _m_interface;
    public EntityInterface m_Interface
    {
        get
        {
            if (_m_interface == null)
                _m_interface = new EntityInterface(this);
            return _m_interface;
        }
    }

    // Use this for initialization
    void Start() {
        m_Properties = new DProperty[6];
        m_Buffs = new BuffManager(this);

        //for test
        m_Properties[SPEED].d_Value = 1;
        m_Properties[ARMOR].d_Value = 10;
        m_Properties[MAX_HP].d_Value = 200;
        m_fHp = 200;
        //for test end

        gameObject.GetComponent<Transceiver>().AddResolver("Damage", Damage);

        //init from lua script

    }


    private void Update()
    {
        m_Buffs.Update();
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
        float _fRatio = 100.0f / (100.0f + m_Properties[ARMOR].d_Value);
        if (_fRatio > 10.0f || _fRatio < 0.0f)
            _fRatio = 10.0f;

        float _fDamage = System.Convert.ToSingle(signal._arg1) * _fRatio;
        m_fHp -= _fDamage;
        if (m_fHp <= 0.0f) {
            Debug.Log(gameObject + " was killed by " + signal._sender);
            Destroy(gameObject);
            if (gameObject.tag == "Player")//TODO:shouldn't be like this!!!!!!
                Camera.main.GetComponent<GameInput>().m_Player = null;
        }
    }


    [SLua.CustomLuaClass]
    public class EntityInterface
    {
        [SLua.DoNotToLua]
        public Entity m_Entity;

        public EntityInterface(Entity ety = null)
        {
            m_Entity = ety;
        }

        public void AddBuff(Buff.BuffInterface buff)
        {
            if (m_Entity != null)
                m_Entity.m_Buffs.AddBuff(buff.m_Buff);
        }
    }
}
