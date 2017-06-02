using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSaver {

    public GameSaver() { }
   public PlayerProfile LoadProfile()
    {
        string profileString = System.IO.File.ReadAllText(PlayerPrefs.GetString("savePath") + PlayerPrefs.GetString("profile") + "/profile.json");
        PlayerProfile playerProfile = JsonUtility.FromJson<PlayerProfile>(profileString);
        return playerProfile;
    }

   public void SaveProfile(PlayerProfile playerProfile)
    {
        string saveFile = (PlayerPrefs.GetString("savePath") + PlayerPrefs.GetString("profile") + "/profile.json");
        string newProfileString = JsonUtility.ToJson(playerProfile);
        System.IO.File.WriteAllText(saveFile, newProfileString);
    }
}
