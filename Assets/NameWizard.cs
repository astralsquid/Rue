using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameWizard : MonoBehaviour
{
    string[] villageNounArr;
    string[] namesArr;
    string[] adjectiveArr;
    string[] nounArr;
    string[] maleNamesArr;
    string[] femaleNamesArr;
    string[] lastNamesArr;


    void Awake()
    {
        loadWordLists();
    }
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public string RandomNoun()
    {
        return nounArr[Random.Range(0, nounArr.Length)];
    }
    public string RandomName()
    {
        return namesArr[Random.Range(0, namesArr.Length)];
    }
    public string RandomAdjective()
    {
        return adjectiveArr[Random.Range(0, adjectiveArr.Length)];
    }
    public string RandomVillageNoun()
    {
        return villageNounArr[Random.Range(0, villageNounArr.Length)];
    }
    public string RandomMaleName()
    {
        return maleNamesArr[Random.Range(0, maleNamesArr.Length)];
    }
    public string RandomFemaleName()
    {
        return femaleNamesArr[Random.Range(0, femaleNamesArr.Length)];
    }
    public string RandomLastName()
    {
        return lastNamesArr[Random.Range(0, lastNamesArr.Length)];
    }

    private void loadWordLists()
    {
        TextAsset villageNounsAsset = Resources.Load("Words/village_nouns") as TextAsset;
        TextAsset nounsAsset = Resources.Load("Words/nouns") as TextAsset;
        TextAsset adjectivesAsset = Resources.Load("Words/adj") as TextAsset;
        TextAsset namesAsset = Resources.Load("Words/all_names") as TextAsset;
        TextAsset maleNamesAsset = Resources.Load("Words/male_names") as TextAsset;
        TextAsset femaleNamesAsset = Resources.Load("Words/female_names") as TextAsset;
        TextAsset lastNamesAsset = Resources.Load("Words/last_names") as TextAsset;

        char[] archDelim = new char[] { '\r', '\n' };

        villageNounArr = villageNounsAsset.text.Split(archDelim, System.StringSplitOptions.RemoveEmptyEntries);
        nounArr = nounsAsset.text.Split(archDelim, System.StringSplitOptions.RemoveEmptyEntries);
        adjectiveArr = adjectivesAsset.text.Split(archDelim, System.StringSplitOptions.RemoveEmptyEntries);
        namesArr = namesAsset.text.Split(archDelim, System.StringSplitOptions.RemoveEmptyEntries);
        maleNamesArr = maleNamesAsset.text.Split(archDelim, System.StringSplitOptions.RemoveEmptyEntries);
        femaleNamesArr = femaleNamesAsset.text.Split(archDelim, System.StringSplitOptions.RemoveEmptyEntries);
        lastNamesArr = lastNamesAsset.text.Split(archDelim, System.StringSplitOptions.RemoveEmptyEntries);
    }

    private void destroyWordLists()
    {

        namesArr = new string[0];
        nounArr = new string[0];
        adjectiveArr = new string[0];
        villageNounArr = new string[0];
        maleNamesArr = new string[0];
        femaleNamesArr = new string[0];
        lastNamesArr = new string[0];
        Resources.UnloadUnusedAssets();
    }

    public string GenerateCityName()
    {
        int f = Random.Range(0, 5);
        string name = "";
        switch (f)
        {
            case 0:
                name = RandomNoun() + " " + RandomVillageNoun();
                break;
            case 1:
                name = RandomName();
                break;
            case 2:
                name = RandomVillageNoun() + " of " + RandomName();
                break;
            case 3:
                name = RandomVillageNoun() + " of " + RandomNoun();
                break;
            case 4:
                name = RandomAdjective() + " " + RandomVillageNoun();
                break;
            default:
                break;
        }
        return name;
    }
    public string GenerateForestName()
    {
        int f = Random.Range(0, 5);
        string name = "";
        return name;
    }
}
