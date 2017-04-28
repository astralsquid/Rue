using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/*	Creates / Stores World
 *  Handles Enemy Movement
 *  Handles Events
 * */
public class GameController : MonoBehaviour {
	//settings
	public float enemyMovementSpacing;
	public float runTurnDelay;

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

	//Storage
	public GameObject[] tileGrid;
	public Unit[] unitGrid;
	public int[] occupationGrid;
	public int[] targetGrid;
	public List<EnemyController> enemyControllers;
	public Unit playerUnit;
	public List<Unit> unitList;

    void Awake(){
		unitList = new List<Unit> ();

	}

	void Start () {
		enemyControllers = new List<EnemyController> ();
		targetGrid = new int[0];
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
                float tone = UnityEngine.Random.Range(0f, .05f);
                spawnedTile.GetComponent<SpriteRenderer>().color = new Color(tone, tone, tone, .5f);
                tileGrid[y * width + x] = spawnedTile;
            }
        }

    }
	public IEnumerator RunTurn(){

		for (int i = 0; i < enemyControllers.Count; i++) {
			enemyControllers [i].ClaimMove();
		}
		for (int i = 0; i < enemyControllers.Count; i++) {
			playerInputController.inputEnabled = false;
			yield return new WaitForSeconds(enemyMovementSpacing);
			enemyControllers [i].TakeTurn ();
			playerInputController.inputEnabled = true;
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

    public IEnumerator MoveToPosition(Transform transform, Vector3 position, float timeToMove)
    {
        playerInputController.inputEnabled = false;
        var currentPos = transform.position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
        playerInputController.inputEnabled = true;

    }
}