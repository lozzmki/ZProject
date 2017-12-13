using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff {
    //for script
    private LuaCache m_Cache;
    public BuffInterface m_Interface;

    //properties
    public Entity m_Owner;
    public string m_sName;
    public string m_sDesc;
    public int m_nMaxStackNum;
    public bool m_bStackable;
    public float m_fDuration;
    public DAttributeBonus[] m_Bonuses;

    //maintenance
    public int m_nStackNum;
    public float m_fTimeLeft;
    private bool m_bRemove = false;
    public bool ifRemove
    {
        get
        {
            return m_fTimeLeft < 0.0f || m_bRemove;
        }
    }

    [SLua.CustomLuaClass]
    public class BuffInterface
    {
        [SLua.DoNotToLua]
        public Buff m_Buff;

        public BuffInterface(Buff buff = null)
        {
            m_Buff = buff;
        }

        public LuaCache GetCache()
        {
            if (m_Buff != null)
                return m_Buff.m_Cache;
            return null;
        }

        public int GetStackNum()
        {
            if (m_Buff != null)
                return m_Buff.m_nStackNum;
            return 0;
        }

        public float GetTime()
        {
            return m_Buff.m_fTimeLeft;
        }

        private static BuffFunc _CastFunc(object func)
        {
            if (func != null)
                return ((SLua.LuaFunction)func).cast<BuffFunc>();
            return null;
        }

        //[SLua.MonoPInvokeCallbackAttribute(typeof(SLua.LuaCSFunction))]
        [SLua.StaticExport]
        public static BuffInterface CreateBuff(SLua.LuaTable table)
        {
            Buff _buff = new Buff
            {
                //properties
                m_sName = (string)table["Name"],
                m_sDesc = (string)table["Description"],
                m_nMaxStackNum = System.Convert.ToInt32(table["MaxStackNum"]),
                m_bStackable = System.Convert.ToBoolean(table["Stackable"]),
                m_fDuration = System.Convert.ToSingle(table["Duration"])
            };

            //bonuses
            SLua.LuaTable _bonus = (SLua.LuaTable)table["Bonus"];
            _buff.m_Bonuses = new DAttributeBonus[_bonus.length()];
            for (int i = 1; i <= _bonus.length(); i++) {
                SLua.LuaTable _oneBonus = (SLua.LuaTable)_bonus[i];
                _buff.m_Bonuses[i - 1] = new DAttributeBonus(
                        (AttrType)System.Convert.ToInt32(_oneBonus[1]),
                        System.Convert.ToSingle(_oneBonus[3]),
                        System.Convert.ToBoolean(_oneBonus[2])
                );
            }

            //functions
            _buff.OnAttach += _CastFunc(table["OnAttach"]);
            _buff.OnDetach += _CastFunc(table["OnDetach"]);
            _buff.OnUpdate += _CastFunc(table["OnUpdate"]);
            _buff.OnDamaged += _CastFunc(table["OnDamaged"]);
            _buff.OnTimeUp += _CastFunc(table["OnTimeUp"]);

            _buff.m_Interface = new BuffInterface(_buff);

            return _buff.m_Interface;
        }
    }

    [SLua.CustomLuaClass]
    public delegate void BuffFunc(BuffInterface buff, float arg);

    //When attached to an entity;
    private event BuffFunc OnAttach;
    //When Detached from an entity;
    private event BuffFunc OnDetach;
    //Called every frame
    private event BuffFunc OnUpdate;
    //Called when get Hit
    private event BuffFunc OnDamaged;
    //Called when time up
    private event BuffFunc OnTimeUp;

    public void Activate()
    {
        m_Cache = new LuaCache();
    }

    public void Update()
    {
        m_fTimeLeft -= Time.deltaTime;
        if (OnUpdate != null)
            OnUpdate(m_Interface, Time.deltaTime);
        if(m_fTimeLeft < 0.0f) {
            if (OnTimeUp != null)
                OnTimeUp(m_Interface, 0.0f);
        }

    }
    public void Attach(Entity ety)
    {
        m_Owner = ety;
        if (OnAttach != null)
            OnAttach(m_Interface, Time.deltaTime);
    }
    public void Detach()
    {
        if(OnDetach != null)
            OnDetach(m_Interface, Time.deltaTime);
    }
    public void Damaged(float damage)
    {
        if (OnDamaged != null)
            OnDamaged(m_Interface, Time.deltaTime);
    }




}
