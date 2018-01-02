using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SLua.CustomLuaClass]
public static class ModelEffect {

    [SLua.DoNotToLua]
    public static GameObject CreateEffect(GameObject prefab, Vector3 position, float life = 1.0f)
    {
        GameObject _eff = Object.Instantiate(prefab);
        _eff.transform.position = position;
        if(life>0.0f)
            _eff.AddComponent<FiniteLife>().LifeTime = life;
        return _eff;

    }

    public static GameObject CreateEffect(string name, Vector3 position, float life = 1.0f)
    {
        GameObject _eff = Object.Instantiate(ObjectManager.Get(name.GetHashCode()));
        _eff.transform.position = position;
        if (life > 0.0f)
            _eff.AddComponent<FiniteLife>().LifeTime = life;
        return _eff;
    
    }
}
