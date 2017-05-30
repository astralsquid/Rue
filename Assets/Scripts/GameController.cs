using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

/*	Creates / Stores World
 *  Handles Enemy Movement
 *  Handles Events
 * */

public class GameController : MonoBehaviour {
	//settings
	public float enemyMovementSpacing;
	public float runTurnDelay;
    public Color tile_color_bound_1;
    public Color tile_color_bound_2;

    //reflex
    public float reflex = 3.0f;
    float current_reflex = 3.0f;
    public GameObject reflex_bar;
    float reflex_bar_max_width;
    float reflex_bar_max_height;
    bool reflex_stopped;
    public Color reflex_color;

    //elevator
    public Elevator elevator;

    //Enums
    enum Direction {North, South, East, West};
	public enum LevelType {Lava, Snow, Marsh, Desert, Fields};

    public PlayerInputController playerInputController;

	//Objects
	public GameObject tile;
	public GameObject exit;

	public GameObject enemyController;

	//Meta Info
	public int currentLevel;
	public int maxLevel;
	public int daysLeft;
	public int enemies;

	//Level Info
	public int levelHeight;
	public int levelWidth;

    //win info
    bool level_won = false;
    bool elevator_called;

	//Storage
	public GameObject[] tileGrid;
	public Unit[] unitGrid;
	public int[] occupationGrid;
	public int[] targetGrid;
	public List<EnemyController> enemyControllers;
	public Unit playerUnit;
	public List<Unit> unitList;

    void Awake(){
        reflex_stopped = true;
		unitList = new List<Unit> ();
        reflex_bar_max_height = reflex_bar.GetComponent<RectTransform>().sizeDelta.y;
        reflex_bar_max_width = reflex_bar.GetComponent<RectTransform>().sizeDelta.x;
        //reflex_bar.transform.position = new Vector3(0, Screen.height, -9);
    }

	void Start () {
		enemyControllers = new List<EnemyController> ();
		targetGrid = new int[0];
		unitGrid = new Unit[0];
		occupationGrid = new int[0];
		tileGrid = new GameObject[0];
		ChangeLevel (LevelType.Fields, levelWidth, levelHeight);
        LoadPlayerProfile();
        StartCoroutine(elevator.Lower(true));
	}

    void LoadPlayerProfile()
    {
        string profileString = System.IO.File.ReadAllText(PlayerPrefs.GetString("savePath") + PlayerPrefs.GetString("profile") + "/profile.json");
        PlayerProfile playerProfile = JsonUtility.FromJson<PlayerProfile>(profileString);
        playerUnit.SetColor(playerProfile.myUnit.myColor);
        playerUnit.SetName(playerProfile.myUnit.name);
        playerUnit.SetWish(playerProfile.myUnit.wish);
        Debug.Log(profileString);
    }
    void SavePlayerProfile()
    {
        
    }

	// Update is called once per frame
	void Update () {
        TickReflex();
	}
	public void ChangeLevel(LevelType type, int width, int height){
		//set width and height
		levelHeight = height;
		levelWidth = width;

		//destroy old level
		for (int i = 0; i < tileGrid.Length; ++i) {
			Destroy (tileGrid [i]);
			Destroy (unitGrid [i]);
		}
		//create new grid
		targetGrid = new int[width * height];
		unitGrid = new Unit[width*height];
		tileGrid = new GameObject[width*height];
        occupationGrid = new int[width * height];

        //spawn player
        playerUnit.GetComponent<Unit>().Move(width/2,height/2, false);

		//spawn enemies
		List<Vector2> coords = new List<Vector2> ();
		for (int w = 0; w < width; w++) {
			for (int h = 0; h < height; h++) {
				if (w != width / 2 || h != height / 2) {
					coords.Add (new Vector2 (w, h));
				}
			}
		}

		//shuffle list
		for (int i = 0; i < coords.Count; i++)
		{
			Vector3 temp = coords[i];
			int randomIndex = UnityEngine.Random.Range(0, coords.Count);
			coords[i] = coords[randomIndex];
			coords[randomIndex] = temp;
		}

		//place enemies
		for (int i = 0; i < enemies; i++) {
			GameObject e = GameObject.Instantiate (enemyController, new Vector3 (0, 0, 0), Quaternion.identity);
			enemyControllers.Add (e.GetComponent<EnemyController>());
			enemyControllers[enemyControllers.Count-1].unit.Move ((int)coords [i].x, (int)coords [i].y, false);
		}

        //spawn tiles
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject spawnedTile = Instantiate(tile, new Vector3(x - (width) / 2f, y - (height) / 2f, 0), Quaternion.identity) as GameObject;
                spawnedTile.GetComponent<SpriteRenderer>().color = Color.Lerp(tile_color_bound_1, tile_color_bound_2, UnityEngine.Random.Range(0.0f, 1.0f));
                tileGrid[y * width + x] = spawnedTile;
            }
        }

    }


	public IEnumerator RunTurn(){
        level_won = true;
		for (int i = 0; i < enemyControllers.Count; i++) {
			enemyControllers [i].ClaimMove();
		}
		for (int i = 0; i < enemyControllers.Count; i++) {
            playerInputController.DisableInput();
			yield return new WaitForSeconds(enemyMovementSpacing);
            enemyControllers[i].TakeTurn();
            playerInputController.EnableInput();
		}
        for (int i = 0; i < enemyControllers.Count; i++)
        {
            if (enemyControllers[i].unit.alive)
            {
                level_won = false;
            }
        }

        if (level_won && !elevator_called)
        {
            elevator_called = true;
            StartCoroutine(elevator.Lower(false));
        }

        if(playerUnit.cordX == levelWidth/2 && playerUnit.cordY == levelHeight/2 && level_won && elevator.elevator_lowered)
        {
            StartCoroutine(EndLevel());
        }
    }

    IEnumerator EndLevel()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(elevator.Raise(true));
    }
		
	public void AddTarget(int x, int y){
		targetGrid [y * levelWidth + x] += 1;
	}
	public void SubTarget(int x, int y){
		targetGrid [y * levelWidth + x] -= 1;
	}
	public int GetTarget(int x, int y){
		return targetGrid [y * levelWidth + x];
	}
	public int GetLevelHeight(){
		return levelHeight;
	}
	public int GetLevelWidth(){
		return levelWidth;
	}
	public Unit GetUnit(int x, int y){
		return unitGrid [y * levelWidth + x];
	}
	public void SetUnit(int x, int y, Unit u){
		unitGrid [y * levelWidth + x] = u;
	}
	public void RemoveUnit(int x, int y){
		unitGrid [y * levelWidth + x] = null;
	}
	public int GetOccupation(int x, int y){
        if (x >= 0 && x < levelWidth && y >= 0 && y < levelHeight) {
            return occupationGrid[y * levelWidth + x];
        }
        return 1;
	}
	public void SetOccupation(int x, int y, int v){
		occupationGrid [y * levelWidth + x] = v;	
	}
	public int GetDistanceFromUnit(int i, int x, int y){
		return Mathf.Max(Mathf.Abs (unitList [i].cordX - x) + Mathf.Abs (unitList [i].cordY - y));
	}

    void TickReflex()
    {
        if (!reflex_stopped)
        {
            current_reflex -= Time.deltaTime;
            if (current_reflex > 0)
            {
                reflex_bar.GetComponent<RectTransform>().sizeDelta = new Vector2(reflex_bar_max_width * (current_reflex / reflex), reflex_bar_max_height);
            }
            else
            {
                current_reflex = reflex;
                if (reflex > .05f)
                {
                    reflex = 0.9f * reflex;
                }
                reflex_bar.GetComponent<RectTransform>().sizeDelta = new Vector2(reflex_bar_max_width, reflex_bar_max_height);
                StartCoroutine(RunTurn());
                ResetReflex();
            }
        }
    }

    public void StopReflex()
    {
        current_reflex = reflex;
        reflex_stopped = true;
        reflex_bar.GetComponent<Image>().color = new Color(0, 0, 0, 0);
    }

    public void ResetReflex()
    {
        
        current_reflex = reflex;
        if (!level_won)
        {
            reflex_stopped = false;
            reflex_bar.GetComponent<Image>().color = reflex_color;
        }
    }
    public GameObject GetTile(int x, int y)
    {
        return tileGrid[y * levelWidth + x];
    }
    public GameObject GetMiddleTile()
    {
        return tileGrid[levelWidth/2 * levelWidth + levelHeight/2];
    }
}