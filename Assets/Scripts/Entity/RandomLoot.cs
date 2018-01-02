using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum PowerUpType
{
    POWERUP_HEAL_SMALL,
    POWERUP_HEAL_MEDIUM,
    POWERUP_HEAL_LARGE,
    POWERUP_HEAL_FULL,
    POWERUP_CHARGE_SMALL,
    POWERUP_CHARGE_MEDIUM,
    POWERUP_CHARGE_LARGE,
    POWERUP_CHARGE_FULL
}
public enum LootType
{
    LOOT_ITEM,
    LOOT_POWERUP,
    LOOT_COINS,
}

[System.Serializable]
public class LootObject
{
    public LootType m_nType;
    public int m_nParam;
    public int m_nNum = 1;
    public int m_nWeight = 1;
}

[System.Serializable]
public class LootGroup
{
    public float m_fDropRate;
    public LootObject[] lootList;

    public void RollDice(Vector3 vPosition, Transform parent = null)
    {
        if (Random.value < m_fDropRate) {
            //make drops
            int _nSum = 0;
            for (int i = 0; i < lootList.Length; i++)
                _nSum += lootList[i].m_nWeight;

            int _nPick = Random.Range(0, _nSum);
            for(int i = 0; i<lootList.Length; i++) {
                if(_nPick >= lootList[i].m_nWeight) {
                    _nPick -= lootList[i].m_nWeight;
                }
                else {
                    //pick this
                    //TODO:Animation!
                    for(int k = 0; k<lootList[i].m_nNum; k++) {
                        Vector3 _vPos = vPosition + new Vector3(0.0f, 0.0f, -10000.0f);
                        GameObject _obj = null;
                        switch (lootList[i].m_nType) {
                            case LootType.LOOT_ITEM:
                                _obj = ItemPool.CreateRandomItem((ItemPool.PoolType)lootList[i].m_nParam, _vPos);
                                break;
                            case LootType.LOOT_POWERUP:
                                switch ((PowerUpType)lootList[i].m_nParam) {
                                    case PowerUpType.POWERUP_HEAL_SMALL:
                                        _obj = Object.Instantiate(Resources.Load<GameObject>("Prefabs/PowerUps/Heal(Small)"));
                                        break;
                                    case PowerUpType.POWERUP_HEAL_MEDIUM:
                                        _obj = Object.Instantiate(Resources.Load<GameObject>("Prefabs/PowerUps/Heal(Small)"));
                                        break;
                                    case PowerUpType.POWERUP_HEAL_LARGE:
                                        _obj = Object.Instantiate(Resources.Load<GameObject>("Prefabs/PowerUps/Heal(Small)"));
                                        break;
                                    case PowerUpType.POWERUP_HEAL_FULL:
                                        _obj = Object.Instantiate(Resources.Load<GameObject>("Prefabs/PowerUps/Heal(Small)"));
                                        break;
                                    case PowerUpType.POWERUP_CHARGE_SMALL:
                                        _obj = Object.Instantiate(Resources.Load<GameObject>("Prefabs/PowerUps/Charge(Small)"));
                                        break;
                                    case PowerUpType.POWERUP_CHARGE_MEDIUM:
                                        _obj = Object.Instantiate(Resources.Load<GameObject>("Prefabs/PowerUps/Charge(Small)"));
                                        break;
                                    case PowerUpType.POWERUP_CHARGE_LARGE:
                                        _obj = Object.Instantiate(Resources.Load<GameObject>("Prefabs/PowerUps/Charge(Small)"));
                                        break;
                                    case PowerUpType.POWERUP_CHARGE_FULL:
                                        _obj = Object.Instantiate(Resources.Load<GameObject>("Prefabs/PowerUps/Charge(Small)"));
                                        break;
                                    default:
                                        break;
                                }
                                _obj.transform.position = _vPos;
                                break;
                            case LootType.LOOT_COINS:
                                _obj = Object.Instantiate(Resources.Load<GameObject>("Prefabs/PowerUps/Coins"));
                                _obj.GetComponent<PowerUp>().m_arg = System.Convert.ToInt32(Random.Range(0.8f, 1.2f) * lootList[i].m_nParam);
                                break;
                            default:
                                break;
                        }

                        GameObject _carrier = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Animation/ItemDropAnimation"));
                        _carrier.GetComponent<AnimationItemCarrier>().item = _obj;
                        _carrier.transform.position = vPosition;
                        _carrier.transform.parent = parent;
                    }
                    

                }
            }
        }
    }
}

[RequireComponent(typeof(Entity))]
public class RandomLoot : MonoBehaviour {

    public LootGroup[] lootGroups;

	// Use this for initialization
	void Start () {
        GetComponent<Transceiver>().AddResolver("Dead", Drop);
	}

    public void Drop(DSignal signal)
    {
        for(int i=0; i<lootGroups.Length; i++) {
            lootGroups[i].RollDice(transform.position, transform.parent);
        }
    }
}


