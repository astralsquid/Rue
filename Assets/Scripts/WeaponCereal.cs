using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponCereal {
    public Weapon.WeaponType myType;
    public string name;
	public int range;
	public int damage;

    public WeaponCereal() {
        myType = 0;
        name = "sword";
        range = 1;
        damage = 1;
    }
    public WeaponCereal(Weapon w)
    {
        myType = w.GetMyType();
        name = w.name;
        range = w.range;
        damage = w.damage;
    }
    public void Convert(Weapon w)
    {
        myType = w.GetMyType();
        name = w.name;
		range = w.range;
		damage = w.damage;
    } 
}
