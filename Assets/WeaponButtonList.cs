using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponButtonList : MonoBehaviour {

    int current_index;
    public List<WeaponCereal> weaponCereals;
    public GameObject elementGrid;
    public GameObject weaponSelectButtonObject;
    public int selection_index = 0;

	// Use this for initialization
	void Start () {
        current_index = 0;
        weaponCereals = new List<WeaponCereal>();
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
                Debug.Log("wow");
                break;
            case (Weapon.WeaponType.spear):
            default:
                Debug.Log("nada");
                break;
        }
    }
}
