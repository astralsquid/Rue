using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : Weapon {

	// Use this for initialization
	void Start () {
        base.Start();
        myType = WeaponType.spear;
    }
	
	// Update is called once per frame
	void Update () {
        base.Update();
	}
}
