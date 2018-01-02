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
public class Projectile : MonoBehaviour {
    [HideInInspector]public float m_Damage = 0;
    [HideInInspector]public float m_Speed = 30.0f;
    public int m_MaxTargetNum = 1;
    public GameObject m_Master;
    public Item m_Weapon;
    public Item m_Parts;
    private float m_fLife = 5.0f;
    private HashSet<int> m_Set;
	// Use this for initialization
	void Awake () {
        m_Set = new HashSet<int>();
	}
	
	// Update is called once per frame
	void Update () {
        m_fLife -= Time.deltaTime;
        if(m_fLife < 0.0f) {
            Safe_Destroy();
        }

        //movement
        gameObject.transform.position += gameObject.transform.forward.normalized * m_Speed * Time.deltaTime;
	}

    public void Safe_Destroy()
    {
        if (Globe.netMode)
        {
            GetComponent<SyncPosRot>().entity.renderObj = null;
            GetComponent<SyncPosRot>().entity.cellCall("reqDestroySelf");
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        //if wall,cut through? rebound?

        //if entity
        if(other.gameObject != m_Master && other.gameObject.GetComponent<Entity>() != null) {
            
            if (!m_Set.Contains(other.gameObject.GetInstanceID())) {
                m_Set.Add(other.gameObject.GetInstanceID());
                //before damage

                //damage
                Transceiver.SendSignal(new DSignal(m_Master, other.gameObject, "Damage", m_Damage));

                //after damage
                if (m_Weapon != null)
                    m_Weapon.Hit(other.gameObject.GetComponent<Entity>());
                if (m_Parts != null)
                    m_Parts.Hit(other.gameObject.GetComponent<Entity>());

                if (m_Set.Count >= m_MaxTargetNum) {
                    Safe_Destroy();
                }
            }
            
        }
    }
}
