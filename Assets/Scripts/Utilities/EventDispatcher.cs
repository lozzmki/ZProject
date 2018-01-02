using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct LEvent
{
    public string _event;
    public object _arg1;
    public object _arg2;
    public LEvent(string sEvent = "", object arg1 = null, object arg2 = null)
    {
        _event = sEvent;
        _arg1 = arg1;
        _arg2 = arg2;
    }
}

/// <summary>
/// Global Event Manager
/// </summary>
public class EventDispatcher {

    public delegate void LEventListener(LEvent _event);
    private static Dictionary<string, LEventListener> _s_listeners;
    private static Dictionary<string, LEventListener> GetListeners()
    {
        if(_s_listeners == null) {
            _s_listeners = new Dictionary<string, LEventListener>();
        }
        return _s_listeners;
    }

    public static void AddListener(string eventName, LEventListener newListener)
    {
        var _listeners = GetListeners();
        if (!_listeners.ContainsKey(eventName)) {
            _listeners.Add(eventName, null);
        }
        _listeners[eventName] += newListener;
    }

    public static void RemoveListener(string eventName, LEventListener theListener)
    {
        var _listeners = GetListeners();
        if (_listeners.ContainsKey(eventName)) {
            _listeners[eventName] -= theListener;
        }
    }

    public static void FireEvent(LEvent levent)
    {
        var _listeners = GetListeners();
        if (_listeners.ContainsKey(levent._event)) {
            _listeners[levent._event](levent);
        }
    }

    public static void FireEvent(string key = "", object arg1 = null, object arg2 = null)
    {
        FireEvent(new LEvent(key, arg1, arg2));
    }

}
