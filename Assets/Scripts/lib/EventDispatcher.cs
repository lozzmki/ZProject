using System.Collections.Generic;

namespace nglib {

    public enum EventType
    {
        EVENT_DEFAULT,

   }
    public class Event
    {
        public EventType _nType;
        public int _nArg1=0;
        public int _nArg2=0;
        public float _fArg1=0.0f;
        public float _fArg2=0.0f;

        public Event(EventType nType = EventType.EVENT_DEFAULT)
        {
            _nType = nType;
        }
    }
    public interface IEventListener
    {
        bool handleEvent(Event e);
    }
    struct DTempListenerNode
    {
        public EventType _nEventID;
        public IEventListener _listener;
        public DTempListenerNode(EventType nEventID, IEventListener iListener)
        {
            _nEventID = nEventID;
            _listener = iListener;
        }
    }
    //弃用
    public class EventDispatcher {

        private static EventDispatcher s_inst;
        private Dictionary<int, LinkedList<IEventListener>> m_EventListeners;
        private bool m_bSafeLock;
        private LinkedList<Event> m_EventQueue;
        private LinkedList<DTempListenerNode> m_AddRequests;
        private LinkedList<DTempListenerNode> m_RemoveRequests;

        public static EventDispatcher getInstance()
        {
            if (s_inst == null)
            {
                s_inst = new EventDispatcher();
            }
            return s_inst;
        }
        private EventDispatcher()
        {
            m_EventListeners = new Dictionary<int, LinkedList<IEventListener>>();
            m_EventQueue = new LinkedList<Event>();
            m_AddRequests = new LinkedList<DTempListenerNode>();
            m_RemoveRequests = new LinkedList<DTempListenerNode>();
            m_bSafeLock = false;
        }
        

        public void addListener(EventType nEventID, IEventListener iListener)
        {
            if (m_bSafeLock)
            {
                m_AddRequests.AddLast(new DTempListenerNode(nEventID, iListener));
                return;
            }

            LinkedList<IEventListener> _list;
            if (!m_EventListeners.ContainsKey((int)nEventID))
            {
                _list = new LinkedList<IEventListener>();
                m_EventListeners.Add((int)nEventID, _list);
            }
            else
            {
                _list = m_EventListeners[(int)nEventID];
            }

            _list.AddLast(iListener);
        }

        public void removeListener(EventType nEventID, IEventListener iListener)
        {
            if (m_bSafeLock)
            {
                m_RemoveRequests.AddLast(new DTempListenerNode(nEventID, iListener));
                return;
            }

            LinkedList<IEventListener> _list;
            if (!m_EventListeners.ContainsKey((int)nEventID))
            {
                return;
            }
            else
            {
                _list = m_EventListeners[(int)nEventID];
            }

            foreach (IEventListener _p in _list)
            {
                if(_p == iListener)
                {
                    _list.Remove(_p);
                    return;
                }
            }
        }

        public void fireEvent(Event e)
        {
            if(!m_bSafeLock){
                m_bSafeLock = true;
                if (m_EventListeners.ContainsKey((int)e._nType))
                {
                    LinkedList<IEventListener> _list = m_EventListeners[(int)e._nType];
                    foreach (IEventListener _p in _list)
                    {
                        if (_p.handleEvent(e))
                        {
                            break;
                        }
                    }
                }
                m_bSafeLock = false;

                //add&remove listeners
                foreach(DTempListenerNode _d in m_AddRequests)
                {
                    addListener(_d._nEventID, _d._listener);
                }
                m_AddRequests.Clear();
                foreach (DTempListenerNode _d in m_RemoveRequests)
                {
                    removeListener(_d._nEventID, _d._listener);
                }
                m_RemoveRequests.Clear();

                //next event
                if (m_EventQueue.Count > 0)
                {
                    Event _nextEvent = m_EventQueue.First.Value;
                    m_EventQueue.RemoveFirst();
                    fireEvent(_nextEvent);
                }
            }
            else
            {
                //another event is firing, save to queue
                m_EventQueue.AddLast(e);
            }
        }
    }
}