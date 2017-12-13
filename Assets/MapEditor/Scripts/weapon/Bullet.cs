using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 子弹类
/// </summary>
public class Bullet : MonoBehaviour {
    private float m_Damage;
    public float Damage {
        get { return m_Damage; }
        set { m_Damage = value; }
    }
    [SerializeField]
    private float m_Speed;
    public float Speed {
        set { m_Speed = value; }
    }
    [SerializeField]
    private bool isIntilized = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (isIntilized) {
            this.transform.position += this.transform.forward * m_Speed * Time.deltaTime;
        }
	}
    public void Bullet_Initilize(float damage,float speed,Vector3 iforward,float offset) {
        this.m_Damage = damage;
        this.m_Speed = speed;
        this.transform.LookAt(iforward);
        this.transform.position += offset * this.transform.forward;
        Debug.Log("BulletForward:" + this.transform.forward);
        this.isIntilized = true;
    }
}
