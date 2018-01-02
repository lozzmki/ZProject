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

public class EntityHUD
{
    GameObject m_Parent;

    private Timer m_HideTimer;

    private float m_fCurrentValue;
    private float m_fSubValue;

    private const float c_fSubSpeed = 0.15f;
    private const float c_fSizeUnit = 0.4f;
    private const float c_fAspectRatio = 5.0f;
    static Material _s_barMat;
    static Material BarMat
    {
        get
        {
            if (_s_barMat == null)
                _s_barMat = Resources.Load<Material>("UI/verticalBars");
            return _s_barMat;
        }
    }
    

    public GameObject gameObject
    {
        get
        {
            return m_Parent;
        }
    }
    public EntityHUD()
    {
        //float _fFrameZ = 0.05f;
        //float _fSubBarZ = 0.0f;
        float _fBarZ = -0.05f;

        m_Parent = new GameObject("HUD");

        Mesh _mesh;
        Vector3[] _v = new Vector3[4]
        {
            new Vector3(0.0f, 0.0f)*c_fSizeUnit,
            new Vector3(0.0f, c_fAspectRatio)*c_fSizeUnit,
            new Vector3(1.0f, c_fAspectRatio)*c_fSizeUnit,
            new Vector3(1.0f, 0.0f)*c_fSizeUnit
        };
        Vector2[] _uv = new Vector2[4]
        {
            new Vector2(0.0f,0.0f),
            new Vector2(0.0f,1.0f),
            new Vector2(1.0f,1.0f),
            new Vector2(1.0f,0.0f),
        };
        int[] _tr = new int[6]
        {
            0,1,2,0,2,3
        };
        //bar

        m_Parent.transform.position = Vector3.zero;
        _mesh = m_Parent.AddComponent<MeshFilter>().mesh;
        m_Parent.AddComponent<MeshRenderer>().material = BarMat;
        m_Parent.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.blue);
        m_Parent.GetComponent<MeshRenderer>().material.SetColor("_SubColor", Color.cyan);
        for (int i = 0; i < 4; i++)
            _v[i].z = _fBarZ;
        _mesh.vertices = _v;
        _mesh.uv = _uv;
        _mesh.triangles = _tr;

        m_fCurrentValue = 1.0f;
        m_HideTimer = new Timer(3.0f);
        m_bVisible = true;
        Visible = false;
    }
    bool m_bVisible;
    public bool Visible
    {
        set
        {
            if(m_bVisible != value) {
                m_bVisible = value;
                m_Parent.SetActive(value);
            }
        }
        get
        {
            return m_bVisible;
        }
    }

    public void SetValue(float val)
    {
        if (val > 1.0f)
            val = 1.0f;
        if (val < 0.0f)
            val = 0.0f;
        if (m_fCurrentValue == val)
            return;

        m_fCurrentValue = val;
        //m_Bar.transform.localScale = new Vector3(1.0f, m_fCurrentValue, 1.0f);

        m_Parent.GetComponent<MeshRenderer>().material.SetFloat("_Percent", m_fCurrentValue);

        if (m_fSubValue < val) {
            m_fSubValue = val;
            m_Parent.GetComponent<MeshRenderer>().material.SetFloat("_SubPercent", m_fSubValue);
        }

        m_HideTimer.Reset();
        Visible = true;
        m_Parent.GetComponent<MeshRenderer>().material.SetFloat("_Alpha", 1.0f);
    }

    public void Update(Transform trans)
    {
        if (!Visible)
            return;

        m_Parent.transform.rotation = Camera.main.transform.rotation;
        m_Parent.transform.position = trans.position + Camera.main.transform.right * 1.0f + Camera.main.transform.up * -1.2f * c_fSizeUnit;
        //Vector3 _pos = m_Parent.transform.parent.position + Camera.main.transform.right * 1.0f + Camera.main.transform.up * -1.2f * c_fSizeUnit;
        //m_Parent.transform.localPosition = m_Parent.transform.parent.InverseTransformPoint(_pos);

        if (m_fSubValue > m_fCurrentValue) {
            m_fSubValue -= c_fSubSpeed * Time.deltaTime;
            m_Parent.GetComponent<MeshRenderer>().material.SetFloat("_SubPercent", m_fSubValue);
        }
        else {
            m_HideTimer.Update();
            if (m_HideTimer.IfExpired)
                Visible = false;
            else {
                float _a = m_HideTimer.LeftPercent / 0.2f;
                if (_a > 1.0f)
                    _a = 1.0f;
                m_Parent.GetComponent<MeshRenderer>().material.SetFloat("_Alpha", _a);
            }
        }


    }
}


/// <summary>
/// 实体类。
/// 包含各种实体通用的属性和方法
/// </summary>
[RequireComponent(typeof(Transceiver))]
public class Entity : MonoBehaviour {

    #region Properties
    public string m_EntityName = "Default Entity Name";
    public float m_fHp;
    public float m_fEnergy;
    public float m_fCostReduce = 0.0f;
    public DProperty[] m_Properties;
    public Vector3 m_MovingDirection;
    public GameObject m_Target;
    #endregion

    #region ForInspector
    [HideInInspector] public float MaxHp = 200.0f;
    [HideInInspector] public float MaxEn = 200.0f;
    [HideInInspector] public float Melee = 0.0f;
    [HideInInspector] public float Range = 0.0f;
    [HideInInspector] public float Armor = 0.0f;
    [HideInInspector] public float Speed = 5.0f;
    #endregion

    #region Macros
    //tags, for coding..
    public const int MAX_HP = 0;
    public const int MAX_EN = 1;
    public const int MELEE_POWER = 2;
    public const int RANGE_POWER = 3;
    public const int ARMOR = 4;
    public const int SPEED = 5;

    #endregion

    private EntityHUD m_HUD;

    public BuffManager m_Buffs;
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

    public bool IsDead
    {
        get
        {
            return m_fHp <= 0.0f;
        }
    }

    // Use this for initialization
    void Start() {
        m_Properties = new DProperty[6];
        m_Buffs = new BuffManager(this);

        m_Properties[MAX_HP].d_Value = m_fHp = MaxHp;
        m_Properties[MAX_EN].d_Value = m_fEnergy = MaxEn;
        m_Properties[MELEE_POWER].d_Value = Melee;
        m_Properties[RANGE_POWER].d_Value = Range;
        m_Properties[ARMOR].d_Value = Armor;
        m_Properties[SPEED].d_Value = Speed;

        ////for test
        //m_Properties[SPEED].d_Value = 3;
        //m_Properties[ARMOR].d_Value = 10;
        //m_Properties[MAX_HP].d_Value = 200;
        //m_fHp = 200;
        //m_fEnergy = 9999;
        ////for test end

        m_HUD = new EntityHUD();
        m_HUD.gameObject.transform.parent = transform;

        GetComponent<Transceiver>().AddResolver("Damage", Damage);
        GetComponent<Transceiver>().AddResolver("CostEnergy", CostEnergy);
        GetComponent<Transceiver>().AddResolver("Heal", Heal);
        GetComponent<Transceiver>().AddResolver("Charge", Charge);
        GetComponent<Transceiver>().AddResolver("AddMove", Move);
        //init from lua script

    }
    Vector3 m_vMovement = new Vector3(0, 0, 0);
    private void FixedUpdate()
    {
        GetComponent<Rigidbody>().velocity = m_vMovement;
        m_vMovement = Vector3.zero;
    }

    private void Update()
    {
        m_Buffs.Update();

        if (m_fHp > m_Properties[MAX_HP].d_Value)
            m_fHp = m_Properties[MAX_HP].d_Value;
        if (m_fEnergy > m_Properties[MAX_EN].d_Value)
            m_fEnergy = m_Properties[MAX_EN].d_Value;
    }

    private void LateUpdate()
    {
        if (m_Properties[MAX_HP].d_Value != 0.0f)
            m_HUD.SetValue(m_fHp / m_Properties[MAX_HP].d_Value);
        else
            m_HUD.SetValue(1.0f);
        m_HUD.Update(transform);
    }

    private void OnEnable()
    {
        if(m_HUD != null)
            m_HUD.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        m_HUD.gameObject.SetActive(false);
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

    public bool CanAfford(float cost)
    {
        float _ratio = 1.0f - m_fCostReduce;
        if (_ratio < 0.1f)
            _ratio = 0.1f;
        return cost * _ratio <= m_fEnergy;
    }
    public void CostEnergy(DSignal signal)
    {
        float _fRatio = 1.0f - m_fCostReduce;
        if (_fRatio < 0.1f)
            _fRatio = 0.1f;

        float _fCost = System.Convert.ToSingle(signal._arg1) * _fRatio;
        m_fEnergy -= _fCost;
    }

    public void Kill()
    {
        
        Destroy(m_HUD.gameObject);
        ModelEffect.CreateEffect(Resources.Load<GameObject>("Prefabs/Effects/Blast"), transform.position);
        Transceiver.SendSignal(new DSignal(null, gameObject, "Dead"));
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }

    public void Damage(DSignal signal)
    {
        
        float _fRatio = 100.0f / (100.0f + m_Properties[ARMOR].d_Value * 3.0f);
        if (_fRatio > 10.0f || _fRatio < 0.0f)
            _fRatio = 10.0f;

        float _fDamage = System.Convert.ToSingle(signal._arg1) * _fRatio;
        DamageIndicator.CreateFloatingText(gameObject.transform.position, (int)_fDamage);

        if (IsDead)
            return;

        m_fHp -= _fDamage;

        if (IsDead) {
            //Debug.Log(gameObject + " was killed by " + signal._sender);
            Kill();
            //if (gameObject.tag == "Player")//TODO:shouldn't be like this!!!!!!
            //    Camera.main.GetComponent<GameInput>().m_Player = null;
        }
    }

    public void Heal(DSignal signal)
    {
        float _fHeal = System.Convert.ToSingle(signal._arg1);
        m_fHp += _fHeal;
        DamageIndicator.CreateFloatingText(gameObject.transform.position, (int)_fHeal, false);
        if (m_fHp > m_Properties[MAX_HP].d_Value) {
            m_fHp = m_Properties[MAX_HP].d_Value;
        }
    }

    public void Charge(DSignal signal)
    {
        float _fCharge = System.Convert.ToSingle(signal._arg1);
        m_fEnergy += _fCharge;
        if (m_fEnergy > m_Properties[MAX_EN].d_Value) {
            m_fEnergy = m_Properties[MAX_EN].d_Value;
        }
    }

    public void Move(DSignal signal)
    {
        m_vMovement = (Vector3)signal._arg1;
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
