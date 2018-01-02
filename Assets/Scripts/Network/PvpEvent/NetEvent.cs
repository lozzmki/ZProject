using UnityEngine;
using KBEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using _Entity = Entity;

public class NetEvent : MonoBehaviour
{
    public GameObject playerPrefab;
    private Camera ctrlCamera;

    // Use this for initialization
    void Start()
    {
        gameObject.AddComponent<AmmoNetEvent>();
        installEvents();
        ctrlCamera = Camera.main;
    }

    void installEvents()
    {
        // register by kbe plugin
        KBEngine.Event.registerOut("addSpaceGeometryMapping", this, "addSpaceGeometryMapping");
        KBEngine.Event.registerOut("onEnterWorld", this, "onEnterWorld");
        KBEngine.Event.registerOut("onLeaveWorld", this, "onLeaveWorld");
        KBEngine.Event.registerOut("set_position", this, "set_position");
        //KBEngine.Event.registerOut("set_direction", this, "set_direction");
        KBEngine.Event.registerOut("updatePosition", this, "updatePosition");
        KBEngine.Event.registerOut("updateDirection", this, "updateDirection");
        KBEngine.Event.registerOut("onControlled", this, "onControlled");
        KBEngine.Event.registerOut("onLoseControlledEntity", this, "onLoseControlledEntity");

        // register by scripts
        KBEngine.Event.registerOut("onSceneAlloc", this, "onSceneAlloc");
        KBEngine.Event.registerOut("onMoveObjEnterWorld", this, "onMoveObjEnterWorld");
        KBEngine.Event.registerOut("onRoleEnterWorld", this, "onRoleEnterWorld");
        KBEngine.Event.registerOut("onObjKilled", this, "onObjKilled");
        KBEngine.Event.registerOut("set_name", this, "set_entityName");
        KBEngine.Event.registerOut("set_moveSpeed", this, "set_moveSpeed");
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

    public void onObjKilled(KBEngine.PropsEntity entity)
    {
        if (entity.renderObj == null)
            return;

        Destroy((GameObject)entity.renderObj);
        entity.renderObj = null;
    }
    
    public void onEnterWorld(KBEngine.PropsEntity entity)
    {
       
    }

    public void onLeaveWorld(KBEngine.PropsEntity entity)
    {
        if (entity.renderObj == null)
            return;

        if (entity.className == "Projectile")
            return;

        Destroy((GameObject)entity.renderObj);
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
        syncScript.RealSyncPosition(entity);
        syncScript.spaceID = KBEngineApp.app.spaceID;
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
        syncScript.RealSyncRotation(entity);
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

    public void onLoseControlledEntity(KBEngine.Entity entity)
    {
        return;
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
