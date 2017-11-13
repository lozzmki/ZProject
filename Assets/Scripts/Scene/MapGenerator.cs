using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DDoor
{
    public int x, y;//relative to the room
    public DPointN d_ptGlobalPos;//used in map generating
    public short d_nPosition; //relative to the room
    public const short TOP = 0x1;
    public const short LEFT = 0x2;
    public const short BOTTOM = 0x4;
    public const short RIGHT = 0x8;
    public DDoor(int xval = 0, int yval = 0, short nPosition = TOP)
    {
        x = xval; y = yval; d_nPosition = nPosition;
        d_ptGlobalPos = new DPointN();
    }
    public DDoor(DDoor door)
    {
        x = door.x;
        y = door.y;
        d_ptGlobalPos = new DPointN(door.d_ptGlobalPos.x, door.d_ptGlobalPos.y);
        d_nPosition = door.d_nPosition;

    }
    public bool CheckPair(DDoor other)
    {
        short _result = (short)(d_nPosition | other.d_nPosition);
        return (_result == 0x5) || (_result == 0xA);
    }
}

public class DRoom
{
    //index of data
    public int d_ID;
    //contains the border
    public DSizeN d_Size;
    //public string d_sContents;todo
    public LinkedList<DDoor> d_DoorList;

    public DRoom()
    {
        d_DoorList = new LinkedList<DDoor>();
    }
    public DRoom(DRoom room)
    {
        d_ID = room.d_ID;
        d_Size = new DSizeN(room.d_Size.d_nWidth, room.d_Size.d_nHeight);
        d_DoorList = new LinkedList<DDoor>();
        foreach(DDoor _door in room.d_DoorList) {
            d_DoorList.AddLast(new DDoor(_door));
        }
    }
}

class DSavedRoom
{
    //contains the border
    public Rect d_Rect;
    public int d_ID;

    public DSavedRoom(Rect rct, int ID)
    {
        d_Rect = rct;
        d_ID = ID;
    }
}

/// <summary>
/// Random map generator
/// - Generate map with preset rooms
/// </summary>
class MapGenerator
{
    //m_Seed;

    static LinkedList<DSavedRoom> m_RoomList = new LinkedList<DSavedRoom>();
    static LinkedList<DDoor> m_OpeningDoorList = new LinkedList<DDoor>();
    static LinkedList<DDoor> m_ClosedDoorList = new LinkedList<DDoor>();

    private static bool CheckRoom(DRoom room, DDoor door, DPointN globalAnchor)
    {
        Rect _newRoom, _savedRoom;
        _newRoom = new Rect(
            new Vector2(globalAnchor.x - door.x, globalAnchor.y - door.y),
            new Vector2(room.d_Size.d_nWidth-1, room.d_Size.d_nHeight-1)
            );

        //check if is collided with exist rooms
        foreach (DSavedRoom _sroom in m_RoomList) {
            _savedRoom = _sroom.d_Rect;
            if (_newRoom.Overlaps(_savedRoom)) {
                return false;
            }
        }

        return true;
    }

    private static bool CheckDoorWithRoom(Rect room, DPointN door)
    {
        int _minX = (int)room.x;
        int _maxX = (int)room.x + (int)room.width;
        int _minY = (int)room.y;
        int _maxY = (int)room.y + (int)room.height;

        return door.x >= _minX && door.x <= _maxX && door.y >= _minY && door.y <= _maxY;

    }

    private static void RegisterNewRoom(DRoom room, DDoor door/*the one connected with the map*/, DPointN globalAnchor)
    {
        //DOC:
        //recheck the opendoor list with the new room, remove unaccessable doors;
        //add new doors to opendoorlist

        Rect _newRoom = new Rect(
            new Vector2(globalAnchor.x - door.x, globalAnchor.y - door.y),
            new Vector2(room.d_Size.d_nWidth-1, room.d_Size.d_nHeight-1)
            );

        //set position for new room's doors
        foreach (DDoor _dr in room.d_DoorList) {
            _dr.d_ptGlobalPos = new DPointN(globalAnchor.x - door.x + _dr.x, globalAnchor.y - door.y + _dr.y);
        }

        //check new doors with the opening doors
        for (LinkedListNode<DDoor> _node = m_OpeningDoorList.First; _node != null;) {
            LinkedListNode<DDoor> _tempNode = _node.Next;
            if (CheckDoorWithRoom(_newRoom, _node.Value.d_ptGlobalPos)) {
            //if (_newRoom.Contains(_node.Value.d_ptGlobalPos.ToVec2())) {
                //the door should be removed anyway
                m_OpeningDoorList.Remove(_node);

                LinkedListNode<DDoor> _rmDoor = room.d_DoorList.First;
                for (; _rmDoor != null;) {
                    if (_rmDoor.Value.d_ptGlobalPos.Equals(_node.Value.d_ptGlobalPos)) {
                        //door coincided. Form a closed door
                        m_ClosedDoorList.AddLast(_rmDoor.Value);
                        room.d_DoorList.Remove(_rmDoor);
                        break;
                    }
                    _rmDoor = _rmDoor.Next;
                }
            }
            _node = _tempNode;
        }

        //add new doors to opening door list
        for (LinkedListNode<DDoor> _rmDoor = room.d_DoorList.First; _rmDoor != null;) {
            bool _flag = false;
            foreach (DSavedRoom _room in m_RoomList) {
                if (CheckDoorWithRoom(_room.d_Rect, _rmDoor.Value.d_ptGlobalPos)) {
                //if (_room.d_Rect.Contains(_rmDoor.Value.d_ptGlobalPos.ToVec2())) {
                    _flag = true;
                    break;
                }
            }
            if (!_flag) {
                m_OpeningDoorList.AddLast(_rmDoor.Value);
            }
            _rmDoor = _rmDoor.Next;
        }

        //Add the room to roomlist
        m_RoomList.AddLast(new DSavedRoom(_newRoom, room.d_ID));
    }

    private static bool AddNewRoom()
    {
        //Random pick a new **non-start** room,todo
        DRoom _room = RoomPresetsManager.GetInstance().RandomPicking().GetRoomInfo();
        //DRoom _room = RoomPresetsManager.GetInstance().GetPreset(0).GetRoomInfo();

        foreach (DDoor _opendoor in m_OpeningDoorList) {
            foreach (DDoor _door in _room.d_DoorList) {
                if (_door.CheckPair(_opendoor)) {
                    if (CheckRoom(_room, _door, _opendoor.d_ptGlobalPos)) {

                        RegisterNewRoom(_room, _door, _opendoor.d_ptGlobalPos);

                        return true;
                    }
                }
            }
        }

        return false;
    }

    private static void Clear()
    {
        m_RoomList.Clear();
        m_OpeningDoorList.Clear();
        m_ClosedDoorList.Clear();
    }

    private static Rect GetMapRect()
    {
        float _xMin, _xMax, _yMin, _yMax;
        _xMax = _xMin = _yMax = _yMin = 0.0f;
        foreach(DSavedRoom _room in m_RoomList) {
            Rect _tmp = _room.d_Rect;
            if (_tmp.xMin < _xMin)
                _xMin = _tmp.xMin;
            if (_tmp.xMax > _xMax)
                _xMax = _tmp.xMax;
            if (_tmp.yMin < _yMin)
                _yMin = _tmp.yMin;
            if (_tmp.yMax > _yMax)
                _yMax = _tmp.yMax;
        }

        return new Rect(_xMin, _yMin, _xMax - _xMin + 1, _yMax - _yMin + 1);
    }

    //todo
    public static Stage NewMap(int times)
    {
        Clear();
        //Random pick a **start** room and register it;
        DRoom _room = RoomPresetsManager.GetInstance().RandomPicking().GetRoomInfo();
        //DRoom _room = RoomPresetsManager.GetInstance().GetPreset(0).GetRoomInfo();
        RegisterNewRoom(_room, _room.d_DoorList.First.Value, new DPointN(0, 0));

        for(int i=0; i<times; i++) {
            AddNewRoom();
            if (m_OpeningDoorList.Count == 0) {
                Debug.Log(i);
                break;
            }
        }


        //make map
        
        Rect _rct = GetMapRect();
        Stage _stage = new Stage((int)_rct.width, (int)_rct.height);
        foreach(DSavedRoom _svroom in m_RoomList) {
            DRoomPreset _preset = RoomPresetsManager.GetInstance().GetPreset(_svroom.d_ID);
            if (_preset == null) continue;

            int _x = (int)_svroom.d_Rect.x - (int)_rct.x;
            int _y = (int)_svroom.d_Rect.y - (int)_rct.y;
            int _w = (int)_svroom.d_Rect.width+1;
            int _h = (int)_svroom.d_Rect.height+1;

            for(int i=0; i<_w; i++) {
                for(int j=0; j<_h; j++) {
                    int _mapPos = (_x + i) + (_y + j) * (int)_rct.width;
                    int _presetPos = i + j * _w;

                    DCell _cell = _preset.d_Data[_presetPos];
                    if (_cell.d_bIsDoor)
                        _stage.GetMap()[_mapPos] = new DCell(0, false, true, false);
                    else
                        _stage.GetMap()[_mapPos] = _cell;
                }
            }
        }
        //Add ClosedDoors to data
        foreach(DDoor _door in m_ClosedDoorList) {
            int _x = _door.d_ptGlobalPos.x - (int)_rct.x;
            int _y = _door.d_ptGlobalPos.y - (int)_rct.y;
            int _mapPos = _x + _y * (int)_rct.width;
            _stage.GetMap()[_mapPos] = new DCell(0, true, false, false);//get data from DDoor?todo
        }

        return _stage;
    }
}