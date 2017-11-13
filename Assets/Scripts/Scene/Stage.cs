using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//one tile
public class DCell
{
    public const float CELL_BORDER_LENGTH = 1.0f;

    public int d_nTex;
    public bool d_bIsDoor;
    public bool d_bIsWall;
    public bool d_bPathable;

    public DCell(int texture = 0, bool isDoor = false, bool isWall = false,bool ifPathable = true)
    {
        d_nTex = texture;
        d_bIsDoor = isDoor;
        d_bIsWall = isWall;
        d_bPathable = ifPathable;
    }

}

/// <summary>
/// 场景地图信息
/// </summary>
public class Stage {
    private DSizeN m_Size;
    private DCell[] m_Data;

    public Stage(int width, int height)
    {
        m_Size = new DSizeN(width, height);
        m_Data = new DCell[width * height];
    }

    public DCell GetCell(int x, int y)
    {
        return m_Data[x + y*m_Size.d_nWidth];
    }

    public DCell[] GetMap()
    {
        return m_Data;
    }
    

    public void CreateStage()
    {
        //establish stage with data,todo

        //testing
        GameObject _stage = new GameObject("stage");
        for(int i=0; i<m_Size.d_nWidth; i++) {
            for(int j=0; j<m_Size.d_nHeight; j++) {
                DCell _tmp = m_Data[i + j * m_Size.d_nWidth];
                if(_tmp != null) {
                    GameObject _obj;
                    if (_tmp.d_bIsWall) {
                        _obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    }
                    else if (_tmp.d_bIsDoor) {
                        _obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    }
                    else {
                        //_obj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                        continue;
                    }
                    _obj.transform.position = new Vector3(i * DCell.CELL_BORDER_LENGTH, 0.0f, j * DCell.CELL_BORDER_LENGTH);
                    _obj.transform.parent = _stage.transform;
                }
            }
        }
    }
}
