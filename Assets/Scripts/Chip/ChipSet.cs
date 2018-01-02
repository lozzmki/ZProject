using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipSet {

    #region Properties
    int m_nCurrentCost;
    int m_nCapacity;
    #endregion

    #region Runtime
    List<GameObject> m_Chipset;
    #endregion

    public ChipSet()
    {
        m_Chipset = new List<GameObject>();
        m_nCurrentCost = 0;
        m_nCapacity = 100;
    }

    public bool AddChip(GameObject chip)
    {
        Item _it = chip.GetComponent<Item>();
        if (_it != null) {
            if (m_nCapacity >= _it.m_nChipCost + m_nCurrentCost) {
                m_Chipset.Add(chip);
                m_nCurrentCost += _it.m_nChipCost;
                return true;
            }
        }
        return false;
    }

    public List<GameObject> GetChipset(){
        return m_Chipset;
    }

    public bool RemoveChip(GameObject chip)
    {
        return m_Chipset.Remove(chip);
    }
}
