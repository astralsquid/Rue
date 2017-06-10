using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*	Creates / Stores World
 *  Handles Enemy Movement
 *  Handles Events
 * */

public class GameController : MonoBehaviour {
	//optional components
	public ReflexManager reflexManager;

	//settings
	public float enemyMovementSpacing;
	public float runTurnDelay;
    public Color tile_color_bound_1;
    public Color tile_color_bound_2;
    public Armory armory;
	public bool elevator_online;
	public int spawn_x;
	public int spawn_y;

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
    public WeaponManager weaponManager;
	public ScreenFader screenFader;

	//Meta Info
	public int currentLevel;
	public int maxLevel;
	public int daysLeft;
	public int enemies;

	//Level Info
	public int levelHeight;
	public int levelWidth;
    public enum LevelMode {Arena, Quarters, Intro};
    LevelMode levelMode;

    //win info
    public bool level_won = false;
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



    }

	void Start () {
		unitList = new List<Unit> ();
		elevator_called = false;
		enemyControllers = new List<EnemyController> ();
		targetGrid = new int[0];
		unitGrid = new Unit[0];
		occupationGrid = new int[0];
		tileGrid = new GameObject[0];
		ChangeLevel(LevelType.Fields, levelWidth, levelHeight);

		//optional features
		LowerElevator(true);
		StopReflex ();
		FadeIn ();

	}

	void FadeIn(){
		if (screenFader != null) {
			screenFader.FadeFromBlack ();
		}
	}
	void FadeOut(){
		if (screenFader != null) {
			screenFader.FadeToBlack ();
		}
	}

    void LoadPlayerProfile()
    {

        GameSaver gs = new GameSaver();
        PlayerProfile playerProfile = gs.LoadProfile();
        playerUnit.SetColor(playerProfile.myUnit.myColor);
        playerUnit.SetName(playerProfile.myUnit.name);
        playerUnit.SetWish(playerProfile.myUnit.wish);
        playerUnit.LoadWeapon(playerProfile.myUnit.weapon);
    }
    //dishes out weapons to everyone in the arena
    void DishOutWeapons()
    {
        for(int i = 0; i<enemyControllers.Count; i++)
        {
            GameObject primary_weapon_object = GameObject.Find("WeaponManager").GetComponent<WeaponManager>().GetRandomWeapon();
            enemyControllers[i].unit.primaryWeapon = GameObject.Instantiate(primary_weapon_object, new Vector3(enemyControllers[i].unit.transform.position.x, enemyControllers[i].unit.transform.position.y, 0), Quaternion.identity).GetComponent<Weapon>();
            enemyControllers[i].unit.primaryWeapon.owner = enemyControllers[i].unit;
            enemyControllers[i].unit.primaryWeapon.transform.parent = enemyControllers[i].unit.transform;
        }
    }
    void Save()
    {
        SavePlayerProfile();
    }
    void SavePlayerProfile()
    {
        PlayerProfile playerProfile = new PlayerProfile(playerUnit, armory.GetWeaponCereals());
        Debug.Log("weapon type" + playerUnit.primaryWeapon.GetType());
        GameSaver gs = new GameSaver();
        gs.SaveProfile(playerProfile);
    }

	// Update is called once per frame
	void Update () {
	
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
        LoadPlayerProfile();


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
		playerUnit.GetComponent<Unit>().Teleport(spawn_x,spawn_y, false);

		//spawn enemies
		List<Vector2> coords = new List<Vector2> ();
		for (int w = 0; w < width; w++) {
			for (int h = 0; h < height; h++) {
				if (w != width / 2 || h != height / 2 && w !=( width / 2) + 1 && w != (width / 2) - 1 && h != (height / 2) + 1 && h != (height / 2) + - 1) {
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
            enemyControllers[enemyControllers.Count - 1].unit.GrantRandomWeapon();
            enemyControllers[enemyControllers.Count - 1].unit.Teleport((int)coords[i].x, (int)coords[i].y, false);
        }



    }


	public IEnumerator RunTurn(){
		ClaimEnemyMoves ();

		//take enemy moves
		for (int i = 0; i < enemyControllers.Count; i++) {
			playerInputController.DisableInput();
			yield return new WaitForSeconds(enemyMovementSpacing);
			enemyControllers[i].TakeTurn();
			playerInputController.EnableInput();
		}
		CheckDeath ();

		CheckLevelWon ();

        if (level_won && !elevator_called)
        {
			Debug.Log ("wow");
            elevator_called = true;
			StopReflex ();
			LowerElevator (false);
        }
			
		EndLevelIfNecessary ();

		//optional features
		ResetReflex();
    }

	public void ResetReflex(){
		if (reflexManager != null) {
			reflexManager.ResetReflex ();
		}
	}

	void EndLevelIfNecessary(){
		if (elevator != null) {
			if (playerUnit.cordX == levelWidth / 2 && playerUnit.cordY == levelHeight / 2 && level_won && elevator.elevator_lowered) {
				StartCoroutine (EndLevel ());
			}
		}
	}
	void CheckLevelWon(){
		level_won = true;
		for (int i = 0; i < enemyControllers.Count; i++)
		{
			if (enemyControllers[i].unit.alive)
			{
				level_won = false;
			}
		}
	}
	void CheckDeath(){
		if (!playerUnit.alive)
		{
			SceneManager.LoadScene("DeathScene");
		}
	}
	public void ClaimEnemyMoves(){
		for (int i = 0; i < enemyControllers.Count; i++) {
			enemyControllers [i].ClaimMove();
		}
	}

    IEnumerator EndLevel()
    {
		StopReflex (); //optional feature
		if (elevator != null) {
			StartCoroutine (elevator.Raise (true));
		}
		Save();
        yield return new WaitForSeconds(1);
		SceneManager.LoadScene("Quarters");

    }
		
	void StopReflex(){
		if (reflexManager != null) {
			reflexManager.StopReflex ();
		}
	}
	public void RaiseElevator(bool b){
		if (elevator_online) {
			StartCoroutine (elevator.Raise (b));
		}
	}
	public void LowerElevator(bool b){
		if (elevator_online) {
			StartCoroutine (elevator.Lower (b));
		}
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
    public GameObject GetTile(int x, int y)
    {
        return tileGrid[y * levelWidth + x];
    }
	public void SetTileColor (int x, int y, Color c){
		tileGrid [y * levelWidth + x].GetComponent<SpriteRenderer> ().color = c;
	}
    public GameObject GetMiddleTile()
    {
        return tileGrid[levelWidth/2 * levelHeight + levelHeight/2];
    }
}