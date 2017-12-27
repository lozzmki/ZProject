using UnityEngine;
using KBEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using _Entity = Entity;

public class NetEvent : MonoBehaviour
{
    public GameObject playerPrefab;
    private List<GameObject> ammos = new List<GameObject>();
    private Camera ctrlCamera;
    private bool m_bShotLock;

    // Use this for initialization
    void Start()
    {
        installEvents();
        StartCoroutine("loopWaitRangeFire");
        ctrlCamera = Camera.main;
    }

    void installEvents()
    {
        // register by kbe plugin
        KBEngine.Event.registerOut("addSpaceGeometryMapping", this, "addSpaceGeometryMapping");
        KBEngine.Event.registerOut("onEnterWorld", this, "onEnterWorld");
        KBEngine.Event.registerOut("onLeaveWorld", this, "onLeaveWorld");
        //KBEngine.Event.registerOut("set_position", this, "set_position");
        //KBEngine.Event.registerOut("set_direction", this, "set_direction");
        KBEngine.Event.registerOut("updatePosition", this, "updatePosition");
        KBEngine.Event.registerOut("updateDirection", this, "updateDirection");
        KBEngine.Event.registerOut("onControlled", this, "onControlled");

        // register by scripts
        KBEngine.Event.registerOut("onSceneAlloc", this, "onSceneAlloc");
        KBEngine.Event.registerOut("onProjectileEnterWorld", this, "onProjectileEnterWorld");
        KBEngine.Event.registerOut("onMoveObjEnterWorld", this, "onMoveObjEnterWorld");
        KBEngine.Event.registerOut("onRoleEnterWorld", this, "onRoleEnterWorld");
        KBEngine.Event.registerOut("set_name", this, "set_entityName");
        KBEngine.Event.registerOut("set_moveSpeed", this, "set_moveSpeed");
        KBEngine.Event.registerOut("recRangeFire", this, "recRangeFire");
    }

    public void onProjectileEnterWorld(UInt64 rndUUID, Int32 eid, KBEngine.Projectile entity)
    {
        Debug.Log("创建子弹对象!!");
        GameObject res = Resources.Load<GameObject>("Prefabs/Projectiles/" + entity.resName);
        GameObject _ammo = Instantiate(res, entity.position, entity.rotation);
        _ammo.GetComponent<SyncPosRot>().entity = entity;
        _ammo.GetComponent<SyncPosRot>().enabled = true;
        entity.renderObj = _ammo;

        //set init props
        set_position(entity);
        set_direction(entity);
        set_moveSpeed(entity);

        //set sync identity
        KBEngine.Entity player = KBEngineApp.app.player();
        if (player != null)
        {
            if (entity.masterId == player.id)
            {
                _ammo.GetComponent<SyncPosRot>().isLocalPlayer = true;
            }
        }

        GameObject master = KBEngineApp.app.findEntity(entity.masterId).renderObj as GameObject;
        if (master != null)
        {
            Transceiver.SendSignal(new DSignal(_ammo, master, "onAmmoCreated", _ammo));
        }

        if (!_ammo.GetComponent<SyncPosRot>().isLocalPlayer)
        {
            _ammo.GetComponent<Projectile>().enabled = false;
        }

        _ammo.SetActive(false);
        ammos.Add(_ammo);

        Debug.Log("被控制：" + entity.isControlled);
    }

    IEnumerator loopWaitRangeFire()
    {
        while(true)
        {
            waitRangeFire(WeaponType.WEAPON_SPREAD);
            yield return null;
        }
    }

    bool waitRangeFire(WeaponType shotType)
    {
        //calculate count
        int parallelCount = 1;
        switch (shotType)
        {
            case WeaponType.WEAPON_SPREAD: parallelCount = 5; break;
        }

        //parallel shot
        if (ammos.Count >= parallelCount)
        {
            for (int i = 0; i < parallelCount; i++)
            {
                GameObject ammo = ammos[0];
                if(ammo != null)
                {
                    ammo.transform.Rotate(0, 20.0f - 10.0f * i, 0);
                    ammo.SetActive(true);
                    ammos.RemoveAt(0);
                    //while (true)
                    //{
                    //    if (!m_bShotLock)
                    //    {
                    //        m_bShotLock = true;

                    //        ammos.RemoveAt(0);

                    //        m_bShotLock = false;
                    //        break;
                    //    }
                    //}
                }
                else
                {
                    //Debug.Log("emmmmm");
                }
                //ammo.transform.position = master.transform.position;

            }
            return true;
        }
        return false;
    }

    public void recRangeFire(Byte res, UInt32 shotType)
    {
        if(res == 0)
        {
            WeaponType _shotType = (WeaponType)shotType;
            //StartCoroutine("waitRangeFire", _shotType);
        }
        else
        {
            //ammos.Clear();
        }
    }

    public void onSceneAlloc(UInt64 rndUUID, Int32 eid, KBEngine.SceneAlloc alloc)
    {
        SceneSyncBind.reqMoveObjsSync();
    }

    public void onMoveObjEnterWorld(UInt64 rndUUID, Int32 eid, KBEngine.MoveObj entity)
    {
        //instantiate
        GameObject obj = GameObject.Find(entity.name);
        if (obj == null) return;
        obj.GetComponent<Renderer>().enabled = true;
        entity.renderObj = obj;

        //set init props
        set_position(entity);
        set_direction(entity);

        //set sync entity
        obj.GetComponent<SyncPosRot>().entity = entity;
        obj.GetComponent<SyncPosRot>().isDynamicMode = true;
        obj.GetComponent<SyncPosRot>().isLerpMotion = true;
        obj.GetComponent<SyncPosRot>().enabled = true;
    }

    public void onRoleEnterWorld(UInt64 rndUUID, Int32 eid, KBEngine.Role entity)
    {
        Debug.Log("当前玩家[" + entity.id + "]进入游戏世界...");

        //instantiate
        float y = entity.position.y;
        if (entity.isOnGround)
            y = 1.3f;

        Vector3 pos = new Vector3(entity.position.x, y, entity.position.z);
        GameObject player = Instantiate(playerPrefab, pos, entity.rotation) as GameObject;
        entity.renderObj = player;

        //set init props
        set_position(entity);
        set_direction(entity);
        set_entityName(entity);
        set_moveSpeed(entity);
        player.GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture>("pic/" + ((Role)entity).career_name);

        //set sync entity
        player.GetComponent<SyncPosRot>().entity = entity;
        player.GetComponent<SyncPosRot>().enabled = true;

        //set local controll
        if(entity.isPlayer())
        {
            ctrlCamera.GetComponent<GameInput>().m_Player = player;
            ctrlCamera.GetComponent<GameInput>().enabled = true;
            player.GetComponent<SyncPosRot>().isLocalPlayer = true;
        }
    }

    public void onEnterWorld(KBEngine.PropsEntity entity)
    {
       
    }

    public void onLeaveWorld(KBEngine.PropsEntity entity)
    {
        if (entity.renderObj == null)
            return;

        Destroy((UnityEngine.GameObject)entity.renderObj);
        entity.renderObj = null;
    }

    public void addSpaceGeometryMapping(string respath)
    {
        Debug.Log("加载场景(" + respath + ")...");
    }

    public void set_position(KBEngine.PropsEntity entity)
    {
        if (entity.renderObj == null)
            return;

        SyncPosRot syncScript = ((GameObject)entity.renderObj).GetComponent<SyncPosRot>();
        syncScript.syncPos = entity.position;
        syncScript.position = entity.position;
        syncScript.spaceID = KBEngineApp.app.spaceID;

        updatePosition(entity);
    }

    public void updatePosition(KBEngine.PropsEntity entity)
    {
        if (entity.renderObj == null)
            return;

        SyncPosRot syncScript = ((GameObject)entity.renderObj).GetComponent<SyncPosRot>();
        syncScript.syncPos = entity.position;
        syncScript.spaceID = KBEngineApp.app.spaceID;
    }

    public void set_direction(KBEngine.PropsEntity entity)
    {
        if (entity.renderObj == null)
            return;

        SyncPosRot syncScript = ((GameObject)entity.renderObj).GetComponent<SyncPosRot>();
        syncScript.syncEuler = entity.eulerAngles;
        syncScript.eulerAngles = entity.eulerAngles;
        syncScript.spaceID = KBEngineApp.app.spaceID;
    }

    public void updateDirection(KBEngine.PropsEntity entity)
    {
        if (entity.renderObj == null)
            return;

        SyncPosRot syncScript = ((GameObject)entity.renderObj).GetComponent<SyncPosRot>();
        syncScript.syncEuler = entity.eulerAngles;
        syncScript.spaceID = KBEngineApp.app.spaceID;
    }

    public void onControlled(KBEngine.Entity entity, bool isControlled)
    {
        if (entity.renderObj == null)
            return;

        Debug.Log("onControlled: " + entity.isControlled);

    }

    //=================================================================
    //Props process here
    //=================================================================
    private delegate void process(_Entity e);
    private void SAFE_PROCESS(KBEngine.PropsEntity e, process p)
    {
        GameObject obj = (GameObject)e.renderObj;
        if (obj != null)
        {
            _Entity _e = obj.GetComponent<_Entity>();
            if (_e != null)
            {
                p(_e);
            }
        }
    }

    public void set_entityName(KBEngine.PropsEntity e)
    {
        process _p = (_e) =>
        {
            _e.m_EntityName = e.name;
        };
        SAFE_PROCESS(e, _p);
    }

    public void set_moveSpeed(KBEngine.PropsEntity e)
    {
        process _p = (_e) =>
        {
            _e.m_Properties[_Entity.SPEED].d_Value = e.speed;
        };
        if(e.className == "Projectile")
        {
            GameObject obj = (GameObject)e.renderObj;
            if (obj != null)
            {
                Projectile _e = obj.GetComponent<Projectile>();
                if (_e != null)
                {
                    _e.m_Speed = e.speed;
                }
            }
        }
        else
            SAFE_PROCESS(e, _p);    
    }
}
