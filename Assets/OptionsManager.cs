using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour {
	public Scrollbar musicBar;
	public Scrollbar effectsBar;
	public Text musicPercentage;
	public Text effectsPercentage;

	// Use this for initialization
	void Start () {
		if (PlayerPrefs.GetFloat ("music")==0) {
			PlayerPrefs.SetFloat ("music", .5f);
		}
		if (PlayerPrefs.GetFloat ("effects")==0) {
			PlayerPrefs.SetFloat ("effects", .5f);
		}
		musicBar.value = PlayerPrefs.GetFloat ("music");
		effectsBar.value = PlayerPrefs.GetFloat ("effects");
	}

	public void SaveSettings(){
		if (musicBar.value == 0) {
			PlayerPrefs.SetFloat ("music", -1);
		} else {
			PlayerPrefs.SetFloat ("music", musicBar.value);
		}

		if (effectsBar.value == 0) {
			PlayerPrefs.SetFloat ("effects", -1);
		} else {
			PlayerPrefs.SetFloat ("effects", effectsBar.value);
		}


	}

	// Update is called once per frame
	void Update () {
		musicPercentage.text = ((int)(musicBar.value * 100)).ToString() + "%";
		effectsPercentage.text = ((int)(effectsBar.value * 100)).ToString() + "%";

		//int b = (effectsBar.value * 100);
		//effectsPercentage.text = b.ToString + "%";
	}
}
