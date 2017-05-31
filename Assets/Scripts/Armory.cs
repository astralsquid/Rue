using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armory : MonoBehaviour {

    public List<Weapon> weapons;
    List<WeaponCereal> weaponCereals;
	// Use this for initialization
	void Awake () {
		weapons = new List<Weapon> ();
	}
	public void AddWeapon(Weapon w)
    {
        weapons.Add(w);
    }

	// Update is called once per frame
	void Update () {
	}

    public void SaveWeapons()
    {
        BuildWeaponCereals();
        string savePath = PlayerPrefs.GetString("savePath") + PlayerPrefs.GetString("profile") + "/";
        string weaponCerealsString = JsonUtility.ToJson(weaponCereals);
        Debug.Log(savePath);
        Debug.Log(weaponCerealsString);
        System.IO.File.WriteAllText(savePath + "litter.json", weaponCerealsString);
    }
    
    void BuildWeaponCereals()
    {

    }
    public List<WeaponCereal> GetWeaponCereals()
    {
        weaponCereals = new List<WeaponCereal>();
        for (int i = 0; i < weapons.Count; i++)
        {
            WeaponCereal wc = new WeaponCereal();
            wc.Convert(weapons[i]);
            weaponCereals.Add(wc);
        }
        return weaponCereals;
    }
}