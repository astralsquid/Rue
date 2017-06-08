using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WeaponButtonList : MonoBehaviour {
    PlayerProfile playerProfile;

    int current_index;
    List<WeaponCereal> weaponCereals;
    public GameObject elementGrid;
    public GameObject weaponSelectButtonObject;
    public int selection_index = 0;

    //Display info
    public Text weaponDamage;
    public Text weaponRange;
    public Text weaponName;

    public Text myWeaponDamage;
    public Text myWeaponRange;
    public Text myWeaponName;

    int view;
    int selection;

    // Use this for initialization
    void Awake () {
        view = 0;
        current_index = 0;
        weaponCereals = new List<WeaponCereal>();
		Debug.Log(PlayerPrefs.GetString("profilePath"));
        string profileString = System.IO.File.ReadAllText(PlayerPrefs.GetString("profilePath"));
        playerProfile = JsonUtility.FromJson<PlayerProfile>(profileString);
        //AddWeaponButton(playerProfile.myUnit.weapon);
        for (int i = 0; i < playerProfile.litter.Count; i++)
        {
            AddWeaponButton(playerProfile.litter[i]);
        }
        DisplayWeaponInfo(0);
        PickupWeapon();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void AddWeaponButton(WeaponCereal wc)
    {
        GameObject newButton = Instantiate(weaponSelectButtonObject, new Vector3(0, 0, 0), Quaternion.identity);
        newButton.transform.SetParent(elementGrid.transform);
        newButton.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
        Text button_text = newButton.transform.Find("Text").GetComponent<Text>();
        button_text.text = wc.name;

        newButton.GetComponent<WeaponSelectButton>().index = current_index;
        current_index += 1;
        weaponCereals.Add(wc);

        switch (wc.myType)
        {
            case (Weapon.WeaponType.sword):
                //sprites
                break;
            case (Weapon.WeaponType.spear):
                //sprites
            default:
                //Debug.Log("nada");
                break;
        }
    }

    public void DisplayWeaponInfo(int i)
    {
        Debug.Log(i);
        Debug.Log(weaponCereals.Count);
        weaponName.text = weaponCereals[i].name;
        weaponDamage.text = weaponCereals[i].damage.ToString();
        weaponRange.text = weaponCereals[i].range.ToString();
        view = i;
    }
    public void PickupWeapon()
    {
        myWeaponName.text = weaponCereals[view].name;
        myWeaponDamage.text = weaponCereals[view].damage.ToString();
        myWeaponRange.text = weaponCereals[view].range.ToString();
        selection = view;
    }
    public void FinalizeSelection()
    {
        playerProfile.myUnit.weapon = weaponCereals[selection];
        playerProfile.litter = new List<WeaponCereal>();
        GameSaver gs = new GameSaver();
        gs.SaveProfile(playerProfile);
        SceneManager.LoadScene("Arena");
    }
}
