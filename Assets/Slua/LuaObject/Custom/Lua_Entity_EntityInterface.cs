using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Entity_EntityInterface : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int constructor(IntPtr l) {
		try {
			Entity.EntityInterface o;
			Entity a1;
			checkType(l,2,out a1);
			o=new Entity.EntityInterface(a1);
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int AddBuff(IntPtr l) {
		try {
			Entity.EntityInterface self=(Entity.EntityInterface)checkSelf(l);
			Buff.BuffInterface a1;
			checkType(l,2,out a1);
			self.AddBuff(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"Entity.EntityInterface");
		addMember(l,AddBuff);
		createTypeMetatable(l,constructor, typeof(Entity.EntityInterface));
	}
}
