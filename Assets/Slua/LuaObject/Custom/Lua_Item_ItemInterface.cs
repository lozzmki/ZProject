using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Item_ItemInterface : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int constructor(IntPtr l) {
		try {
			Item.ItemInterface o;
			Item a1;
			checkType(l,2,out a1);
			o=new Item.ItemInterface(a1);
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
	static public int GetCache(IntPtr l) {
		try {
			Item.ItemInterface self=(Item.ItemInterface)checkSelf(l);
			var ret=self.GetCache();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"Item.ItemInterface");
		addMember(l,GetCache);
		createTypeMetatable(l,constructor, typeof(Item.ItemInterface));
	}
}
