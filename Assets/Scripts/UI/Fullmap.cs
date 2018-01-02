using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fullmap : MonoBehaviour {

    public RawImage map;
    public Stage stage;
    bool m_bVisible = false;

    private void Start()
    {
        EventDispatcher.AddListener("FullMap", SetVisible);
    }

    void SetVisible(LEvent e)
    {
        m_bVisible = !m_bVisible;
        map.enabled = m_bVisible;
    }

    // Update is called once per frame
    void LateUpdate () {
        if (null != map && null != GameManager.CurrentStage) {
            map.texture = GameManager.CurrentStage.minimapTex;
        }
    }
}
