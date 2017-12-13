using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SLua.CustomLuaClass]
public class LuaCache {

    //[SLua.DoNotToLua]
    static LuaCache _s_Cache;

    //[SLua.DoNotToLua]
    static int _n_nextInt = 0;
    //[SLua.DoNotToLua]
    public static int GlobalTicket
    {
        get
        {
            return _n_nextInt++;
        }
    }

    //[SLua.DoNotToLua]
    public static LuaCache GlobalCache
    {
        get
        {
            if (_s_Cache == null)
                _s_Cache = new LuaCache();
            return _s_Cache;
        }
    }

    private Dictionary<int, object> m_Cache;

    public LuaCache()
    {
        m_Cache = new Dictionary<int, object>();
    }

    public void StoreValue(int key, object val)
    {
        if (!m_Cache.ContainsKey(key)) {
            m_Cache.Add(key, val);
        }
        else {
            m_Cache[key] = val;
        }
    }

    public object LoadValue(int key)
    {
        if (!m_Cache.ContainsKey(key))
            return null;
        else
            return m_Cache[key];
    }
}
