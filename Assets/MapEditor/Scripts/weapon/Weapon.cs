using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///所有武器的基类
/// </summary>
public abstract class Weapon : MonoBehaviour {

    public abstract void UseWeapon();

    public abstract void LoadWeapon(HeroEquipment hero);

    public abstract void UnLoadWeapon(HeroEquipment hero);
    
}
