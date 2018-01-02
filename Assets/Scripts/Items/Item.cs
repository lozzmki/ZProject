using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    ITEM_WEAPON,
    ITEM_PARTS,
    ITEM_ARMOR,
    ITEM_CHIP,
}

public enum WeaponType
{
    WEAPON_ARC,
    WEAPON_AUTO,
    WEAPON_SPREAD,
    WEAPON_LASER,
    WEAPON_MELEE,
}

public enum ItemRarity
{
    RARITY_UNIQUE,
    RARITY_BASIC,
    RARITY_ADVANCED,
    RARITY_RARE,
}

/// <summary>
/// 物品的模板类
/// 使用Lua文件装载数据
/// </summary>
public class Item : MonoBehaviour {
    #region Properties
    public string m_ItemName = "Default Item Name";
    public string m_Description = "No Description";
    public ItemType m_Type;
    public WeaponType m_WeaponType;
    public ItemRarity m_Rarity;
    public float m_AttackSpeed;
    public float m_EnergyCost;
    public float m_SkillCost;
    public float m_SkillCooldown;
    public float m_Damage;
    public float m_Armor;
    public int m_nChipCost;
    public int m_Price;
    public string m_LuaScript="";
    public string m_Prefab = "";
    public string m_Projectile = "";
    public DAttributeBonus[] m_BonusList;
    public Texture2D m_Icon;
    #endregion

    #region Param
    public bool m_bPicked = false;
    private Timer m_AttackTimer;
    private Timer m_SkillTimer;
    public Inventory m_Inventory;//ref to inv if picked
    public bool m_bForSale = false;
    #endregion

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
    //private float m_fFloat = 0.0f;

    [SLua.CustomLuaClass]
    public delegate void ItemFunc(ItemInterface inter, object args);

    event ItemFunc OnUpdate;
    event ItemFunc OnUse;
    event ItemFunc OnHit;

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

        public Entity.EntityInterface GetOwner()
        {
            if (m_Item.m_bPicked)
                return m_Item.m_Inventory.gameObject.GetComponent<Entity>().m_Interface;
            else
                return null;
        }
    }

	// Use this for initialization
	//void Start () {
    //
    //
    //}

    // Update is called once per frame
    void Update()
    {
        if(m_AttackTimer != null)
            m_AttackTimer.Update();

        //update
        //if (m_bPicked) {
        //    //call update in lua
        //    if (OnUpdate != null)
        //        OnUpdate(m_Interface, Time.deltaTime);
        //}
    }

    //update when picked && in hand, called by inventory
    public void ActiveUpdate()
    {
        if (OnUpdate != null)
            OnUpdate(m_Interface, Time.deltaTime);
    }

    public void Use()
    {
        //pass the user ID,FIN
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
        if(m_AttackTimer.IfExpired) {
            m_AttackTimer.TimerTime = 1.0f / m_AttackSpeed;
            m_AttackTimer.Reset();
        }
    }
    public bool IfCooled
    {
        get
        {
            return m_AttackTimer.IfExpired;
        }
    }
    public void CastSkill()
    {
        if (m_SkillTimer.IfExpired) {
            m_SkillTimer.TimerTime = m_SkillCooldown;
            m_SkillTimer.Reset();
        }
    }
    public bool IfSkillCooled
    {
        get
        {
            return m_SkillTimer.IfExpired;
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
            _item.m_AttackTimer = new Timer();
            _item.m_SkillTimer = new Timer();
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
        gameObject.AddComponent<MeshFilter>().mesh = _tmp.GetComponent<MeshFilter>().mesh;
        //gameObject.AddComponent<MeshRenderer>().material = _tmp.GetComponent<Renderer>().material;
        gameObject.AddComponent<MeshRenderer>().materials = _tmp.GetComponent<Renderer>().materials;
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
                //m_WeaponType = (WeaponType)((int)_table["ShotType"]);
                m_AttackSpeed = System.Convert.ToSingle(_table["AttackSpeed"]);
                m_EnergyCost = System.Convert.ToSingle(_table["EnergyCost"]);
                m_SkillCost = System.Convert.ToSingle(_table["SkillCost"]);
                m_SkillCooldown = System.Convert.ToSingle(_table["SkillCooldown"]);
                m_Damage = System.Convert.ToSingle(_table["FirePower"]);
                break;
            case ItemType.ITEM_PARTS:
                m_Damage = System.Convert.ToSingle(_table["Damage"]);
                m_Projectile = (string)_table["Ammo"];
                break;
            case ItemType.ITEM_ARMOR:
                m_Armor = System.Convert.ToSingle(_table["Armor"]);
                break;
            case ItemType.ITEM_CHIP:
                m_nChipCost = System.Convert.ToInt32(_table["Cost"]);
                break;
            default:
                break;
        }
        m_Rarity = (ItemRarity)System.Convert.ToInt32(_table["Rarity"]);
        
        m_Price = System.Convert.ToInt32(_table["Price"]);

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

    public void MarkItem(bool val = true)
    {
        //change shader
        if (val) {
            //MeshRenderer[] _renderers = gameObject.GetComponents<MeshRenderer>();
            Camera.main.GetComponent<HighLightEdge>().renderList.Add(gameObject.GetComponent<MeshRenderer>());
            ItemHUD.markItem = gameObject;
        }
        else {
            Camera.main.GetComponent<HighLightEdge>().renderList.Clear();
            ItemHUD.markItem = null;
        }

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
