using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Charge_Small : PowerUp {

    protected override void OnEffect(GameObject player)
    {
        Entity _ety;
        if (null != player) {
            _ety = player.GetComponent<Entity>();
            if (_ety != null) {
                Transceiver.SendSignal(new DSignal(null, player, "Charge", _ety.m_Properties[Entity.MAX_EN].d_Value * 0.1f));
            }
        }
    }
}
