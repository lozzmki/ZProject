using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[SerializeField]
public class RoomGenerateTool : EditorWindow
{
    

    public int m_nWidth;
    public int m_nHeight;
    public int m_nTimes = 1;
    public RoomType m_nRoomType;

    public int m_nNorth;
    public int m_nWest;
    public int m_nSouth;
    public int m_nEast;

    public int m_nArg;

    public static System.UInt32 ms_nSerial = 0;

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Width", EditorStyles.boldLabel);
        m_nWidth = EditorGUILayout.IntField(m_nWidth);
        GUILayout.Label("Height", EditorStyles.boldLabel);
        m_nHeight = EditorGUILayout.IntField(m_nHeight);
        GUILayout.EndHorizontal();

        GUILayout.Label("Doors", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.Label("North", EditorStyles.boldLabel);
        m_nNorth = EditorGUILayout.IntField(m_nNorth);
        GUILayout.Label("West", EditorStyles.boldLabel);
        m_nWest = EditorGUILayout.IntField(m_nWest);
        GUILayout.Label("South", EditorStyles.boldLabel);
        m_nSouth = EditorGUILayout.IntField(m_nSouth);
        GUILayout.Label("East", EditorStyles.boldLabel);
        m_nEast = EditorGUILayout.IntField(m_nEast);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("RoomType", EditorStyles.boldLabel);
        m_nRoomType = (RoomType)EditorGUILayout.EnumPopup(m_nRoomType);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        switch (m_nRoomType) {
            case RoomType.ROOM_NORMAL:
                GUILayout.Label("MobSpawnerNum", EditorStyles.boldLabel);
                m_nArg = EditorGUILayout.IntField(m_nArg);
                break;
            case RoomType.ROOM_TREASURE:
                GUILayout.Label("TreasureNum", EditorStyles.boldLabel);
                m_nArg = EditorGUILayout.IntField(m_nArg);
                break;
            default:
                break;
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Times", EditorStyles.boldLabel);
        m_nTimes = EditorGUILayout.IntField(m_nTimes);
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Randomise")) {
            Randomise();
        }

        if (GUILayout.Button("Generate")) {
            for(int i=0; i<m_nTimes; i++)
                OnGenerate();
        }
    }

    private void Randomise()
    {
        m_nWidth = Random.Range(8, 21);
        m_nHeight = Random.Range(8, 21);

        m_nNorth = Random.Range(0, 3);
        m_nWest = Random.Range(0, 3);
        m_nEast = Random.Range(0, 3);
        m_nSouth = Random.Range(0, 3);

        //m_nRoomType = (RoomType)Random.Range(0, 4);

        switch (m_nRoomType) {
            case RoomType.ROOM_NORMAL:
                m_nArg = Random.Range(1, 9);
                break;
            case RoomType.ROOM_TREASURE:
                m_nArg = Random.Range(1, 3);
                break;
            default:
                break;
        }
    }

    struct SceneObject
    {
        public int d_Hash;
        public ushort d_x;
        public ushort d_y;
        public int d_Data;
        public SceneObject(int hash, ushort x, ushort y, int data)
        {
            d_Hash = hash;
            d_x = x;
            d_y = y;
            d_Data = data;
        }
    }
    private void OnGenerate()
    {
        int[] _data = new int[m_nWidth * m_nHeight];
        List<SceneObject> _objList = new List<SceneObject>();

        //walls
        for (int i=1; i<m_nWidth-1; i++) {
            _data[i] = (int)CellType.CELL_WALL;
            _data[i + (m_nHeight - 1) * m_nWidth] = (int)CellType.CELL_WALL;
        }
        for(int i=1; i<m_nHeight-1; i++) {
            _data[i * m_nWidth] = (int)CellType.CELL_WALL;
            _data[(i + 1) * m_nWidth - 1] = (int)CellType.CELL_WALL;
        }
        //joint
        _data[0] = (int)CellType.CELL_JOINT;
        _data[m_nWidth - 1] = (int)CellType.CELL_JOINT;
        _data[m_nWidth * (m_nHeight - 1)] = (int)CellType.CELL_JOINT;
        _data[m_nWidth * m_nHeight - 1] = (int)CellType.CELL_JOINT;

        //doors
        int[] _w = new int[m_nWidth - 2];
        for (int i = 1; i < m_nWidth - 1; i++)
            _w[i-1] = i;
        int[] _h = new int[m_nHeight - 2];
        for (int i = 1; i < m_nHeight - 1; i++)
            _h[i-1] = i;
        //north
        SmallKits.Shuffle(ref _w);
        for (int i=0; i<m_nWidth - 2&& i<m_nNorth; i++) {
            _data[_w[i]] = (int)CellType.CELL_DOOR;
        }
        //south
        SmallKits.Shuffle(ref _w);
        for (int i = 0; i < m_nWidth - 2 && i < m_nSouth; i++) {
            _data[_w[i]+(m_nHeight - 1)*m_nWidth] = (int)CellType.CELL_DOOR;
        }
        //west
        SmallKits.Shuffle(ref _h);
        for(int i=0; i<m_nHeight-2 && i<m_nWest; i++) {
            _data[_h[i] * m_nWidth] = (int)CellType.CELL_DOOR;
        }
        //east
        SmallKits.Shuffle(ref _h);
        for (int i = 0; i < m_nHeight - 2 && i < m_nEast; i++) {
            _data[(_h[i] + 1) * m_nWidth - 1] = (int)CellType.CELL_DOOR;
        }


        //Objects
        int[] _tmp = new int[(m_nWidth - 2) * (m_nHeight - 2)];
        for(int j=0; j<m_nHeight-2; j++)
            for(int i=0; i< m_nWidth - 2; i++) {
                _tmp[i + j * (m_nWidth - 2)] = i + 1 + (j + 1) * m_nWidth;
            }
        switch (m_nRoomType) {
            case RoomType.ROOM_NORMAL:
                SmallKits.Shuffle(ref _tmp);
                for(int i=0; i<m_nArg && i<_tmp.Length; i++) {
                    //_data[_tmp[i]] = (int)CellType.CELL_MOBSPAWNER;
                    int _x = _tmp[i] % m_nWidth;
                    int _y = _tmp[i] / m_nWidth;

                    _objList.Add(new SceneObject("MobSpawner".GetHashCode(), (ushort)_x, (ushort)_y, "Mob".GetHashCode()));
                }
                break;
            case RoomType.ROOM_TREASURE:
                SmallKits.Shuffle(ref _tmp);
                for (int i = 0; i < m_nArg && i < _tmp.Length; i++) {
                    //_data[_tmp[i]] = (int)CellType.CELL_TREASURE;
                    int _x = _tmp[i] % m_nWidth;
                    int _y = _tmp[i] / m_nWidth;

                    _objList.Add(new SceneObject("TreasureSpawner".GetHashCode(), (ushort)_x, (ushort)_y, 0));
                }
                _objList.Add(new SceneObject("Shop".GetHashCode(), (ushort)(m_nWidth / 2), (ushort)(m_nHeight / 2), 0));
                break;
            case RoomType.ROOM_BOSS:
                //_data[Random.Range(1, m_nWidth) + Random.Range(1, m_nHeight) * m_nWidth] = (int)CellType.CELL_MOBSPAWNER;
                _objList.Add(new SceneObject("MobSpawner".GetHashCode(), (ushort)Random.Range(1, m_nWidth), (ushort)Random.Range(1, m_nHeight), "Boss".GetHashCode()));
                break;
            default:
                break;
        }

        //Generate File
        FileStream _fs;
        const string _sFilePrePath = "Assets/Data/RoomPresets";
        if (!Directory.Exists(_sFilePrePath))
            Directory.CreateDirectory(_sFilePrePath);
        while(File.Exists(_sFilePrePath +"/"+ SmallKits.FixName(ms_nSerial) + ".rmp")) {
            ++ms_nSerial;
        }
        _fs = new FileStream(_sFilePrePath + "/" + SmallKits.FixName(ms_nSerial) + ".rmp", FileMode.Create, FileAccess.Write);
        BinaryWriter _os = new BinaryWriter(_fs);

        _os.Write((System.UInt16)m_nRoomType);
        _os.Write((System.UInt16)m_nWidth);
        _os.Write((System.UInt16)m_nHeight);

        
        for(int j=0; j<m_nHeight; j++) {
            for(int i=0; i<m_nWidth; i++) {
                int _pos = i + j * m_nWidth;
                ushort _u16 = 0;
                switch ((CellType)_data[_pos]) {
                    case CellType.CELL_SPACE:
                        break;
                    case CellType.CELL_JOINT:
                        _u16 = 0x8000 | 0x4000;
                        break;
                    case CellType.CELL_WALL:
                        _u16 = 0x4000;
                        break;
                    case CellType.CELL_DOOR:
                        _u16 = 0x2000;
                        break;
                    //case CellType.CELL_MOBSPAWNER:
                    //    //TODO:temp here:
                    //    _objList.Add(new SceneObject("MobSpawner".GetHashCode(), (ushort)i, (ushort)j, "Mob".GetHashCode()));
                    //    break;
                    //case CellType.CELL_TREASURE:
                    //    _objList.Add(new SceneObject("TreasureSpawner".GetHashCode(), (ushort)i, (ushort)j, 0));
                    //    break;
                    default:
                        break;
                }
                _u16 |= 0x0;//preset data for other use;
                _os.Write(_u16);
            }
        }

        _os.Write(_objList.Count);

        for(int i=0; i<_objList.Count; i++) {
            _os.Write(_objList[i].d_Hash);
            _os.Write(_objList[i].d_x);
            _os.Write(_objList[i].d_y);
            _os.Write(_objList[i].d_Data);
        }

        //add other data here
        _os.Close();
        _fs.Close();
    }
}

public class ToolKit
{
    [MenuItem("ToolKit/RoomGenerateTool")]
    public static void S_MapGenerateToolOpen()
    {
        RoomGenerateTool _tool = (RoomGenerateTool)EditorWindow.GetWindow(typeof(RoomGenerateTool));
        _tool.Show();
    }
}