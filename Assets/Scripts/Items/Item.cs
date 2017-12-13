﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    ITEM_WEAPON,
    ITEM_PARTS,
    ITEM_ARMOR,
    ITEM_SUPPLY,
}

public enum WeaponType
{
    WEAPON_ARC,
    WEAPON_AUTO,
    WEAPON_SPREAD,
    WEAPON_LASER,
    WEAPON_MELEE,
}


/// <summary>
/// 物品的模板类
/// 使用Lua文件装载数据
/// </summary>
public class Item : MonoBehaviour {
    public string m_ItemName = "Default Item Name";
    public string m_Description = "No Description";
    public ItemType m_Type;
    public WeaponType m_WeaponType;
    public float m_AttackSpeed;
    public float m_EnergyCost;
    public float m_Damage;
    public float m_Armor;
    public float m_Weight;
    public string m_LuaScript="";
    public string m_Prefab = "";
    public string m_Projectile = "";
    public DAttributeBonus[] m_BonusList;
    public Texture2D m_Icon;
    public bool m_bPicked = false;

    public LuaCache m_Cache;
    private ItemInterface _m_interface;
    public ItemInterface m_Interface
    {
        get
        {
            if (_m_interface == null)
                _m_interface = new ItemInterface(this);
            return _m_interface;
        }
    }

    private float m_fCooldown;
    private float m_fFloat = 0.0f;

    [SLua.CustomLuaClass]
    public delegate void ItemFunc(ItemInterface inter, object args);

    event ItemFunc OnUpdate;
    event ItemFunc OnUse;
    event ItemFunc OnHit;
    

    //[SLua.CustomLuaClass]
    //public delegate void ItemUsage();
    //private ItemUsage m_UsageDelegate;
    
    //[SLua.CustomLuaClass]
    //public delegate void ItemUpdate(LuaCache c, float deltaTime);
    //private ItemUpdate m_UpdateDelegate;

    //if needs load data from script, clone ones shall not
    public bool m_bIfLoaded = false;




    [SLua.CustomLuaClass]
    public class ItemInterface
    {
        [SLua.DoNotToLua]
        Item m_Item;

        public ItemInterface(Item item = null)
        {
            m_Item = item;
        }

        public LuaCache GetCache()
        {
            return m_Item.m_Cache;
        }
    }




	// Use this for initialization
	void Start () {


    }

    // Update is called once per frame
    void Update()
    {
        if(m_fCooldown > 0.0f) {
            m_fCooldown -= Time.deltaTime;
        }

        //animation & update
        if (!m_bPicked) {
            m_fFloat += Time.deltaTime;
            if (m_fFloat > Mathf.PI)
                m_fFloat -= Mathf.PI * 2.0f;

            gameObject.transform.position = new Vector3(0.0f, 0.5f*Mathf.Sin(m_fFloat) + 0.8f, 0.0f) + Vector3.Scale(gameObject.transform.position, new Vector3(1, 0, 1));
            gameObject.transform.Rotate(Vector3.up, 80.0f * Time.deltaTime);
        }
        else {
            //call update in lua
            if (OnUpdate != null)
                OnUpdate(m_Interface, Time.deltaTime);
        }
    }

    public void Use()
    {
        //pass the user ID,todo
        if(OnUse != null) {
            OnUse(m_Interface, Time.deltaTime);
        }
    }
    public void Hit(Entity target)
    {
        if(OnHit != null) {
            OnHit(m_Interface, target.m_Interface);
        }
    }

    public void Attack()
    {
        if(m_fCooldown <= 0.0f) {
            m_fCooldown = 1.0f / m_AttackSpeed;
        }
    }
    public bool IfCooled
    {
        get
        {
            return m_fCooldown <= 0.0f;
        }
    }

    public void OnDuplicate(DSignal signal)
    {
        Item _item = ((GameObject)signal._arg1).GetComponent<Item>();
        if (_item == null)
            return;
        else {
            //copy object references
            _item.OnUse = OnUse;
            _item.OnUpdate = OnUpdate;
            _item.OnHit = OnHit;
            _item.m_Cache = new LuaCache();
        }
    }

    private ItemFunc _CastFunc(object func)
    {
        if (func != null)
            return ((SLua.LuaFunction)func).cast<ItemFunc>();
        return null;
    }

    public void InitFromLuaFile() {
        SLua.LuaTable _table;
        //Script prefix path: Assets/Scripts/Lua/
        //read file
        SLua.LuaSvr.getInstance().doFile("Items/" + m_LuaScript);

        //functions
        OnUse += _CastFunc(SLua.LuaSvr.mainState["OnUse"]);
        OnUpdate += _CastFunc(SLua.LuaSvr.mainState["OnUpdate"]);
        OnHit += _CastFunc(SLua.LuaSvr.mainState["OnHit"]);

        //load all the properties
        _table = (SLua.LuaTable)SLua.LuaSvr.mainState["properties"];

        //load mesh
        m_Prefab = (string)_table["Mesh"];
        GameObject _tmp = Resources.Load<GameObject>("Meshes/" + m_Prefab);
        _tmp = Instantiate(_tmp);
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<MeshFilter>().mesh = _tmp.GetComponent<MeshFilter>().mesh;
        gameObject.GetComponent<Renderer>().material = _tmp.GetComponent<Renderer>().material;
        gameObject.transform.localScale = _tmp.transform.localScale;
        GameObject.Destroy(_tmp);
        if(gameObject.GetComponent<Transceiver>() == null)
            gameObject.AddComponent<Transceiver>();
        gameObject.GetComponent<Transceiver>().AddResolver("Duplicate", OnDuplicate);


        //attributes
        m_ItemName = (string)_table["Name"];
        m_Description = (string)_table["Description"];

        m_Type = (ItemType)System.Convert.ToInt32(_table["Type"]);
        switch (m_Type) {
            case ItemType.ITEM_WEAPON:
                m_WeaponType = (WeaponType)System.Convert.ToInt32(_table["ShotType"]);
                m_AttackSpeed = System.Convert.ToSingle(_table["AttackSpeed"]);
                m_EnergyCost = System.Convert.ToSingle(_table["EnergyCost"]);
                m_Damage = System.Convert.ToSingle(_table["FirePower"]);
                break;
            case ItemType.ITEM_PARTS:
                m_Damage = System.Convert.ToSingle(_table["Damage"]);
                m_Projectile = (string)_table["Ammo"];
                break;
            case ItemType.ITEM_ARMOR:
                m_Armor = System.Convert.ToSingle(_table["Armor"]);
                break;
            default:
                break;
        }
        m_Weight = System.Convert.ToSingle(_table["Weight"]);

        SLua.LuaTable _bonus = (SLua.LuaTable)_table["Bonus"];
        m_BonusList = new DAttributeBonus[_bonus.length()];
        for(int i=1; i<=_bonus.length(); i++) {
            SLua.LuaTable _oneBonus = (SLua.LuaTable)_bonus[i];
            m_BonusList[i - 1] = new DAttributeBonus(
                    (AttrType)System.Convert.ToInt32(_oneBonus[1]),
                    System.Convert.ToSingle(_oneBonus[3]),
                    System.Convert.ToBoolean(_oneBonus[2])
            );
        }
        
        m_bIfLoaded = true;
    }

    private void MarkItem(bool val = true)
    {
        //change shader
        if (val) {
            gameObject.GetComponent<MeshRenderer>().material.shader = Shader.Find("ZShader/Edge");
            gameObject.GetComponent<MeshRenderer>().material.SetColor("_OutlineCol", Color.green);
        }
        else
            gameObject.GetComponent<MeshRenderer>().material.shader = Shader.Find("Standard");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (m_bPicked || other.tag != "Player") return;
        Inventory _inv = other.gameObject.GetComponent<Inventory>();
        if(_inv != null) {
            if(_inv.m_ItemForPick != null) {
                _inv.m_ItemForPick.GetComponent<Item>().MarkItem(false);
            }
            _inv.m_ItemForPick = gameObject;
            MarkItem();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (m_bPicked || other.tag != "Player") return;
        Inventory _inv = other.gameObject.GetComponent<Inventory>();
        if (_inv != null) {
            if (_inv.m_ItemForPick == null) {
                _inv.m_ItemForPick = gameObject;
                MarkItem();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (m_bPicked || other.tag != "Player") return;
        Inventory _inv = other.gameObject.GetComponent<Inventory>();
        if (_inv != null) {
            if(_inv.m_ItemForPick == gameObject) {
                _inv.m_ItemForPick = null;
                MarkItem(false);
            }
        }
    }
}
