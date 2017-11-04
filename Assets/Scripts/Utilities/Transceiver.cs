using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct DSignal
{
    public GameObject _sender;
    public GameObject _receiver;
    public string _key;
    public object _arg1;
    public object _arg2;
    public DSignal(GameObject sender = null, GameObject receiver = null, string key = "", object arg1 = null, object arg2 = null)
    {
        _sender = sender;
        _receiver = receiver;
        _key = key;
        _arg1 = arg1;
        _arg2 = arg2;
    }
}
public delegate void SignalResolver(DSignal signal);
/// <summary>
/// 绑定游戏物体的信号收发器
/// </summary>
public class Transceiver : MonoBehaviour
{
    public static bool ms_bIsOnlineMode = false;
    private Dictionary<string, SignalResolver> m_ResolverDic;
    // Use this for initialization
    void Awake()
    {
        m_ResolverDic = new Dictionary<string, SignalResolver>();
    }
    public void AddResolver(string key, SignalResolver newResolver)
    {
        if (!m_ResolverDic.ContainsKey(key)) {
            m_ResolverDic.Add(key, null);
        }
        m_ResolverDic[key] += newResolver;
    }
    public void RemoveResolver(string key, SignalResolver theResolver)
    {
        if (!m_ResolverDic.ContainsKey(key)) {
            return;
        }
        m_ResolverDic[key] -= theResolver;
    }
    public static void SendSignal(DSignal signal)
    {
        if (!ms_bIsOnlineMode) {
            //single mode, send to the object directly
            if (signal._receiver)
            {
                signal._receiver.GetComponent<Transceiver>().OnReceiveSignal(signal);
            }
        }
        else {
            //online mode, send it to the server
        }
    }
    public void OnReceiveSignal(DSignal signal)
    {
        if (m_ResolverDic.ContainsKey(signal._key)) {
            SignalResolver _rsvr = m_ResolverDic[signal._key];
            if (_rsvr != null)
                _rsvr(signal);
        }
    }
}
