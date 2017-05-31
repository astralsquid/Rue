using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour {
    public Text nameText;
    public Text wishText;
    public Image characterImage;

    public GameObject namePanel;
    public GameObject wishPanel;
    public GameObject customizationPanel;
    public GameObject mainPanel;

    public Slider rSlider;
    public Slider gSlider;
    public Slider bSlider;

    string savePath;

    Vector3 dumpPosition;
    Vector3 centerPosition = new Vector3(0, 0, 0);
    Color myColor;

	// Use this for initialization
	void Start () {
        PlayerPrefs.SetString("savePath", Application.dataPath + "/Saves/");
        myColor = new Color(255, 255, 255);
        dumpPosition = new Vector3(1000, 1000, 1000);
	}
	
	// Update is called once per frame
	void Update () {
        UpdateSpriteColor();
    }
    void UpdateSpriteColor()
    {
        myColor = new Color(rSlider.value, gSlider.value, bSlider.value);
        characterImage.color = myColor;
    }

    void HideAllPanels()
    {
        namePanel.transform.position = dumpPosition;
        wishPanel.transform.position = dumpPosition;
        customizationPanel.transform.position = dumpPosition;
        mainPanel.transform.position = dumpPosition;
    }
    void ShowPanel(GameObject panel)
    {
        panel.transform.position = centerPosition;
    }
    public void ShowMainPanel()
    {
        HideAllPanels();
        ShowPanel(mainPanel);
    }
    public void ShowNamePanel()
    {
        HideAllPanels();
        ShowPanel(namePanel);
    }
    public void ShowWishPanel()
    {
        HideAllPanels();
        ShowPanel(wishPanel);
    }
    public void ShowCustomizationPanel()
    {
        HideAllPanels();
        ShowPanel(customizationPanel);
    }
    public void StartGame()
    {
        //load all info into file, switch to intro scene
        CreateProfile();
        SceneManager.LoadScene("Arena");
    }
    void CreateProfile()
    {
        PlayerProfile newProfile = new PlayerProfile();
        newProfile.myUnit.name = wishText.text;
        newProfile.myUnit.wish = nameText.text;
        newProfile.myUnit.myColor = myColor;
        PlayerPrefs.SetString("profile", nameText.text);
        savePath = PlayerPrefs.GetString("savePath") + PlayerPrefs.GetString("profile");
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
            string newProfileString = JsonUtility.ToJson(newProfile);
            System.IO.File.WriteAllText(savePath + "/profile.json", newProfileString);
            PlayerPrefs.SetString("profilePath", savePath + "/profile.json");
            //string profilePath = PlayerPrefs.GetString("savePath") + PlayerPrefs.GetString("profile") + "/profile.json";
           // PlayerPrefs.SetString("profilePath", savePath);
        }
        else
        {
            //idk wave a finger or something
        }
    }
}