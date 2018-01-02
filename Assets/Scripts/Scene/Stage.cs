using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    ROOM_START,
    ROOM_NORMAL,
    ROOM_TREASURE,
    ROOM_BOSS
}

public enum CellType
{
    CELL_SPACE,
    CELL_JOINT,
    CELL_WALL,
    CELL_DOOR,
}

//one tile
public class DCell
{
    public const float CELL_BORDER_LENGTH = 2.0f;

    public int d_nTex;
    public bool d_bIsDoor;
    public bool d_bIsWall;
    public bool d_bIsJoint;

    public RoomType d_nType;

    public bool IsFloor
    {
        get
        {
            return !d_bIsDoor && !d_bIsWall && !d_bIsJoint;
        }
    }
    public DCell(int texture = 0, bool isDoor = false, bool isWall = false,bool isJoint = false, RoomType nType = RoomType.ROOM_NORMAL)
    {
        d_nTex = texture;
        d_bIsDoor = isDoor;
        d_bIsWall = isWall;
        d_bIsJoint = isJoint;
        d_nType = nType;
        //d_bPathable = ifPathable;
    }

}
/// <summary>
/// 场景地图信息
/// </summary>
public class Stage {
    public DSizeN m_Size;
    private DCell[] m_Data;
    public DPointN m_FirstJoint;
    public Vector3 m_StartPoint;
    public List<DSceneObject> m_Objects;
    public GameObject _stage;

    public Stage(int width, int height)
    {
        m_Size = new DSizeN(width, height);
        m_Data = new DCell[width * height];
        m_Objects = new List<DSceneObject>();
    }

    public DCell[] GetMap()
    {
        return m_Data;
    }

    public Vector2 playerPos
    {
        get
        {
            if (null == GameManager.Player)
                return Vector2.zero;
            else {
                Vector2 _pos = new Vector2((GameManager.Player.transform.position.x - _stage.transform.position.x) / DCell.CELL_BORDER_LENGTH / m_Size.d_nWidth,
                                            (GameManager.Player.transform.position.z - _stage.transform.position.z) / DCell.CELL_BORDER_LENGTH / m_Size.d_nHeight);
                if (_pos.x < 0.0f)
                    _pos.x = 0.0f;
                if (_pos.x > 1.0f)
                    _pos.x = 1.0f;
                if (_pos.y < 0.0f)
                    _pos.y = 0.0f;
                if (_pos.y > 1.0f)
                    _pos.y = 1.0f;

                return _pos;
            }
        }
    }

    private Texture2D m_BaseMinimap;
    private Texture2D m_CurrentMinimap;
    private Vector2 m_LastSpot = Vector2.zero;
    public Texture2D minimapTex
    {
        get
        {
            if(m_BaseMinimap == null) {
                Color _boss = new Color(0.5f, 0, 0);
                Color _treasure = new Color(0.5f, 0.5f, 0);
                Color _normal = new Color(0, 0, 0.5f);
                
                m_BaseMinimap = new Texture2D(m_Size.d_nWidth, m_Size.d_nHeight);
                for (int j = 0; j< m_Size.d_nHeight; j++) {
                    for(int i=0; i<m_Size.d_nWidth; i++) {
                        DCell _cell = m_Data[i + j * m_Size.d_nWidth];
                        if (_cell == null)
                            m_BaseMinimap.SetPixel(i, j, Color.black);
                        else {
                            if(_cell.d_bIsDoor)
                                m_BaseMinimap.SetPixel(i, j, Color.yellow);
                            else if(_cell.d_bIsJoint || _cell.d_bIsWall)
                                m_BaseMinimap.SetPixel(i, j, Color.cyan);
                            else {
                                //room tiles
                                
                                switch (_cell.d_nType) {
                                    case RoomType.ROOM_START:
                                        m_BaseMinimap.SetPixel(i, j, Color.gray);
                                        break;
                                    case RoomType.ROOM_NORMAL:
                                        m_BaseMinimap.SetPixel(i, j, _normal);
                                        break;
                                    case RoomType.ROOM_TREASURE:
                                        m_BaseMinimap.SetPixel(i, j, _treasure);
                                        break;
                                    case RoomType.ROOM_BOSS:
                                        m_BaseMinimap.SetPixel(i, j, _boss);
                                        break;
                                    default:
                                        break;
                                }
                            }
                                
                        }
                    }
                }
                m_BaseMinimap.Apply();
            }
            if(m_CurrentMinimap == null) {
                m_CurrentMinimap = Object.Instantiate(m_BaseMinimap);
            }
            if(null != GameManager.Player) {
                int _x = System.Convert.ToInt32( (GameManager.Player.transform.position.x - _stage.transform.position.x) / DCell.CELL_BORDER_LENGTH);
                int _y = System.Convert.ToInt32( (GameManager.Player.transform.position.z - _stage.transform.position.z) / DCell.CELL_BORDER_LENGTH);

                if (_x < m_Size.d_nWidth && _y < m_Size.d_nHeight) {
                    m_CurrentMinimap.SetPixel((int)m_LastSpot.x, (int)m_LastSpot.y, m_BaseMinimap.GetPixel((int)m_LastSpot.x, (int)m_LastSpot.y));
                    m_CurrentMinimap.SetPixel(_x, _y, Color.green);
                    m_LastSpot.x = _x;
                    m_LastSpot.y = _y;
                    m_CurrentMinimap.Apply();
                }
            }

            return m_CurrentMinimap;
            
        }
    }

    public void PressCell(int pos, DCell newCell)
    {
        if (    (m_Data[pos] == null) || 
                (!m_Data[pos].d_bIsDoor && newCell.d_bIsDoor) ||
                (!m_Data[pos].d_bIsJoint && newCell.d_bIsJoint) ||
                (!m_Data[pos].d_bIsWall && newCell.d_bIsWall)) 
        {
            m_Data[pos] = newCell;
        }
    }

    private DCell GetCell(DPointN point)
    {
        return m_Data[point.x + point.y * m_Size.d_nWidth];
    }
    private bool CheckPoint(DPointN point)
    {
        return point.x >= 0 && point.x < m_Size.d_nWidth && point.y >= 0 && point.y < m_Size.d_nHeight;
    }

    private static Material _s_WallMat;
    private Material GetWallMat()
    {
        if(_s_WallMat == null) {
            _s_WallMat = Resources.Load<Material>("Materials/wall");
        }
        return _s_WallMat;
    }
    private void CreateWall(GameObject stage, DPointN start, DPointN end)
    {
        //GameObject _wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //float _centerX = (float)(start.x + end.x) / 2.0f;
        //float _centerY = (float)(start.y + end.y) / 2.0f;
        //float _scaleX = Mathf.Abs(start.x - end.x) + 1;
        //float _scaleY = Mathf.Abs(start.y - end.y) + 1;
        const float _fHeight = 4.0f;
        //_wall.transform.position = new Vector3(_centerX* DCell.CELL_BORDER_LENGTH, _fHeight * DCell.CELL_BORDER_LENGTH / 2.0f, _centerY* DCell.CELL_BORDER_LENGTH);
        //_wall.transform.localScale = new Vector3(_scaleX* DCell.CELL_BORDER_LENGTH, _fHeight * DCell.CELL_BORDER_LENGTH, _scaleY* DCell.CELL_BORDER_LENGTH);
        //_wall.transform.parent = stage.transform;
        //_wall.GetComponent<MeshRenderer>().material = GetWallMat();
        //_wall.tag = "SceneObj";
        //_wall.layer = 9;

        GameObject _wall = new GameObject("wall");
        float _centerX = (start.x + end.x) / 2.0f;
        float _centerY = (start.y + end.y) / 2.0f;
        float _sizeX = Mathf.Abs(start.x - end.x) + 1;
        float _sizeY = Mathf.Abs(start.y - end.y) + 1;

        int _up = Mathf.Max(start.y, end.y);
        int _down = Mathf.Min(start.y, end.y);
        int _right = Mathf.Max(start.x, end.x);
        int _left = Mathf.Min(start.x, end.x);

        DCell _cell = null;
        GameObject _wallTex = null;

        Vector3[] _vertices = new Vector3[4];
        Vector2[] _uvs = new Vector2[4];
        int[] _tr = new int[6] { 0, 1, 2, 0, 2, 3 };
        Mesh _mesh;

        //CreateMesh
        #region Up
        if (_up < m_Size.d_nHeight - 1) {
            _cell = GetCell(new DPointN(_left, _up + 1));
            if(!(_cell == null || _cell.d_bIsWall)) {
                _wallTex = new GameObject("walltex");
                _wallTex.transform.parent = _wall.transform;
                _mesh = _wallTex.AddComponent<MeshFilter>().mesh;
                _wallTex.AddComponent<MeshRenderer>().material = GetWallMat();

                _vertices[0] = new Vector3(_sizeX, -_fHeight, _sizeY)   * DCell.CELL_BORDER_LENGTH/2.0F;
                _vertices[1] = new Vector3(_sizeX, _fHeight, _sizeY)    * DCell.CELL_BORDER_LENGTH/2.0F;
                _vertices[2] = new Vector3(-_sizeX, _fHeight, _sizeY)   * DCell.CELL_BORDER_LENGTH/2.0F;
                _vertices[3] = new Vector3(-_sizeX, -_fHeight, _sizeY)  * DCell.CELL_BORDER_LENGTH/2.0F;

                _uvs[0] = new Vector2(0.0f, 0.0f) * DCell.CELL_BORDER_LENGTH;
                _uvs[1] = new Vector2(0.0f, _fHeight) * DCell.CELL_BORDER_LENGTH;
                _uvs[2] = new Vector2(_sizeX, _fHeight) * DCell.CELL_BORDER_LENGTH;
                _uvs[3] = new Vector2(_sizeX, 0.0f) * DCell.CELL_BORDER_LENGTH;

                _mesh.vertices = _vertices;
                _mesh.uv = _uvs;
                _mesh.triangles = _tr;
            }
        }
        #endregion
        #region Down
        if (_down > 0) {
            _cell = GetCell(new DPointN(_left, _down - 1));
            if (!(_cell == null || _cell.d_bIsWall)) {
                _wallTex = new GameObject("walltex");
                _wallTex.transform.parent = _wall.transform;
                _mesh = _wallTex.AddComponent<MeshFilter>().mesh;
                _wallTex.AddComponent<MeshRenderer>().material = GetWallMat();

                _vertices[0] = new Vector3(-_sizeX, -_fHeight, -_sizeY) * DCell.CELL_BORDER_LENGTH / 2.0F;
                _vertices[1] = new Vector3(-_sizeX, _fHeight, -_sizeY) * DCell.CELL_BORDER_LENGTH / 2.0F;
                _vertices[2] = new Vector3(_sizeX, _fHeight, -_sizeY) * DCell.CELL_BORDER_LENGTH / 2.0F;
                _vertices[3] = new Vector3(_sizeX, -_fHeight, -_sizeY) * DCell.CELL_BORDER_LENGTH / 2.0F;

                _uvs[0] = new Vector2(0.0f, 0.0f) * DCell.CELL_BORDER_LENGTH;
                _uvs[1] = new Vector2(0.0f, _fHeight) * DCell.CELL_BORDER_LENGTH;
                _uvs[2] = new Vector2(_sizeX, _fHeight) * DCell.CELL_BORDER_LENGTH;
                _uvs[3] = new Vector2(_sizeX, 0.0f) * DCell.CELL_BORDER_LENGTH;

                _mesh.vertices = _vertices;
                _mesh.uv = _uvs;
                _mesh.triangles = _tr;
            }
        }
        #endregion
        #region Left
        if (_left > 0) {
            _cell = GetCell(new DPointN(_left - 1, _up));
            if (!(_cell == null || _cell.d_bIsWall)) {
                _wallTex = new GameObject("walltex");
                _wallTex.transform.parent = _wall.transform;
                _mesh = _wallTex.AddComponent<MeshFilter>().mesh;
                _wallTex.AddComponent<MeshRenderer>().material = GetWallMat();

                _vertices[0] = new Vector3(-_sizeX, -_fHeight, _sizeY) * DCell.CELL_BORDER_LENGTH / 2.0F;
                _vertices[1] = new Vector3(-_sizeX, _fHeight, _sizeY) * DCell.CELL_BORDER_LENGTH / 2.0F;
                _vertices[2] = new Vector3(-_sizeX, _fHeight, -_sizeY) * DCell.CELL_BORDER_LENGTH / 2.0F;
                _vertices[3] = new Vector3(-_sizeX, -_fHeight, -_sizeY) * DCell.CELL_BORDER_LENGTH / 2.0F;

                _uvs[0] = new Vector2(0.0f, 0.0f) * DCell.CELL_BORDER_LENGTH;
                _uvs[1] = new Vector2(0.0f, _fHeight) * DCell.CELL_BORDER_LENGTH;
                _uvs[2] = new Vector2(_sizeY, _fHeight) * DCell.CELL_BORDER_LENGTH;
                _uvs[3] = new Vector2(_sizeY, 0.0f) * DCell.CELL_BORDER_LENGTH;

                _mesh.vertices = _vertices;
                _mesh.uv = _uvs;
                _mesh.triangles = _tr;
            }
        }
        #endregion
        #region Right
        if (_right <m_Size.d_nWidth-1) {
            _cell = GetCell(new DPointN(_right + 1, _up));
            if (!(_cell == null || _cell.d_bIsWall)) {
                _wallTex = new GameObject("walltex");
                _wallTex.transform.parent = _wall.transform;
                _mesh = _wallTex.AddComponent<MeshFilter>().mesh;
                _wallTex.AddComponent<MeshRenderer>().material = GetWallMat();

                _vertices[0] = new Vector3(_sizeX, -_fHeight, -_sizeY) * DCell.CELL_BORDER_LENGTH / 2.0F;
                _vertices[1] = new Vector3(_sizeX, _fHeight, -_sizeY) * DCell.CELL_BORDER_LENGTH / 2.0F;
                _vertices[2] = new Vector3(_sizeX, _fHeight, _sizeY) * DCell.CELL_BORDER_LENGTH / 2.0F;
                _vertices[3] = new Vector3(_sizeX, -_fHeight, _sizeY) * DCell.CELL_BORDER_LENGTH / 2.0F;

                _uvs[0] = new Vector2(0.0f, 0.0f) * DCell.CELL_BORDER_LENGTH;
                _uvs[1] = new Vector2(0.0f, _fHeight) * DCell.CELL_BORDER_LENGTH;
                _uvs[2] = new Vector2(_sizeY, _fHeight) * DCell.CELL_BORDER_LENGTH;
                _uvs[3] = new Vector2(_sizeY, 0.0f) * DCell.CELL_BORDER_LENGTH;

                _mesh.vertices = _vertices;
                _mesh.uv = _uvs;
                _mesh.triangles = _tr;
            }
        }
        #endregion

        //Collider
        _wall.AddComponent<BoxCollider>().size = new Vector3(_sizeX, _fHeight, _sizeY) * DCell.CELL_BORDER_LENGTH;

        _wall.transform.localPosition = new Vector3(_centerX * DCell.CELL_BORDER_LENGTH, _fHeight * DCell.CELL_BORDER_LENGTH / 2.0f, _centerY * DCell.CELL_BORDER_LENGTH);
        _wall.transform.parent = stage.transform;
        _wall.tag = "SceneObj";
        _wall.layer = 9;
    }
    private static Material _s_floorMat;
    private static Material GetFloorMat()
    {
        if (_s_floorMat == null)
            _s_floorMat = Resources.Load<Material>("Materials/floor");
        return _s_floorMat;
    }

    private bool _test = true;
    private GameObject CreateFloor(int texture = 0)
    {
        GameObject _floor = new GameObject("tile");
        Mesh _mesh = _floor.AddComponent<MeshFilter>().mesh;
        _floor.AddComponent<MeshRenderer>().material = GetFloorMat();

        //TODO:texture here
        //if (_test)
        //    _floor.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture>("bg"));
        //else
        //    _floor.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture>("flare"));
        _floor.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture>("Textures/factory_floor"));
        _test = !_test;

        Vector3[] _vertices = new Vector3[4];
        Vector2[] _uvs = new Vector2[4];
        int[] _triangles = new int[6] { 0, 1, 2, 0, 2, 3 };

        _vertices[0] = new Vector3(-0.5f, 0.0f, -0.5f);
        _vertices[1] = new Vector3(-0.5f, 0.0f, 0.5f);
        _vertices[2] = new Vector3(0.5f, 0.0f, 0.5f);
        _vertices[3] = new Vector3(0.5f, 0.0f, -0.5f);

        _uvs[0] = new Vector2(0.0f, 0.0f);
        _uvs[1] = new Vector2(0.0f, 1.0f);
        _uvs[2] = new Vector2(1.0f, 1.0f);
        _uvs[3] = new Vector2(1.0f, 0.0f);

        _mesh.vertices = _vertices;
        _mesh.uv = _uvs;
        _mesh.triangles = _triangles;

        _floor.layer = 9;
        //_floor.AddComponent<MeshCollider>().sharedMesh = _mesh;

        return _floor;
    }

    private GameObject CreateCeiling(Vector2 size)
    {
        GameObject _obj = new GameObject("ceiling");
        Mesh _mesh = _obj.AddComponent<MeshFilter>().mesh;
        _obj.AddComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/ceiling");

        Vector3[] _vertices = new Vector3[4];
        Vector2[] _uvs = new Vector2[4];
        int[] _triangles = new int[6] { 0, 1, 2, 0, 2, 3 };

        _vertices[0] = new Vector3(-0.5f, 0.0f, 0.5f);
        _vertices[1] = new Vector3(-0.5f, 0.0f, -0.5f);
        _vertices[2] = new Vector3(0.5f, 0.0f, -0.5f);
        _vertices[3] = new Vector3(0.5f, 0.0f, 0.5f);

        _uvs[0] = new Vector2(0.0f, 0.0f);
        _uvs[1] = new Vector2(0.0f, size.y);
        _uvs[2] = new Vector2(size.x, size.y);
        _uvs[3] = new Vector2(size.x, 0.0f);

        _mesh.vertices = _vertices;
        _mesh.uv = _uvs;
        _mesh.triangles = _triangles;
        _obj.AddComponent<MeshCollider>().sharedMesh = _mesh;

        _obj.layer = 9;

        return _obj;
    }

    public void CreateStage(Vector3 pos)
    {
        _stage = new GameObject("stage");
        _stage.transform.position = pos;
        //establish stage with data,todo
        Queue<DPointN> _Q = new Queue<DPointN>();
        HashSet<DPointN> _S = new HashSet<DPointN>();
        _Q.Enqueue(m_FirstJoint);

        while(_Q.Count > 0) {
            DPointN _joint = _Q.Dequeue();

            DPointN _start, _pointer;
            DCell _cell;
            //create walls
            //TODO:raw dirty code. needs clean and cut.
            #region -X
            _pointer = _start = new DPointN(_joint.x - 1, _joint.y);
            if(CheckPoint(_start)){
                _cell = GetCell(_start);
                if (_cell != null) {
                    if (_cell.d_bIsJoint || _cell.d_bIsDoor) {
                        if (!_S.Contains(_start)) {
                            _S.Add(_start);
                            _Q.Enqueue(_start);
                        }
                    }
                    else {
                        if (_cell.d_bIsWall) {
                            for (int i = _start.x; i >= 0; i--) {
                                DPointN _p = _start.OffsetX(i - _start.x);
                                if (!CheckPoint(_p) || GetCell(_p)==null) {
                                    break;
                                }
                                _cell = GetCell(_p);
                                if (_cell.d_bIsJoint || _cell.d_bIsDoor) {
                                    if (!_S.Contains(_p)) {
                                        _S.Add(_p);
                                        _Q.Enqueue(_p);
                                        CreateWall(_stage, _start, _pointer);
                                    }
                                    else {
                                        if (_Q.Contains(_p)) {
                                            CreateWall(_stage, _start, _pointer);
                                        }//else discard.
                                    }
                                    break;
                                }
                                else if (_cell.d_bIsWall) {
                                    _pointer = _p;
                                }
                                else {//not wall/joint/door
                                      //discard
                                    break;
                                }
                            }
                        }//else discard
                    }
                }
            }
            #endregion
            #region +X
            _pointer = _start = new DPointN(_joint.x + 1, _joint.y);
            if (CheckPoint(_start)) {
                _cell = GetCell(_start);
                if (_cell != null) {
                    if (_cell.d_bIsJoint || _cell.d_bIsDoor) {
                        if (!_S.Contains(_start)) {
                            _S.Add(_start);
                            _Q.Enqueue(_start);
                        }
                    }
                    else {
                        if (_cell.d_bIsWall) {
                            for (int i = _start.x; i < m_Size.d_nWidth; i++) {
                                DPointN _p = _start.OffsetX(i - _start.x);
                                if (!CheckPoint(_p) || GetCell(_p) == null) {
                                    break;
                                }
                                _cell = GetCell(_p);
                                if (_cell.d_bIsJoint || _cell.d_bIsDoor) {
                                    if (!_S.Contains(_p)) {
                                        _S.Add(_p);
                                        _Q.Enqueue(_p);
                                        CreateWall(_stage, _start, _pointer);
                                    }
                                    else {
                                        if (_Q.Contains(_p)) {
                                            CreateWall(_stage, _start, _pointer);
                                        }//else discard.
                                    }
                                    break;
                                }
                                else if (_cell.d_bIsDoor) {
                                    CreateWall(_stage, _start, _pointer);
                                    break;
                                }
                                else if (_cell.d_bIsWall) {
                                    _pointer = _p;
                                }
                                else {//not wall/joint/door
                                      //discard
                                    break;
                                }
                            }
                        }//else discard
                    }
                }
            }
            #endregion
            #region -Y
            _pointer = _start = new DPointN(_joint.x, _joint.y - 1);
            if (CheckPoint(_start)) {
                _cell = GetCell(_start);
                if (_cell != null) {
                    if (_cell.d_bIsJoint || _cell.d_bIsDoor) {
                        if (!_S.Contains(_start)) {
                            _S.Add(_start);
                            _Q.Enqueue(_start);
                        }
                    }
                    else {
                        if (_cell.d_bIsWall) {
                            for (int i = _start.y; i>=0; i--) {
                                DPointN _p = _start.OffsetY(i - _start.y);
                                if (!CheckPoint(_p) || GetCell(_p) == null) {
                                    break;
                                }
                                _cell = GetCell(_p);
                                if (_cell.d_bIsJoint || _cell.d_bIsDoor) {
                                    if (!_S.Contains(_p)) {
                                        _S.Add(_p);
                                        _Q.Enqueue(_p);
                                        CreateWall(_stage, _start, _pointer);
                                    }
                                    else {
                                        if (_Q.Contains(_p)) {
                                            CreateWall(_stage, _start, _pointer);
                                        }//else discard.
                                    }
                                    break;
                                }
                                else if (_cell.d_bIsDoor) {
                                    CreateWall(_stage, _start, _pointer);
                                    break;
                                }
                                else if (_cell.d_bIsWall) {
                                    _pointer = _p;
                                }
                                else {//not wall/joint/door
                                      //discard
                                    break;
                                }
                            }
                        }//else discard
                    }
                }
            }
            #endregion
            #region +Y
            _pointer = _start = new DPointN(_joint.x, _joint.y + 1);
            if (CheckPoint(_start)) {
                _cell = GetCell(_start);
                if (_cell != null) {
                    if (_cell.d_bIsJoint || _cell.d_bIsDoor) {
                        if (!_S.Contains(_start)) {
                            _S.Add(_start);
                            _Q.Enqueue(_start);
                        }
                    }
                    else {
                        if (_cell.d_bIsWall) {
                            for (int i = _start.y; i < m_Size.d_nHeight; i++) {
                                DPointN _p = _start.OffsetY(i - _start.y);
                                if (!CheckPoint(_p) || GetCell(_p) == null) {
                                    break;
                                }
                                _cell = GetCell(_p);
                                if (_cell.d_bIsJoint || _cell.d_bIsDoor) {
                                    if (!_S.Contains(_p)) {
                                        _S.Add(_p);
                                        _Q.Enqueue(_p);
                                        CreateWall(_stage, _start, _pointer);
                                    }
                                    else {
                                        if (_Q.Contains(_p)) {
                                            CreateWall(_stage, _start, _pointer);
                                        }//else discard.
                                    }
                                    break;
                                }
                                else if (_cell.d_bIsDoor) {
                                    CreateWall(_stage, _start, _pointer);
                                    break;
                                }
                                else if (_cell.d_bIsWall) {
                                    _pointer = _p;
                                }
                                else {//not wall/joint/door
                                      //discard
                                    break;
                                }
                            }
                        }//else discard
                    }
                }
            }
            #endregion

            //finally create the joint/door
            _cell = GetCell(_joint);
            float _fHeight = 4.0f;
            if (_cell.d_bIsJoint) {
                GameObject _jobj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                _jobj.GetComponent<MeshRenderer>().material = GetFloorMat();
                _jobj.GetComponent<MeshRenderer>().material.mainTexture = Resources.Load<Texture>("Textures/factory_joint");
                _jobj.transform.position = new Vector3(_joint.x * DCell.CELL_BORDER_LENGTH, _fHeight * DCell.CELL_BORDER_LENGTH / 2.0f, _joint.y * DCell.CELL_BORDER_LENGTH);
                _jobj.transform.localScale = new Vector3(1.0f, _fHeight, 1.0f) * DCell.CELL_BORDER_LENGTH;
                _jobj.transform.parent = _stage.transform;
                _jobj.tag = "SceneObj";
                _jobj.layer = 9;
            }
            else {
                GameObject _door = Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/SceneObjects/Door"));
                _door.transform.position = new Vector3(_joint.x * DCell.CELL_BORDER_LENGTH, _fHeight * DCell.CELL_BORDER_LENGTH / 2.0f, _joint.y * DCell.CELL_BORDER_LENGTH);

                float _x = 1.0f, _z=1.0f;
                if (GetCell(new DPointN(_joint.x - 1, _joint.y)).IsFloor)
                    _x = 0.8f;
                if (GetCell(new DPointN(_joint.x, _joint.y - 1)).IsFloor)
                    _z = 0.8f;

                _door.transform.localScale = _door.transform.localScale = new Vector3(_x, _fHeight, _z) * DCell.CELL_BORDER_LENGTH;
                _door.transform.parent = _stage.transform;
                _door.layer = 9;
            }
        }

        //place floor mesh
        for(int i=0; i<m_Size.d_nWidth; i++) {
            for(int j=0; j<m_Size.d_nHeight; j++) {
                DCell _tmp = m_Data[i + j * m_Size.d_nWidth];
                if(_tmp != null && !_tmp.d_bIsWall && !_tmp.d_bIsJoint) {
                    GameObject _obj = CreateFloor();
                    _obj.transform.localScale = new Vector3(DCell.CELL_BORDER_LENGTH, 1.0f, DCell.CELL_BORDER_LENGTH);
                    _obj.transform.position = new Vector3(i * DCell.CELL_BORDER_LENGTH, 0.0f, j * DCell.CELL_BORDER_LENGTH);
                    _obj.transform.parent = _stage.transform;
                }
            }
        }
        //place floor collider
        GameObject _floor = new GameObject("floor");
        _floor.layer = LayerMask.NameToLayer("SceneObjects");
        Mesh _mesh = _floor.AddComponent<MeshFilter>().mesh;
        Vector3[] _v = new Vector3[4];
        _v[0] = new Vector3(0.0f, 0.0f, 0.0f)*DCell.CELL_BORDER_LENGTH;
        _v[1] = new Vector3(m_Size.d_nWidth, 0.0f, 0.0f)*DCell.CELL_BORDER_LENGTH;
        _v[2] = new Vector3(m_Size.d_nWidth, 0.0f, m_Size.d_nHeight)*DCell.CELL_BORDER_LENGTH;
        _v[3] = new Vector3(0.0f, 0.0f, m_Size.d_nHeight) *DCell.CELL_BORDER_LENGTH;
        int[] _tr = new int[]
        {
            0,2,1,0,3,2
        };
        _mesh.vertices = _v;
        _mesh.triangles = _tr;
        _floor.AddComponent<MeshCollider>().sharedMesh = _mesh;
        _floor.transform.parent = _stage.transform;

        //place ceiling mesh
        GameObject _ceiling = CreateCeiling(new Vector2(m_Size.d_nWidth, m_Size.d_nHeight));
        _ceiling.transform.position = new Vector3(m_Size.d_nWidth / 2.0f, 4.0f, m_Size.d_nHeight / 2.0f) * DCell.CELL_BORDER_LENGTH;
        _ceiling.transform.localScale = new Vector3(m_Size.d_nWidth, 1.0f, m_Size.d_nHeight) * DCell.CELL_BORDER_LENGTH;
        _ceiling.transform.parent = _stage.transform;

        //Place Scene Objects
        for (int i=0; i<m_Objects.Count; i++) {
            var _p = m_Objects[i];
            GameObject _obj = Object.Instantiate(_p.d_Prefab);
            _obj.GetComponent<Transceiver>()._dataCache32 = _p.d_Data;
            _obj.transform.position = new Vector3(_p.x * DCell.CELL_BORDER_LENGTH, 0.0f, _p.y * DCell.CELL_BORDER_LENGTH);
            _obj.transform.parent = _stage.transform;
        }

        //move Player to start point
        //TODO:Create Player with data
        GameManager.Player.transform.position = m_StartPoint;
        //_player = Camera.main.GetComponent<GameInput>().m_Player;
        //Camera.main.GetComponent<Minimap>().stage = this;
        //Camera.main.GetComponent<Fullmap>().stage = this;

        #region TEST
        ItemPool.CreateItem("Cracker", new Vector3(m_StartPoint.x+0.5f, 1.0f, m_StartPoint.z));
        #endregion

        //testing
        //for(int i=0; i<m_Size.d_nWidth; i++) {
        //    for(int j=0; j<m_Size.d_nHeight; j++) {
        //        DCell _tmp = m_Data[i + j * m_Size.d_nWidth];
        //        if(_tmp != null) {
        //            GameObject _obj;
        //            if (_tmp.d_bIsJoint) {
        //                _obj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        //            }
        //            else if (_tmp.d_bIsWall) {
        //                _obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //            }
        //            else if (_tmp.d_bIsDoor) {
        //                _obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //            }
        //            else {
        //                //_obj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        //                continue;
        //            }
        //            _obj.transform.position = new Vector3(i * DCell.CELL_BORDER_LENGTH, 0.0f, j * DCell.CELL_BORDER_LENGTH);
        //            _obj.transform.parent = _stage.transform;
        //        }
        //    }
        //}
    }
}
