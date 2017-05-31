using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerProfile {
    public int level;
	public UnitCereal myUnit;
    public List<WeaponCereal> litter;
    public PlayerProfile()
    {
        level = 0;
        myUnit = new UnitCereal();
        litter = new List<WeaponCereal>();
    }
    public PlayerProfile(Unit unit, List<WeaponCereal> weaponCereals)
    {
        level = 0;
        myUnit = new UnitCereal(unit);
        litter = weaponCereals;
    }
    public void PrintInfo()
    {
        Debug.Log(level);
    }
}
