using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerProfile {
    public int level;
	public UnitCereal myUnit;
    public PlayerProfile()
    {
        level = 0;
        myUnit = new UnitCereal();
    }
}
