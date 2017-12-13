using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CellManager : MonoBehaviour {

    [SerializeField]
    int map_width;
    public int Width
    {
        get { return map_width; }
    }
    [SerializeField]
    int map_height;
    public int Height
    {
        get { return map_height; }
    }
    //仅由MAP窗口调用
    public void RegenerateMap_Editor(int x,int y)
    {
        map_width = x;
        map_height = y;
        generateMap();
    }
    [SerializeField]
    //定义Cell的间距
    int cell_delta;
    [SerializeField]
    Vector2 origin;
    public Vector2 Origin { get { return origin; } }
    public Vector2 Origin_Build { get { return origin + new Vector2(-0.5f, -0.5f); } }

    Transform[,] map;
    bool[,] usedMap;
    public GameObject cellPrefeb;
    // Use this for initialization
    void Start() {
        //generateMap();
        ReadMapFromEditor();
    }

    // Update is called once per frame
    void Update() {

    }
    public void clearMap() {
        if (map != null) {
            map = null;
        }
        if (usedMap != null) {
            usedMap = null;
        }
        var container = GameObject.Find("CellContainer");
        var cells = container.GetComponentsInChildren<Cell>();
        for (int i = 0; i < cells.Length; i++) {
            DestroyImmediate(cells[i].gameObject);
        }
        foreach (var obj in GameObject.Find("BuildingContainer").GetComponentsInChildren<SceneObj>()) 
        {
            DestroyImmediate(obj.gameObject);
        }
    }
    public Transform getCell(Vector2 pos) {
        int x = (int)pos.x;
        int y = (int)pos.y;
        int mx = map.GetLength(0);
        int my = map.GetLength(1);
        if (x >= mx || y >= my) {

            return null;
        }
        return map[x, y];
    }
    public Vector3 getCellPos(Vector2 pos) {
        return new Vector3(
            origin.x + pos.x, origin.y + pos.y, 0);
    }
    //从当前已生成的CELL构建出MAP
    public void ReadMapFromEditor() {
        if (map==null) {
            map = new Transform[map_width, map_height];
        }
        if (usedMap == null) {
            usedMap = new bool[map_width, map_height];
        }
        for(int i = 0; i < usedMap.GetLength(0); i++) {
            for(int j = 0; j < usedMap.GetLength(1); j++) {
                usedMap[i, j] = false;
            }
        }
        var cells = this.GetComponentsInChildren<Cell>();
        foreach(var cell in cells) {
            var mappos = cell.mapPos;
            map[(int)mappos.x, (int)mappos.y] = cell.transform;
        }
    }
    public void generateMap() {
        clearMap();

        map = new Transform[map_width, map_height];
        usedMap = new bool[map_width, map_height];
        var container = GameObject.Find("CellContainer");
        
        //普通MAP初始化
        for (int i = 0; i < map.GetLength(0); i++) {
            for (int j = 0; j < map.GetLength(1); j++) {
                //初始化MAP，生成CELL
                var cell = Instantiate(cellPrefeb, container.transform);
                cell.GetComponent<Cell>().Initilize();
                cell.transform.localPosition = new Vector3(i* cell_delta+origin.x, 0.1f ,j* cell_delta+origin.y);

                cell.GetComponent<Cell>().mapPos = new Vector2(i, j);
                map[i, j] = cell.transform;

                //初始化UsedMap为False
                usedMap[i,j] = false;
            }
        }
    }

    //Helper Function
    public SCENEOBJ_TYPE buildType = SCENEOBJ_TYPE.ROCK;
    //在周围填充方块
    public void BuildBorder() {
        for(int i = 0; i < map.GetLength(0); i++) {
            map[i, 0].GetComponent<Cell>().BuildBox(buildType);
            map[i, map.GetLength(1) - 1].GetComponent<Cell>().BuildBox(buildType);
        }
        for(int j = 0; j < map.GetLength(1); j++) {
            map[0, j].GetComponent<Cell>().BuildBox(buildType);
            map[map.GetLength(0) - 1, j].GetComponent<Cell>().BuildBox(buildType);
        }
    }
    
    
    //UsedMap 接口
    public bool getIfUsed(int x, int y) {
        //Debug.Log(usedMap);
        //Debug.Log(map);
        return usedMap[x, y]; }
    public void setCellUsed(int x,int y) { usedMap[x, y] = true; }
    public void setCellUnUsed(int x,int y) { usedMap[x, y] = false; }
    //通過世界坐標獲取Cell
    /*
    public Cell getMapPosByWorldPos(Vector3 worldPos) {
        Vector2 wpos = new Vector2(worldPos.x, worldPos.y);
        wpos -= origin;
        Debug.Log(new Vector2(wpos.x % cell_delta ,wpos.y % cell_delta));
        return map[(int)(wpos.x % cell_delta), (int)(wpos.y % cell_delta)].GetComponent<Cell>();
    }*/
}
    