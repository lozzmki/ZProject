using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObj : MonoBehaviour {
    public Cell m_FatherCell;
    [SerializeField]
    private string m_SName;
    public string GetName
    {
        get { return m_SName; }
    }
    //旋转
    [SerializeField]
    private int rotateId = 0;
    public int GetObjRotate
    {
        get { return rotateId; }
        set
        {
            rotateId = value % 4;
            this.transform.rotation = Quaternion.Euler(new Vector3(0, rotateId * 90f, 0));
        }
    } 
}
