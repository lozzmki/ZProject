using UnityEngine;
using KBEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using _Entity = Entity;

public class AmmoNetEvent : MonoBehaviour
{
    //shot data cache
    private Dictionary<double, List<GameObject>> ammosDic = new Dictionary<double, List<GameObject>>();
    private List<double> timeoutkeys = new List<double>();
    private double timeout = 2.0f;

    // Use this for initialization
    void Start()
    {
        installEvents();
        StartCoroutine("loopWaitRangeFire");
    }

    void installEvents()
    {
        // register by scripts
        KBEngine.Event.registerOut("onProjectileEnterWorld", this, "onProjectileEnterWorld");
        KBEngine.Event.registerOut("recRangeFire", this, "recRangeFire");
    }

    public void onProjectileEnterWorld(UInt64 rndUUID, Int32 eid, KBEngine.Projectile entity)
    {
        //instantiate
        GameObject res = Resources.Load<GameObject>("Prefabs/Projectiles/" + entity.resName);
        GameObject ammo = Instantiate(res, entity.position, entity.rotation);
        ammo.GetComponent<SyncPosRot>().entity = entity;
        ammo.GetComponent<SyncPosRot>().enabled = true;
        entity.renderObj = ammo;

        //set sync identity
        KBEngine.Entity player = KBEngineApp.app.player();
        if (player != null)
        {
            if (entity.masterId == player.id)
            {
                ammo.GetComponent<SyncPosRot>().isLocalPlayer = true;
            }
            else
            {
                ammo.GetComponent<Projectile>().enabled = false;
            }
        }

        //set sync props
        ammo.GetComponent<SyncPosRot>().RealSync(entity);
        ammo.GetComponent<Projectile>().m_Speed = entity.speed;


        //set local props
        KBEngine.Entity masterEntity = KBEngineApp.app.findEntity(entity.masterId);
        if(masterEntity != null)
        {
            GameObject master = masterEntity.renderObj as GameObject;
            if (master != null)
            {
                //Transceiver.SendSignal(new DSignal(_ammo, master, "onAmmoCreated", _ammo));
                Item _weapon = master.GetComponent<Inventory>().GetWeapon();
                Item _parts = master.GetComponent<Inventory>().GetParts();
                float _baseDmg = 0.0f;
                if (_weapon != null) _baseDmg += _weapon.m_Damage;

                //temporary, todo
                GameObject _proj = ammo;
                _proj.transform.forward = master.transform.forward;
                _proj.GetComponent<Projectile>().m_Master = master;
                _proj.GetComponent<Projectile>().m_Damage += (_baseDmg + master.transform.GetComponent<Entity>().m_Properties[Entity.RANGE_POWER].d_Value);
                _proj.GetComponent<Projectile>().m_Weapon = _weapon;
                _proj.GetComponent<Projectile>().m_Parts = _parts;
            }
        }

        // wait for parallel shot
        if (ammo.GetComponent<SyncPosRot>().isLocalPlayer)
        {
            ammo.SetActive(false);
            if (!ammosDic.ContainsKey(entity.bornTime))
                ammosDic[entity.bornTime] = new List<GameObject>();
            ammosDic[entity.bornTime].Add(ammo);
        }
    }

    IEnumerator loopWaitRangeFire()
    {
        while(true)
        {
            waitRangeFire(WeaponType.WEAPON_SPREAD);
            yield return null;
        }
    }

    void waitRangeFire(WeaponType shotType)
    {
        //calculate count
        int parallelCount = 1;
        switch (shotType)
        {
            case WeaponType.WEAPON_SPREAD: parallelCount = 5; break;
        }

        //iter for shot
        foreach (double timestamp in ammosDic.Keys)
        {
            Debug.Log("shotTimestamp: " + timestamp.ToString());
            Debug.Log("nowTimestamp: " + TimeEx.getTimeStamp());

            //when receive timeout, means failing, we just mark it
            if (TimeEx.getTimeStamp() - timestamp > timeout)
            {
                timeoutkeys.Add(timestamp);
                continue;
            }

            //ready for shot
            List <GameObject> ammos = ammosDic[timestamp];
            if(ammos.Count >= parallelCount)
            {
                for(int i = 0; i < ammos.Count; i++)
                {
                    //set transform
                    GameObject ammo = ammos[i];
                    GameObject master = ammo.GetComponent<Projectile>().m_Master;
                    ammo.transform.position = master.transform.position;
                    ammo.transform.Rotate(0, 20.0f - 10.0f * i, 0);

                    //set sync transfrom
                    SyncPosRot syncScript = ammo.GetComponent<SyncPosRot>();
                    if (!syncScript.isLocalPlayer)
                    {
                        syncScript.RealSync(ammo);
                    }

                    //active
                    ammo.SetActive(true);
                }
                ammos.Clear();
                ammosDic.Remove(timestamp);
                break;
            }
        }

        //clear timeout data
        foreach(double timestamp in timeoutkeys)
        {
            List <GameObject> ammos = ammosDic[timestamp];
            for (int i = 0; i < ammos.Count; i++)
            {
                GameObject ammo = ammos[i];
                ammo.GetComponent<Projectile>().Safe_Destroy();
            }
            ammos.Clear();
            ammosDic.Remove(timestamp);
        }
        timeoutkeys.Clear();
    }

    public void recRangeFire(Byte res, UInt32 shotType)
    {
        if(res == 0)
        {
            WeaponType _shotType = (WeaponType)shotType;
        }
    }
}
