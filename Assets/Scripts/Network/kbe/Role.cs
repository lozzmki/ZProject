using UnityEngine;
using KBEngine;
using System;
using System.Collections;

namespace KBEngine
{
    public class Role : PropsEntity
    {
        public string career_name
        {
            get
            {
                Byte career = (Byte)getDefinedProperty("career");
                switch (career)
                {
                    case 1: return "saber";
                    case 2: return "gunner";
                }
                return "";
            }
        }

        public override void __init__()
        {
            Debug.Log("角色初始化!!");

            if (isPlayer())
            {
                installEvents();
            }
        }

        public void installEvents()
        {
            Event.registerIn("reqRangeFire", this, "reqRangeFire");
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
            Debug.Log("Role: onEnterWorld call");

            Event.fireOut("onRoleEnterWorld", new object[] { KBEngineApp.app.entity_uuid, id, this });
        }

        public virtual void reqRangeFire(int masterId, string resName, UInt32 shotType, Vector3 pos, Vector3 euler)
        {
            baseCall("reqRangeFire", masterId, resName, shotType, pos, euler);
        }

        public virtual void recRangeFire(Byte res, UInt32 shotType)
        {
            Event.fireOut("recRangeFire", new object[] { res, shotType });
        }
    }
}