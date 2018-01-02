using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool {

    public static Dictionary<string, GameObject> _s_Base;
    public static List<string> _s_Unique;
    public static List<string> _s_Basic;
    public static List<string> _s_Advanced;
    public static List<string> _s_Rare;
    public static List<string> _s_All;

    public static void InitPool()
    {
        _s_Base = new Dictionary<string, GameObject>();
        _s_Unique = new List<string>();
        _s_Basic = new List<string>();
        _s_Advanced = new List<string>();
        _s_Rare = new List<string>();
        _s_All = new List<string>();
    }


    public static void AddItem(GameObject item)
    {
        Item _it = item.GetComponent<Item>();
        if (_it == null)
            return;

        List<string> _dic;
        switch (_it.m_Rarity) {
            case ItemRarity.RARITY_UNIQUE:
                _dic = _s_Unique;
                break;
            case ItemRarity.RARITY_BASIC:
                _dic = _s_Basic;
                break;
            case ItemRarity.RARITY_ADVANCED:
                _dic = _s_Advanced;
                break;
            case ItemRarity.RARITY_RARE:
                _dic = _s_Rare;
                break;
            default:
                _dic = null;
                break;
        }
        if(_dic != null)
            if(!_dic.Contains(_it.m_ItemName))
                _dic.Add(_it.m_ItemName);
        if(_it.m_Rarity != ItemRarity.RARITY_UNIQUE)
            if (!_s_All.Contains(_it.m_ItemName))
                _s_All.Add(_it.m_ItemName);
        if (_s_Base.ContainsKey(_it.m_ItemName))
            _s_Base[_it.m_ItemName] = item;
        else
            _s_Base.Add(_it.m_ItemName, item);
    }

    public static GameObject CreateItem(string sName, Vector3 pos)
    {
        GameObject _item = null;
        if (_s_Base.ContainsKey(sName)) {
            _item = _s_Base[sName];
            _item = Object.Instantiate(_item);
            _item.transform.position = pos;
            Transceiver.SendSignal(new DSignal(null, _s_Base[sName], "Duplicate", _item));
        }
        return _item;
    }

    public enum PoolType
    {
        POOL_ALL,
        POOL_BASIC,
        POOL_ADVANCED,
        POOL_RARE,
        POOL_NON_RARE,
        POOL_NON_BASIC,
    }

    public static GameObject CreateRandomItem(PoolType pool, Vector3 pos)
    {
        GameObject _item = null;
        int _len = _s_All.Count;
        int _len_b = _s_Basic.Count;
        int _len_a = _s_Advanced.Count;
        int _len_r = _s_Rare.Count;
        int _len_nr = _len_b + _len_a;
        int _len_nb = _len_a + _len_r;

        int _pick = 0;
        switch (pool) {
            case PoolType.POOL_ALL:
                _pick = Random.Range(0, _len);
                _item = _s_Base[_s_All[_pick]];
                break;
            case PoolType.POOL_BASIC:
                _pick = Random.Range(0, _len_b);
                _item = _s_Base[_s_Basic[_pick]];
                break;
            case PoolType.POOL_ADVANCED:
                _pick = Random.Range(0, _len_a);
                _item = _s_Base[_s_Advanced[_pick]];
                break;
            case PoolType.POOL_RARE:
                _pick = Random.Range(0, _len_r);
                _item = _s_Base[_s_Rare[_pick]];
                break;
            case PoolType.POOL_NON_RARE:
                _pick = Random.Range(0, _len_nr);
                if (_pick >= _len_b)
                    _item = _s_Base[_s_Advanced[_pick - _len_b]];
                else
                    _item = _s_Base[_s_Basic[_pick]];
                break;
            case PoolType.POOL_NON_BASIC:
                _pick = Random.Range(0, _len_nb);
                if (_pick >= _len_a)
                    _item = _s_Base[_s_Rare[_pick - _len_a]];
                else
                    _item = _s_Base[_s_Advanced[_pick]];
                break;
            default:
                break;
        }

        if (_item != null) {
            GameObject _tmp = Object.Instantiate(_item);
            _tmp.transform.position = pos;
            Transceiver.SendSignal(new DSignal(null, _item, "Duplicate", _tmp));
            return _tmp;
        }
        else
            return null;
    }
}
