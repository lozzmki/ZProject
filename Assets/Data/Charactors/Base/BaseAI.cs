using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
[RequireComponent(typeof(Animator))]
public abstract class BaseAI : MonoBehaviour {
    public delegate void StateHandler();

    protected Animator m_Animator;
    protected Entity m_Entity;
    protected Dictionary<int, StateHandler> m_Handlers;
	// Use this for initialization
	void Start () {
        m_Animator = gameObject.GetComponent<Animator>();
        m_Entity = gameObject.GetComponent<Entity>();
        m_Handlers = new Dictionary<int, StateHandler>();
        Init();
	}
	
	// Update is called once per frame
	void Update () {
        var _state = m_Animator.GetCurrentAnimatorStateInfo(0);
        int _key = _state.shortNameHash;
        if(!m_Animator.IsInTransition(0)) {
            if (m_Handlers.ContainsKey(_key)) {
                StateHandler _handler = m_Handlers[_key];
                if (_handler != null) {
                    _handler();
                }
            }
            else {
                Debug.LogWarning("Unhandled State in " + gameObject);
            }
        }
    }

    protected void RegisterHandler(string name, StateHandler handler)
    {
        int _key = Animator.StringToHash(name);
        if (!m_Handlers.ContainsKey(_key)) {
            m_Handlers.Add(_key, null);
        }
        m_Handlers[_key] += handler;
    }

    protected abstract void Init();
}
