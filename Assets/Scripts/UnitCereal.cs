using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitCereal{
    public string name;
    public string wish;
    public int age;
    public Color myColor;
    public WeaponCereal weapon;
    public UnitCereal()
    {
        name = "boi";
        wish = "I wish I wish with all my heart to fly with dragons in a land apart.";
        age = 0;
        myColor = new Color(1, 1, 1);
        weapon = new WeaponCereal();
    }
    public UnitCereal(Unit unit)
    {
        name = unit.name;
        wish = unit.wish;
        age = unit.age;
        myColor = unit.my_color;
        weapon = new WeaponCereal(unit.primaryWeapon);
    }
    public void Convert(Unit unit)
    {
        name = unit.name;
        wish = unit.wish;
        age = unit.age;
        myColor = unit.my_color;
        weapon = new WeaponCereal(unit.primaryWeapon);
    }
}