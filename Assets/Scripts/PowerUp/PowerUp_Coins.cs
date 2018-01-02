using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Coins : PowerUp {

    //public int m_Coins = 1;

    protected override void OnEffect(GameObject player)
    {
        Inventory _inv = player.GetComponent<Inventory>();

        if(_inv != null) {
            _inv.m_Coins += System.Convert.ToInt32(m_arg);
        }
    }

}
