using UnityEngine;
using System;
using System.Collections;

namespace KBEngine
{
    public class Projectile : PropsEntity
    {
        public int masterId
        {
            get
            {
                return (int)getDefinedProperty("masterId");
            }
        }

        public string resName
        {
            get
            {
                return (string)getDefinedProperty("resName");
            }
        }

        public UInt32 shotType
        {
            get
            {
                return (UInt32)getDefinedProperty("shotType");
            }
        }

        public override void __init__()
        {
            Debug.Log("子弹初始化!!");
            installEvents();
        }

        public void installEvents()
        {
            Event.registerIn("reqDestroySelf", this, "reqDestroySelf");
        }

        public override void onDestroy()
        {
            if (isPlayer())
            {
                KBEngine.Event.deregisterIn(this);
            }
        }

        public override void onEnterWorld()
        {
            Event.fireOut("onProjectileEnterWorld", new object[] { KBEngineApp.app.entity_uuid, id, this });
        }

        public virtual void reqDestroySelf()
        {
            baseCall("reqDestroySelf");
        }

    }
}
