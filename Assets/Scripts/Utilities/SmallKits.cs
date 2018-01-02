using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SmallKits {

    public static void Shuffle<T>(ref T[] array)
    {
        for(int i=0; i<array.Length; i++) {
            T _t = array[i];
            int _pick = Random.Range(0, array.Length);
            array[i] = array[_pick];
            array[_pick] = _t;
        }
    }
    public static string FixName(System.UInt32 serialNum)
    {
        string _sName = serialNum.ToString();
        while (_sName.Length < 8)
            _sName = "0" + _sName;
        return _sName;
    }
}
