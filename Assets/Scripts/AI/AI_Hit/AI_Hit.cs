using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class AI_Hit : MonoBehaviour
{
    #region public
    //public GameObject sphere;
    public string m_AttackTargetName = "Cube";
    [SerializeField]
    public HitAction[] m_HitActions;    
    #endregion

    #region temporary
    protected bool m_bInAttackTrigger;
    protected bool m_bHaveGiveAttack;
    protected Vector3 m_WorldMoveDir;
    protected Vector3 m_BeginHitPos;
    #endregion

    #region internal
    int m_HitActionCounts;
    protected Animator m_Animator;
    protected Animator m_PlayerAnimator;
    protected AnimatorStateInfo m_AsInfo;
    protected Rigidbody m_Rigidbody;
    protected GameObject m_AttackTarget;
    #endregion

    #region userDefine
    [System.Serializable]
    public struct ActionFragment
    {
        public float m_BeginPercentage;
        public float m_EndPercentage;
        public int m_ATK;
    }
    [System.Serializable]
    public struct HitAction
    {
        public string m_HitActionName;
        public ActionFragment[] m_ActionFragments;
    }
    #endregion

    // Use this for initialization
    void Start()
    {
        Init();
    }

    void Update()
    {
        HitJudgeInAnimation();
    }

    //===============================
    // Desc:初始化
    //===============================
    protected void Init()
    {
        m_Animator = GetComponentInParent<Animator>();
        m_AttackTarget = GameObject.Find(m_AttackTargetName);
        m_PlayerAnimator = m_AttackTarget.GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.useGravity = false;
        m_Rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        m_Rigidbody.isKinematic = true;
        m_HitActionCounts = m_HitActions.GetLength(0);
    }

    //=================================
    // Desc: 在动画帧中进行打击判断
    //=================================
    void HitJudgeInAnimation()
    {      
        m_AsInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
        float playTime = m_AsInfo.normalizedTime - (int)m_AsInfo.normalizedTime;
        int i = 0;
        while (i < m_HitActionCounts)
        {
            //【判断某个击打动作】
            if (m_AsInfo.IsName(m_HitActions[i].m_HitActionName))
            {
                ActionFragment[] af = m_HitActions[i].m_ActionFragments;
                int fragmentNums = af.GetLength(0);
                int j = 0;
                while (j < fragmentNums)
                {
                    //【判断该攻击片段帧是否击中目标】
                    if (playTime >= af[j].m_BeginPercentage && playTime <= af[j].m_EndPercentage)
                    {
                        //【击中判断】
                        if(HitJudge())
                        {
                            Vector3 hitPos = m_AttackTarget.transform.position;
                            m_WorldMoveDir = m_BeginHitPos - hitPos;
                            Transceiver.SendSignal(new DSignal(gameObject, m_AttackTarget, "Damage", af[j].m_ATK));
                            Transceiver.SendSignal(new DSignal(gameObject, m_AttackTarget, "DamageMove", m_WorldMoveDir));
                            m_bHaveGiveAttack = true;              
                        }
                        return;
                    }
                    j++;
                }
                //【在该动作的空余片段中更新击打位置】
                m_bHaveGiveAttack = false;
                m_BeginHitPos = transform.position;
                return;
            }
            i++;
        }
    }
    //=================================
    // Desc:击中判断
    //=================================
    protected bool HitJudge()
    {
        if (m_bInAttackTrigger)
        {
            if (!m_bHaveGiveAttack)
            {
                return true;
            }
        }
        return false;
    }

    /*
     * Desc:进入触发
     */ 
    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject == m_AttackTarget)
        {
            m_bInAttackTrigger = true;
            Debug.Log("Trigger Enter");
        }
    }
    /*
     * Desc:退出触发
     */
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == m_AttackTarget)
        {
            m_bInAttackTrigger = false;
            Debug.Log("Trigger Exit");
        }
    }
}
