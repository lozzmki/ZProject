using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_LuaCache : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int constructor(IntPtr l) {
		try {
			LuaCache o;
			o=new LuaCache();
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
	static public int StoreValue(IntPtr l) {
		try {
			LuaCache self=(LuaCache)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Object a2;
			checkType(l,3,out a2);
			self.StoreValue(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int LoadValue(IntPtr l) {
		try {
			LuaCache self=(LuaCache)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.LoadValue(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_GlobalTicket(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,LuaCache.GlobalTicket);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_GlobalCache(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,LuaCache.GlobalCache);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"LuaCache");
		addMember(l,StoreValue);
		addMember(l,LoadValue);
		addMember(l,"GlobalTicket",get_GlobalTicket,null,false);
		addMember(l,"GlobalCache",get_GlobalCache,null,false);
		createTypeMetatable(l,constructor, typeof(LuaCache));
	}
}
