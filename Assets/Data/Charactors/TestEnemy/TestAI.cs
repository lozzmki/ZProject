using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAI : BaseAI {

    float m_fIdleTime;
    float m_fMoveTime;
    bool m_bAttacked = true;

    override protected void Init()
    {
        RegisterHandler("Idle", OnIdle);
        RegisterHandler("Move", OnMove);
        RegisterHandler("Attack", OnAttack);

    }

    void OnIdle()
    {
        //stand then move towards a random direction
        m_fIdleTime -= Time.deltaTime;
        if (m_fIdleTime <= 0.0f) {
            if(m_bAttacked){
                float _ang = Random.Range(0.0f, 360.0f);
                Vector3 _dir = new Vector3(Mathf.Cos(_ang), 0.0f, Mathf.Sin(_ang));
                //gameObject.transform.forward = _dir;
                gameObject.GetComponent<Entity>().m_MovingDirection = _dir;
                m_fMoveTime = Random.Range(0.8f, 1.2f);
                m_Animator.SetBool("move", true);
            }
            else {
                m_bAttacked = true;
                m_Animator.SetBool("attack", true);
            }
        }
    }

    void OnMove()
    {
        //move forward ,then stop
        m_fMoveTime -= Time.deltaTime;
        if (m_fMoveTime <= 0.0f) {
            m_fIdleTime = Random.Range(0.8f, 1.2f);
            m_Animator.SetBool("move", false);
            m_bAttacked = false;
        }
    }

    void OnAttack()
    {
        //do nothing
    }

    void OnKeyFrame()
    {
        GameObject _proj = Instantiate(Resources.Load<GameObject>("Prefabs/Projectiles/Default"), gameObject.transform.position, Quaternion.AngleAxis(0.0f, Vector3.up));
        _proj.transform.forward = gameObject.transform.forward;
        _proj.GetComponent<Projectile>().m_Master = gameObject;
        _proj.GetComponent<Projectile>().m_Damage += 10.0f;
        _proj.GetComponent<Projectile>().m_Speed = 10.0f;
    }

    void OnAttackOver()
    {
        m_Animator.SetBool("attack", false);
    }
}
