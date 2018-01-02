using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour {

    public RawImage map;
    public Stage stage;

    Vector2 m_vMapSize = new Vector2(0.5f, 0.5f);

    // Use this for initialization
    //void Start () {
    //	
    //}

    // Update is called once per frame
    void LateUpdate()
    {
        if (null != map && null != GameManager.CurrentStage) {
            map.texture = GameManager.CurrentStage.minimapTex;


            //TODO:temporary, move to setstage method
            float _fMaxPixels = 40.0f;
            float _fBaseRatio;
            if(GameManager.CurrentStage.m_Size.d_nWidth > GameManager.CurrentStage.m_Size.d_nHeight) {
                _fBaseRatio = _fMaxPixels / GameManager.CurrentStage.m_Size.d_nHeight;
                if (_fBaseRatio > 1.0f)
                    _fBaseRatio = 1.0f;
                m_vMapSize = new Vector2(_fBaseRatio * GameManager.CurrentStage.m_Size.d_nHeight / GameManager.CurrentStage.m_Size.d_nWidth, _fBaseRatio);
            }
            else {
                _fBaseRatio = _fMaxPixels / GameManager.CurrentStage.m_Size.d_nWidth;
                if (_fBaseRatio > 1.0f)
                    _fBaseRatio = 1.0f;
                m_vMapSize = new Vector2(_fBaseRatio, _fBaseRatio * GameManager.CurrentStage.m_Size.d_nWidth / GameManager.CurrentStage.m_Size.d_nHeight);
            }

            Rect _rct = new Rect(0, 0, m_vMapSize.x, m_vMapSize.y);
            
            Vector2 _pos = GameManager.CurrentStage.playerPos;
            if (_pos.x <= m_vMapSize.x / 2.0f)
                _rct.x = 0.0f;
            else if (_pos.x >= 1.0f - m_vMapSize.x / 2.0f)
                _rct.x = 1.0f - m_vMapSize.x;
            else
                _rct.x = _pos.x - m_vMapSize.x / 2.0f;
            
            if (_pos.y <= m_vMapSize.y / 2.0f)
                _rct.y = 0.0f;
            else if (_pos.y >= 1.0f - m_vMapSize.y / 2.0f)
                _rct.y = 1.0f - m_vMapSize.y;
            else
                _rct.y = _pos.y - m_vMapSize.y / 2.0f;
            
            map.uvRect = _rct;
        }
        
    }
}
