using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Buff_BuffInterface : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int constructor(IntPtr l) {
		try {
			Buff.BuffInterface o;
			Buff a1;
			checkType(l,2,out a1);
			o=new Buff.BuffInterface(a1);
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
			Buff.BuffInterface self=(Buff.BuffInterface)checkSelf(l);
			var ret=self.GetCache();
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
	static public int GetStackNum(IntPtr l) {
		try {
			Buff.BuffInterface self=(Buff.BuffInterface)checkSelf(l);
			var ret=self.GetStackNum();
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
	static public int GetTime(IntPtr l) {
		try {
			Buff.BuffInterface self=(Buff.BuffInterface)checkSelf(l);
			var ret=self.GetTime();
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
	static public int CreateBuff_s(IntPtr l) {
		try {
			SLua.LuaTable a1;
			checkType(l,1,out a1);
			var ret=Buff.BuffInterface.CreateBuff(a1);
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
		getTypeTable(l,"Buff.BuffInterface");
		addMember(l,GetCache);
		addMember(l,GetStackNum);
		addMember(l,GetTime);
		addMember(l,CreateBuff_s);
		createTypeMetatable(l,constructor, typeof(Buff.BuffInterface));
	}
}
