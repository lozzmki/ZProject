using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct DProperty
{
    public float d_fOrigin;
    public float d_fFixed { get; set; }
    public float d_fRatio { get; set; }
    public float _fValue {
        get {
            return d_fOrigin * (1.0f + d_fRatio) + d_fFixed;
        }
        set {
            d_fOrigin = value;
        }
    }
    
}

public class Attribute : MonoBehaviour {
    public DProperty m_MaxHP;
    public DProperty m_MaxEnergy;
    public DProperty m_MeleePower;
    public DProperty m_Armor;
    public DProperty m_Speed;

    public float m_fHp;
    public float m_fEnergy;
    private void Start()
    {

    }
    private void Update()
    {
        
    }
}
