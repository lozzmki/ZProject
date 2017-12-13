using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 重量级枪械
/// </summary>
public class BigGun : Weapon {
    //重量级枪械基本属性
    [SerializeField]
    protected float m_fFrequency; // 射击频率
    [SerializeField]
    protected float m_fInitialDamage; //初始伤害
    [SerializeField]
    protected float m_fDamping; //伤害衰减
    [SerializeField]
    protected float m_fAccuracy; //精度
    [SerializeField]
    protected float m_Weight; //重量

    [SerializeField]
    protected float m_fSpeed; //子弹初始速度
    [SerializeField]
    protected GameObject m_Bullet;
    [SerializeField]
    protected float m_Offset;
    
    private float m_AccTime = 0.0f;
    [SerializeField]
    private bool m_CanShoot = false;
    public override void LoadWeapon(HeroEquipment hero) {
        throw new NotImplementedException();
    }

    public override void UnLoadWeapon(HeroEquipment hero) {
        throw new NotImplementedException();
    }

    public override void UseWeapon() {
        //throw new NotImplementedException();
        if (!m_CanShoot) {
            return;
        }
        m_CanShoot = false;
        //暂时直接初始化弹药
        var Bullet = Instantiate(m_Bullet);
        m_Bullet.transform.position = this.transform.position;

        var direction = this.transform.position + 100 * this.transform.forward;
        m_Bullet.GetComponent<Bullet>().Bullet_Initilize(m_fInitialDamage, m_fSpeed, direction,m_Offset);
        

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //更新射击冷却
        if (!m_CanShoot) {
            m_AccTime += Time.deltaTime;
            if (m_AccTime >= 1 / m_fFrequency) {
                m_AccTime = 0.0f;
                m_CanShoot = true;
            }
        }
       
    }
}
