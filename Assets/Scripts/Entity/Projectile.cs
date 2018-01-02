using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    DAMAGE_PHYSICAL,
    DAMAGE_FIRE,
    DAMAGE_FROST,
    DAMAGE_ELECTRIC,
    DAMAGE_LASER,
}

public enum ProjectileType {
    PROJECTILE_MELEE,
    PROJECTILE_BULLET,
    PROJECTILE_EXPLOSION,
    PROJECTILE_LASER,
}

/// <summary>
/// Projectile
/// 
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour {

    #region Properties
    public float m_Damage = 0;
    public float m_Speed = 30.0f;
    public ProjectileType m_nType = ProjectileType.PROJECTILE_BULLET;
    public int m_MaxTargetNum = 1;
    public List<GameObject> m_HitEffect;
    #endregion

    #region Runtime
    public GameObject m_Master;
    public Item m_Weapon;
    public Item m_Parts;
    public float m_fLife = 10.0f;
    private HashSet<int> m_Set;
    #endregion

    // Use this for initialization
    void Start () {
        m_Set = new HashSet<int>();
	}
	
	// Update is called once per frame
	void Update () {
        m_fLife -= Time.deltaTime;
        if(m_fLife < 0.0f) {
            Destroy(gameObject);
        }

        //movement
        if (m_nType == ProjectileType.PROJECTILE_BULLET) {
            gameObject.transform.position += gameObject.transform.forward.normalized * m_Speed * Time.deltaTime;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;
        //if entity
        if(other.gameObject != m_Master && other.gameObject.GetComponent<Entity>() != null) {
            
            if (!m_Set.Contains(other.gameObject.GetInstanceID())) {
                m_Set.Add(other.gameObject.GetInstanceID());
                //before damage

                //damage
                Transceiver.SendSignal(new DSignal(m_Master, other.gameObject, "Damage", m_Damage));
                if(m_HitEffect.Count == 0) {
                    ModelEffect.CreateEffect(Resources.Load<GameObject>("Prefabs/Effects/Flare"), gameObject.transform.position);
                }
                else {
                    for(int i=0; i<m_HitEffect.Count; i++) {
                        ModelEffect.CreateEffect(m_HitEffect[i], gameObject.transform.position);
                    }
                }

                //after damage
                if (m_Weapon != null)
                    m_Weapon.Hit(other.gameObject.GetComponent<Entity>());
                if (m_Parts != null)
                    m_Parts.Hit(other.gameObject.GetComponent<Entity>());

                if (m_Set.Count >= m_MaxTargetNum && m_MaxTargetNum > 0) {
                    Destroy(gameObject);
                }
            }
        }else if(other.gameObject.tag == "SceneObj") {
            //if wall,cut through? rebound?

            if (m_HitEffect.Count == 0) {
                ModelEffect.CreateEffect(Resources.Load<GameObject>("Prefabs/Effects/Flare"), gameObject.transform.position);
            }
            else {
                for (int i = 0; i < m_HitEffect.Count; i++) {
                    ModelEffect.CreateEffect(m_HitEffect[i], gameObject.transform.position);
                }
            }

            //temporary destroyed


            Destroy(gameObject);
        }
    }
}
