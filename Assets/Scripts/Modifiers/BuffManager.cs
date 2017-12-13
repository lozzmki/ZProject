using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager{
    Dictionary<string, Buff> m_Buffs;
    Entity m_Owner;

    public BuffManager(Entity owner = null)
    {
        m_Buffs = new Dictionary<string, Buff>();
        m_Owner = owner;
    }

    public void Update()
    {
        List<string> _keys = new List<string>();
        foreach(var _pair in m_Buffs) {
            _pair.Value.Update();
            if (_pair.Value.ifRemove) {
                _keys.Add(_pair.Key);
                foreach (var attr in _pair.Value.m_Bonuses) {
                    for (int i = 0; i < _pair.Value.m_nStackNum; i++)
                        m_Owner.RemoveBonus(attr);
                }
            }
        }
        foreach(var _k in _keys) {
            m_Buffs.Remove(_k);
        }
    }

    public void AddBuff(Buff buff)
    {
        string _key = buff.m_sName;
        if (m_Buffs.ContainsKey(_key)) {
            Buff _buff = m_Buffs[_key];
            if (_buff.m_nStackNum < _buff.m_nMaxStackNum) {
                _buff.m_nStackNum++;
                foreach (var attr in buff.m_Bonuses) {
                    m_Owner.ApplyBonus(attr);
                }
            }
            _buff.m_fTimeLeft = _buff.m_fDuration;
        }
        else {
            buff.Activate();
            buff.m_fTimeLeft = buff.m_fDuration;
            buff.m_nStackNum = 1;
            foreach(var attr in buff.m_Bonuses) {
                m_Owner.ApplyBonus(attr);
            }
            m_Buffs.Add(buff.m_sName, buff);
            buff.Attach(m_Owner);
        }
    }

    public void RemoveBuff(Buff buff)
    {
        string _key = buff.m_sName;
        if (m_Buffs.ContainsKey(_key)) {
            foreach(var attr in buff.m_Bonuses) {
                for(int i=0; i<m_Buffs[_key].m_nStackNum; i++)
                    m_Owner.RemoveBonus(attr);
            }
            m_Buffs[_key].Detach();
            m_Buffs.Remove(_key);
        }
    }

    public void OnDamaged(float damage)
    {
        foreach(var _buff in m_Buffs) {
            _buff.Value.Damaged(damage);
        }
    }
}
