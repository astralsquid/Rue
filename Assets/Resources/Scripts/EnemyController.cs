using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
	public Unit unit;
	public GameObject unitObject;
	// Use this for initialization
	public GameController gameController;
	void Awake(){
		gameController = GameObject.Find ("GameController").GetComponent<GameController> ();
		unit = GameObject.Instantiate (unitObject, new Vector3 (0, 0, -1), Quaternion.identity).GetComponent<Unit>();
		unit.GetComponent<SpriteRenderer> ().color = Color.red;
	}

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		 
	}
	//reserve spaces and queue up actions
	public void DecideTurn(){

	}

	public void ClaimMove(){
		//move towards player
		int claimX = unit.cordX;
		int claimY = unit.cordY;

	}

	public void TakeTurn(){
		if (unit.alive) {
			unit.MoveRandom ();
		}
	}
}
