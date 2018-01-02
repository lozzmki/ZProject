using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DSceneObject
{
    public GameObject d_Prefab;
    public int x;
    public int y;
    public int d_Data;
    public DSceneObject(GameObject prefab=null, int posX=0, int posY = 0, int data=0)
    {
        d_Prefab = prefab;
        x = posX;
        y = posY;
        d_Data = data;
    }
}

public class DRoomPreset
{
    private int m_nID;
    private bool m_bFormatted = false;
    private DRoom d_RoomInfo;
    public DSizeN d_Size;
    public DCell[] d_Data;
    public List<DSceneObject> d_Objects;

    public DRoomPreset(int width, int height)
    {
        d_Size = new DSizeN(width, height);
        d_Data = new DCell[width * height];
        d_Objects = new List<DSceneObject>();
    }
    public DRoomPreset(){}
    
    public DRoom GetRoomInfo()
    {
        if (!m_bFormatted)
            Format();
        return new DRoom(d_RoomInfo);
    }

    public void SetID(int val)
    {
        m_nID = val;
    }

    private void Format()
    {
        d_RoomInfo = new DRoom();
        m_bFormatted = true;
        d_RoomInfo.d_Size = d_Size;
        d_RoomInfo.d_ID = m_nID;

        //0,0 locates in the left bottom corner.
        for(int i=1; i<d_Size.d_nWidth-1; i++) {
            int _tmp = i;
            if (d_Data[_tmp].d_bIsDoor) {
                d_RoomInfo.d_DoorList.AddLast(new DDoor(i, 0, DDoor.BOTTOM));
            }
            _tmp = i + (d_Size.d_nHeight - 1) * d_Size.d_nWidth;
            if (d_Data[_tmp].d_bIsDoor) {
                d_RoomInfo.d_DoorList.AddLast(new DDoor(i, d_Size.d_nHeight - 1, DDoor.TOP));
            }
        }
        for(int i=1; i<d_Size.d_nHeight-1; i++) {
            int _tmp = i * d_Size.d_nWidth;
            if (d_Data[_tmp].d_bIsDoor) {
                d_RoomInfo.d_DoorList.AddLast(new DDoor(0, i, DDoor.LEFT));
            }
            _tmp = i * d_Size.d_nWidth + d_Size.d_nWidth - 1;
            if (d_Data[_tmp].d_bIsDoor) {
                d_RoomInfo.d_DoorList.AddLast(new DDoor(d_Size.d_nWidth - 1, i, DDoor.RIGHT));
            }
        }
    }
}

public class RoomPresetsManager {
    private Dictionary<int, DRoomPreset> m_Presets;
    private int m_nNextID = 0;

    private static RoomPresetsManager _start_inst;
    private static RoomPresetsManager _normal_inst;
    private static RoomPresetsManager _treasure_inst;
    private static RoomPresetsManager _boss_inst;

    public static RoomPresetsManager GetInstance(int i=0)
    {
        switch (i) {
            case 0:
                if (_start_inst == null)
                    _start_inst = new RoomPresetsManager();
                return _start_inst;
            case 1:
                if (_normal_inst == null)
                    _normal_inst = new RoomPresetsManager();
                return _normal_inst;
            case 2:
                if (_treasure_inst == null)
                    _treasure_inst = new RoomPresetsManager();
                return _treasure_inst;
            default:
                if (_boss_inst == null)
                    _boss_inst = new RoomPresetsManager();
                return _boss_inst;
        }
    }

    private RoomPresetsManager()
    {
        m_Presets = new Dictionary<int, DRoomPreset>();
    }

    public void AddPreset(DRoomPreset preset)
    {
        m_Presets.Add(m_nNextID, preset);
        preset.SetID(m_nNextID++);
    }

    public DRoomPreset GetPreset(int ID)
    {
        if (m_Presets.ContainsKey(ID))
            return m_Presets[ID];
        else
            return null;
    }

    public DRoomPreset RandomPicking() {
        int _tmp = Random.Range(0, m_Presets.Count);
        return m_Presets[_tmp];
    }

    //private static GameObject _s_Spawner;
    //private static GameObject _s_Treasure;
    //private static GameObject GetSpawner()
    //{
    //    if (_s_Spawner == null)
    //        _s_Spawner = Resources.Load<GameObject>("Prefabs/SceneObjects/MobSpawner");
    //    return _s_Spawner;
    //}
    //private static GameObject GetTreasure()
    //{
    //    if (_s_Treasure == null)
    //        _s_Treasure = Resources.Load<GameObject>("Prefabs/SceneObjects/TreasureSpawner");
    //    return _s_Treasure;
    //}

    public static void LoadPreset(string file)
    {
        const string _sFilePrePath = "Assets/Data/RoomPresets/";
        FileStream _fs = new FileStream(_sFilePrePath + file, FileMode.Open, FileAccess.Read);
        BinaryReader _is = new BinaryReader(_fs);

        ushort _u16Type = _is.ReadUInt16();
        RoomPresetsManager _mgr = GetInstance(_u16Type);

        ushort _uw = _is.ReadUInt16();
        ushort _uh = _is.ReadUInt16();
        DRoomPreset _preset = new DRoomPreset(_uw, _uh);

        for(int j=0; j<_uh; j++)
            for(int i=0; i<_uw; i++){
                int _pos = i + j * _uw;

                ushort _u16 = _is.ReadUInt16();
                _preset.d_Data[_pos] = new DCell(0, (_u16&0x2000)!=0, (_u16&0x4000)!=0, (_u16&0x8000)!=0,(RoomType)_u16Type);

            }

        //read SceneObjects
        int _objNum = _is.ReadInt32();
        for(uint u = 0; u<_objNum; u++) {
            int _hash = _is.ReadInt32();
            int _x = _is.ReadUInt16();
            int _y = _is.ReadUInt16();
            int _data = _is.ReadInt32();

            _preset.d_Objects.Add(new DSceneObject(ObjectManager.Get(_hash), _x, _y, _data));

        }

        _mgr.AddPreset(_preset);
    }

}
