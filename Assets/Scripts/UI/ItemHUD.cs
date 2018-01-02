using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHUD : MonoBehaviour {

    public GameObject m_Background;
    public GameObject m_Price;
    
	// Use this for initialization
	//void Start () {
	//	
	//}
	
	// Update is called once per frame
	void Update () {

        transform.rotation = Camera.main.transform.rotation;
        int _nLength = GetComponent<TextMesh>().text.Length;
        float _height = 0.7f;
        if (m_Price.GetComponent<TextMesh>().text.Length > 0) {
            m_Background.transform.localPosition = Vector3.up * 8.25f;
            _height = 1.5f;
        }
        else {
            m_Background.transform.localPosition = Vector3.zero;
        }

        m_Background.transform.localScale = new Vector3(_nLength/2.5f, _height, 1.0f)*25;
        if (null != m_Target)
            transform.position = m_Target.transform.position + new Vector3(0, 0.7f);
        else
            transform.position = new Vector3(0, 0, 10000);
	}

    static GameObject _obj;

    public static GameObject Instance
    {
        get
        {
            if (_obj == null) {
                _obj = new GameObject("ItemHUD");
                _obj.AddComponent<TextMesh>().anchor = TextAnchor.MiddleCenter;
                _obj.GetComponent<TextMesh>().fontSize = 100;

                GameObject _price = new GameObject("ItemHUD_price");
                _price.AddComponent<TextMesh>().anchor = TextAnchor.MiddleCenter;
                _price.GetComponent<TextMesh>().fontSize = 100;
                _price.GetComponent<TextMesh>().color = Color.yellow;
                _price.transform.parent = _obj.transform;
                _price.transform.localPosition = Vector3.up * 16.5f;

                GameObject _bg = new GameObject("ItemHUD_BG");
                Mesh _mesh = _bg.AddComponent<MeshFilter>().mesh;
                _bg.AddComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/HUDBG");
                _mesh.vertices = new Vector3[4]
                {
                    new Vector3(-0.5f, -0.5f),
                    new Vector3(-0.5f, 0.5f),
                    new Vector3(0.5f, 0.5f),
                    new Vector3(0.5f, -0.5f)
                };
                _mesh.triangles = new int[6]
                {
                    0,1,2,0,2,3
                };
                _bg.transform.parent = _obj.transform;

                _obj.AddComponent<ItemHUD>().m_Background = _bg;
                _obj.GetComponent<ItemHUD>().m_Price = _price;
                _obj.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
            }
            return _obj;
        }
    }
    GameObject m_Target;
    public static GameObject markItem
    {
        set
        {
            if (Instance.GetComponent<ItemHUD>().m_Target == value)
                return;

            Instance.GetComponent<ItemHUD>().m_Target = value;
            if (value != null && value.GetComponent<Item>() != null) {
                Instance.GetComponent<TextMesh>().text = value.GetComponent<Item>().m_ItemName;
                if (markItem.GetComponent<Item>().m_bForSale) {
                    Instance.GetComponent<ItemHUD>().m_Price.GetComponent<TextMesh>().text = value.GetComponent<Item>().m_Price.ToString();
                }
                else {
                    Instance.GetComponent<ItemHUD>().m_Price.GetComponent<TextMesh>().text = "";
                }
            }
            else {
                Instance.GetComponent<TextMesh>().text = "";
            }
        }
        get
        {
            return Instance.GetComponent<ItemHUD>().m_Target;
        }
    }

}
