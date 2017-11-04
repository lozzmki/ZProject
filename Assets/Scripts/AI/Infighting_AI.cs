using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(Transceiver))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(NavMeshAgent))]

public class Infighting_AI : Land_AI
{
    #region public
    public ParticleSystem m_HitEffect;
    public ParticleSystem m_DeadEffect;
    //public GameObject m_DropPrefab;
    public float m_fBackMoveSpeed = 3f;
    public float m_fBackMoveLimitTime = 0.3f;
    public float m_fDieSinkSpeed = 1f;
    public float m_fDieSinkLimitTime = 1f;
    public float m_fSearchEnemyDis = 5f;
    #endregion

    #region temporary
    bool m_bComeUp;
    bool m_bFindAttackTarget;
    bool m_bTurnFinish;
    bool m_bDieSink;
    #endregion

    #region internal
    Animator m_Animator;
    CapsuleCollider m_CapsuleCollider;
    Rigidbody m_Rigidbody;
    Transceiver m_Transceiver;
    NavMeshAgent m_NavMeshAgent;
    AnimatorStateInfo m_AsInfo;
    #endregion

    #region AnimatorState
    bool m_bRun;
    bool m_bAttack;
    #endregion

    #region Property
    public float m_HP = 1000;
    public float m_ATT = 50;
    public float m_DEF = 80;
    #endregion

    // Use this for initialization
	void Start () {
        m_CapsuleCollider = GetComponent<CapsuleCollider>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
        m_Transceiver = GetComponent<Transceiver>();
        m_NavMeshAgent = GetComponent<NavMeshAgent>();

        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        m_NavMeshAgent.enabled = false;
        m_Transceiver.AddResolver("Damage", getDamage);
        if (m_AttackTarget == null)
        {
            Debug.LogError(name+"没有攻击目标，请重新进行设定!!");
            enabled = false;        
        }       
	}

    public void getDamage(DSignal signal)
    {
        if(!m_bFindAttackTarget)
        {
            m_bFindAttackTarget = true;
            m_bRun = true;
        }
        float damage = (float)signal._arg1;
        m_HP -= damage;
        m_Animator.SetTrigger("BeHitTrigger");
        AddBackMovePower();
        m_HitEffect.Play();

        //【死亡】
        if (m_HP <= 0)
        {
            m_bDead = true;
            m_NavMeshAgent.enabled = false;
            if (!m_DeadEffect.isPlaying)
                m_DeadEffect.Play();
        }
    }

    // Update is called once per frame
    void Update () {
        //【我还活着时】
        if (!m_bDead)
        {
            if (!m_bComeUp)
            {
                //【出现】
                m_bComeUp = Appear();
                if (m_bComeUp)
                {
                    m_NavMeshAgent.enabled = true;
                    m_Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                }
            }
            else
            {                
                //【发现攻击目标】
                if (m_bFindAttackTarget)
                {
                    //【追踪目标】
                    if (m_NavMeshAgent.enabled)
                    {
                        m_NavMeshAgent.SetDestination(m_AttackTarget.transform.position);
                    }

                    //【移动和攻击】
                    MoveAndAttack();
                }
                else
                {
                     //【搜索敌人】
                    m_bFindAttackTarget = SearchEnemy(m_AttackTarget.transform.position, m_fSearchEnemyDis);
                    if (m_bFindAttackTarget) m_bRun = true;
                }
            }

            //【如果被攻击了向后退】
            if (m_InBackMoving)
            {
                bool backMoveFinish = BackMove(m_fBackMoveLimitTime, m_fBackMoveSpeed);
                m_InBackMoving = !backMoveFinish;
            }
              
        }
        else //【我已挂掉】
        {
            if (m_bDieSink)
            {
                bool dieSinkFinish = DieSink(m_fDieSinkLimitTime, m_fDieSinkSpeed);
                if (dieSinkFinish)
                {
                    Destroy(gameObject);
                }
            }
        }

        if (!m_bDieSink)
        {
            //动画状态检测和更改
            CheckAnimatorAndChange();

            //更新动画状态
            UpdateAnimator();
        }
       
	}

    //==================================
    // Desc:登录出场
    //==================================
    bool Appear()
    {
        CheckGroundState();
        if (m_bOnGround) 
            return true;
        else
            return false;      
    }

   

    //==================================
    // Desc:移动和攻击
    //==================================
    void MoveAndAttack()
    {
        //计算移动方向和距离
        WorldToLocalDir(m_AttackTarget.transform.position);

        //切换为攻击
        if (m_fMoveDis <= m_NavMeshAgent.stoppingDistance)
        {
            
            if (m_bTurnFinish) //攻击未完成时为攻击状态
            {
                m_bRun = false;
                m_bAttack = true;
            }
            else
            {
                //已完成则转身，转身完成重置为可攻击
                m_bTurnFinish = TurnRotation(m_LocalMoveDir);
            }

        }//切换为移动
        else if (m_fMoveDis > (m_NavMeshAgent.stoppingDistance))
        {
            if (m_bAttack)
                m_NavMeshAgent.enabled = false;
            else
                m_NavMeshAgent.enabled = true;
            m_bTurnFinish = false;
        }       
    }

    //==================================
    // Desc:更新动画状态
    //==================================
    void UpdateAnimator()
    {
        m_Animator.SetBool("bRun", m_bRun);
        m_Animator.SetBool("bAttack", m_bAttack);
        m_Animator.SetBool("bDie", m_bDead);
    }

    //==================================
    // Desc:动画状态检测和更改
    //==================================
    void CheckAnimatorAndChange()
    {
         m_AsInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
        if (m_AsInfo.IsName("attack"))
        {
            float playTime = m_AsInfo.normalizedTime - (int)m_AsInfo.normalizedTime;
            if(playTime >= 0.9f)
            {               
                //在攻击范围内正对着攻击目标，或者远离了目标,离开攻击状态
                RotateAnglesCompute();
                if ((Mathf.Abs(m_fRotateAngles) > 10f) || m_fMoveDis >= m_NavMeshAgent.stoppingDistance)
                {
                    m_bAttack = false;
                    m_bRun = true;
                    m_bTurnFinish = false;
                }
            }         
        }
        else if (m_AsInfo.IsName("gethit"))
        {
            if (m_AsInfo.normalizedTime >= 0.05f)
            {
                m_Animator.ResetTrigger("BeHitTrigger");
                if (m_fMoveDis > m_NavMeshAgent.stoppingDistance)
                {
                    m_bRun = true;
                    m_bAttack = false;
                }

            }
        }
        else if (m_AsInfo.IsName("die"))
        {
            if (m_AsInfo.normalizedTime >= 1f)
            {
                m_Rigidbody.isKinematic = true;
                m_CapsuleCollider.enabled = false;
                m_NavMeshAgent.enabled = false;
                m_bDieSink = true;
            }           
        }
   
    }

    void OnTriggerEnter(Collider other)
    {
        //若死亡不进行检测
        if (m_bDead) return;
    }
    
}
