using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/*	Creates / Stores World
 *  Handles Enemy Movement
 *  Handles Events
 * */
public class GameController : MonoBehaviour {
	//Enums
	enum Direction {North, South, East, West};
	public enum LevelType {Lava, Snow, Marsh, Desert, Fields};

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

	//Storage
	public GameObject[] tileGrid;
	public Unit[] unitGrid;
	public int[] occupationGrid;
	List<EnemyController> enemyControllers;
	public Unit playerUnit;

    void Awake(){

	}

	void Start () {
		enemyControllers = new List<EnemyController> ();
		unitGrid = new Unit[0];
		occupationGrid = new int[0];
		tileGrid = new GameObject[0];
		ChangeLevel (LevelType.Fields, levelWidth, levelHeight);
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
		unitGrid = new Unit[width*height];
		tileGrid = new GameObject[width*height];
        occupationGrid = new int[width * height];


        //spawn player
        playerUnit.GetComponent<Unit>().Move(width/2,height/2);

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

		for (int i = 0; i < enemies; i++) {
			GameObject e = GameObject.Instantiate (enemyController, new Vector3 (0, 0, 0), Quaternion.identity);
			enemyControllers.Add (e.GetComponent<EnemyController>());
			enemyControllers[enemyControllers.Count-1].unit.Move ((int)coords [i].x, (int)coords [i].y);
			Debug.Log (coords [i].x + "," + coords [i].y);
		}

        //spawn tiles
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject spawnedTile = Instantiate(tile, new Vector3(x - (width) / 2f, y - (height) / 2f, 0), Quaternion.identity) as GameObject;
                float tone = UnityEngine.Random.Range(0f, .05f);
                spawnedTile.GetComponent<SpriteRenderer>().color = new Color(tone, tone, tone, .5f);
                tileGrid[y * width + x] = spawnedTile;
            }
        }

    }
	public void RunTurn(){
		for (int i = 0; i < enemyControllers.Count; i++) {
			//enemyControllers [i];
		}
		for (int i = 0; i < enemyControllers.Count; i++) {
			enemyControllers [i].TakeTurn ();
		}
	}
		


	public int GetLevelHeight(){
		return levelHeight;
	}
	public int GetLevelWidth(){
		return levelWidth;
	}
}