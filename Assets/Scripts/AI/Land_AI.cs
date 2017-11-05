using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]

public class Land_AI : MoveObj
{
    #region public
    public GameObject m_AttackTarget;
    public bool m_bDead;
    #endregion

    #region temporary
    protected bool m_bInBackMoving;
    protected float m_fBackMoveAccumTime = 0f;
    protected float m_fSinkAccumTime = 0f;
    #endregion

    //==================================
    // Desc:搜索敌人
    //==================================
    public bool SearchEnemy(Vector3 enemyPos, float searchRadius)
    {
        //计算移动方向和距离
        WorldToLocalDir(enemyPos);
        if (m_fMoveDis <= searchRadius)
            return true;
        else
            return false;
    }

    //========================================
    // Desc:被击打后朝"攻击目标"反方向后退
    //========================================
    public void AddBackMovePower(bool addForce = true, float force = 3f)
    {
        TurnRotation(m_LocalMoveDir, true);
        m_bInBackMoving = true;
        m_fBackMoveAccumTime = 0f;
        if (addForce)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation|RigidbodyConstraints.FreezePositionY;
            m_WorldMoveDir.y = 0;
            m_WorldMoveDir.Normalize();
            GetComponent<Rigidbody>().AddForce(-m_WorldMoveDir * force, ForceMode.Impulse);
        }
    }

    //==================================
    // Desc:被攻击时持续向后移动
    //==================================
    public bool BackMove(float duration)
    {
        bool bBackMoveFinish = false;
        m_fBackMoveAccumTime += Time.deltaTime;
        if (m_fBackMoveAccumTime >= duration)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            bBackMoveFinish = true;
            m_fBackMoveAccumTime = 0f;
        }
        return bBackMoveFinish;
    }

    //==================================
    // Desc:死亡时尸体开始下沉
    //==================================
    public bool DieSink(float duration, float sinkSpeed)
    {
        bool bSinkFinish = false;
        transform.Translate(Vector3.down * sinkSpeed * Time.deltaTime, Space.Self);
        m_fSinkAccumTime += Time.deltaTime;
        if (m_fSinkAccumTime > duration)
        {
            m_fSinkAccumTime = 0f;
            bSinkFinish = true;
        }
        return bSinkFinish;
    }
}
