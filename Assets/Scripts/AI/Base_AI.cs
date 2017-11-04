using UnityEngine;
using System.Collections;

public class Base_AI : MonoBehaviour {

    #region public
    [Range(0, 360)]
    public float m_MinTurnSpeed = 90f;
    [Range(0, 720)]
    public float m_MaxTurnSpeed = 360f;
    #endregion

    #region temporary
    protected bool m_bOnGround;
    protected bool m_bStraightGo = false;
    protected bool m_bHaveToWalkDest = true;
    protected float m_fRotateAngles = 0f;
    protected float m_fMoveDis;
    protected Vector3 m_WorldMoveDir;
    protected Vector3 m_LocalMoveDir;
    protected Vector3 m_vGroundNormal = Vector3.up;
    #endregion

    #region internal
    protected float m_CheckDis = 1f;
    #endregion

    //===============================================
    // Desc:世界XZ移动方向转化为局部XYZ移动方向
    //===============================================
    public void WorldToLocalDir(Vector3 dest)
    {
        //得到世界移动方向，并正交化
        m_WorldMoveDir = dest - transform.position;
        m_fMoveDis = Mathf.Abs(m_WorldMoveDir.magnitude);
        m_WorldMoveDir.Normalize();

        //转化为局部移动方向
        m_LocalMoveDir = transform.InverseTransformDirection(m_WorldMoveDir);
        CheckGroundState();
        m_LocalMoveDir = Vector3.ProjectOnPlane(m_LocalMoveDir, m_vGroundNormal);

    }

    //=========================================
    // Desc:弧度转化为角度
    //=========================================
    public float radToDegree(float radian)
    {
        return radian / (2 * Mathf.PI) * 360;
    }

    //=========================================
    // Desc:计算旋转角度
    //=========================================
    public void RotateAnglesCompute()
    {
        m_fRotateAngles = Mathf.Atan2(m_LocalMoveDir.x, m_LocalMoveDir.z);
        m_fRotateAngles = radToDegree(m_fRotateAngles);
    }

    //==========================================================
    // Desc:转身到目标(1.近似笔直，直接转。2.否则，插值转)
    //==========================================================
    public bool TurnRotation(Vector3 local_MoveDir, bool immediately = false, float minTurnSpeed = 90, float maxTurnSpeed = 360)
    {
        m_fRotateAngles = Mathf.Atan2(local_MoveDir.x, local_MoveDir.z);
        m_fRotateAngles = radToDegree(m_fRotateAngles);
        bool bTurnFinish = false;
        if (immediately || Mathf.Abs(m_fRotateAngles) < 3)
        {
            transform.Rotate(0, m_fRotateAngles, 0);
            bTurnFinish = true;
        }
        else
        {
            float turnSpeed = Mathf.Lerp(minTurnSpeed, maxTurnSpeed, 1 - local_MoveDir.z);
            float turnAngles = turnSpeed * Time.deltaTime;
            if (m_fRotateAngles < 0) turnAngles = -turnAngles;
            if (Mathf.Abs(turnAngles) >= Mathf.Abs(m_fRotateAngles))
            {
                turnAngles = m_fRotateAngles;
                bTurnFinish = true;
            }
            transform.Rotate(0, turnAngles, 0);
        }
        return bTurnFinish;
    }

    //=========================================
    // Desc:检查地面状态
    //=========================================
    public bool CheckGroundState()
    {
        RaycastHit rayHitInfo;
        bool rayHit = Physics.Raycast(transform.position, -m_vGroundNormal, out rayHitInfo, m_CheckDis);
        if (rayHit)
        {
            m_vGroundNormal = rayHitInfo.normal;
            m_bOnGround = true;
        }
        else
        {
            m_vGroundNormal = Vector3.up;
            m_bOnGround = false;
        }
        return m_bOnGround;
    }

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

}
