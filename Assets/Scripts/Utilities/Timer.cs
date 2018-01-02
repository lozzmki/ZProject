using System;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    #region Param
    private float m_fTime;
    private float m_fSetTime;
    private bool m_bPaused;
    #endregion

    #region Field
    public float TimerTime
    {
        get
        {
            return m_fSetTime;
        }
        set
        {
            m_fSetTime = value;
        }
    }
    public float LeftTime
    {
        get
        {
            return m_fTime;
        }
        set
        {
            if (value > m_fSetTime)
                m_fTime = m_fSetTime;
            else
                m_fTime = value;
        }
    }
    public bool IfExpired
    {
        get
        {
            return m_fTime < 0.0f;
        }
    }
    public bool Paused
    {
        get
        {
            return m_bPaused;
        }
        set
        {
            m_bPaused = value;
        }
    }

    public float LeftPercent
    {
        get
        {
            float _r;
            if (m_fSetTime > 0.0f){
                _r = m_fTime / m_fSetTime;
                if (_r < 0.0f)
                    _r = 0.0f;
            }
            else
                _r = 0.0f;
            return _r;
        }
    }
    #endregion

    public Timer(float time = 1.0f)
    {
        m_fSetTime = m_fTime = time;
        m_bPaused = false;
    }


    public void Update()
    {
        if(!m_bPaused)
            if(m_fTime>0.0f)
                m_fTime -= Time.deltaTime;
    }
    public void Reset()
    {
        m_fTime = m_fSetTime;
    }


}

