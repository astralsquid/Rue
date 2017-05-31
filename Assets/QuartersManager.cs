using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuartersManager : MonoBehaviour {
    PlayerProfile playerProfile;
    public WeaponButtonList weaponButtonList;
    // Use this for initialization
    void Start () {
        //load profile
        Debug.Log(PlayerPrefs.GetString("profilePath"));
        string profileString = System.IO.File.ReadAllText(PlayerPrefs.GetString("profilePath"));
        playerProfile = JsonUtility.FromJson<PlayerProfile>(profileString);
        for(int i = 0; i<playerProfile.litter.Count; i++)
        {
            weaponButtonList.AddWeaponButton(playerProfile.litter[i]);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}