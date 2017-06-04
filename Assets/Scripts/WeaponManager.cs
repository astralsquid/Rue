using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {
    public List<GameObject> weapon_prefabs;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public GameObject GetRandomWeapon()
    {
        //GameObject weapon = weapon_prefabs[Random.Range(0, weapon_prefabs.Count)];
        GameObject weapon = weapon_prefabs[2];
        return weapon;
    }
    public GameObject GetSpear()
    {
        GameObject weapon = weapon_prefabs[1];
        return weapon;
    }
    public GameObject GetSword()
    {
        GameObject weapon = weapon_prefabs[0];
        return weapon;
    }
}
