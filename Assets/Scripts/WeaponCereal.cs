using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponCereal {
    public Weapon.WeaponType myType;
    public string name;

    public WeaponCereal() {
        myType = 0;
        name = "weapon";
    }
    public WeaponCereal(Weapon w)
    {
        myType = w.GetMyType();
        name = w.name;
    }
    public void Convert(Weapon w)
    {
        myType = w.GetMyType();
        name = w.name;
    } 
}
