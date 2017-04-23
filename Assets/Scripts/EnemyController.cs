using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
	public Unit unit;
	public GameObject unitObject;
	public bool attacking;
	public int claimX;
	public int claimY;
	int dodge;
	// Use this for initialization
	public GameController gameController;
	void Awake(){
		gameController = GameObject.Find ("GameController").GetComponent<GameController> ();
		unit = GameObject.Instantiate (unitObject, new Vector3 (0, 0, -1), Quaternion.identity).GetComponent<Unit>();
		dodge = 50;
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
		if (!unit.alive) {
			gameController.enemyControllers.Remove (this);
		} else {
            int rand = Random.Range(0, 100);

            //move towards nearest unit
            //Debug.Log("Claiming Move...");
            claimX = unit.cordX;
			claimY = unit.cordY;

			int nearestDistance = 1000;
			int nui = 0; //nearest unit index

			//search for nearest unit
			Debug.Log (gameController.unitList.Count);
			for (int i = 0; i < gameController.unitList.Count; i++) {
				int dist = gameController.GetDistanceFromUnit (i, unit.cordX, unit.cordY);
				if (dist < nearestDistance && dist != 0) {
					nearestDistance = dist;
					nui = i;
				} else if (dist == nearestDistance) {
					if (rand < 50) {
						nui = i;
					}
				}
			}

            //if nearest unit is in range, attack
            if (nearestDistance == unit.primaryWeapon.range) { //needs a bandaid
                if (rand < 50) {
					attacking = true;
					Debug.Log ("perfect range");
				}
			} else if (nearestDistance < unit.primaryWeapon.range + 1) { //needs a bandaid
				if (rand < 40) {
					attacking = true;
					Debug.Log ("+1 range");

				}
			} else if (nearestDistance < unit.primaryWeapon.range + 2) { //needs a bandaid
				if (rand < 30) {
					attacking = true;
					Debug.Log ("+2 range");

				}
			} else if (nearestDistance < unit.primaryWeapon.range + 3) { //needs a bandaid
				if (rand < 20) {
					attacking = true;
					Debug.Log ("+3 range");

				}
			} 
						
			if (!attacking) {		//else move into range
				//Debug.Log("picking out move " + nearestDistance);
				if (gameController.unitList [nui].cordX > unit.cordX) {
					if (gameController.GetOccupation (claimX + 1, claimY) == 0) {
						claimX = claimX + 1;
					}
				} else if (gameController.unitList [nui].cordX < unit.cordX) {
					if (gameController.GetOccupation (claimX - 1, claimY) == 0) {
						claimX = claimX - 1;
					}
				}
				if (gameController.unitList [nui].cordY > unit.cordY) {
					if (gameController.GetOccupation (claimX, claimY + 1) == 0) {
						claimY = claimY + 1;
					}
				} else if (gameController.unitList [nui].cordY < unit.cordY) {
					if (gameController.GetOccupation (claimX, claimY - 1) == 0) {
						claimY = claimY - 1;
					}
				}

			} else if (gameController.GetTarget (unit.cordX, unit.cordY) > 0) {
				ClaimRandomMove ();
			}
		}
	}


	public void TakeTurn(){
		if (unit.alive) {
			if (!attacking) {
				unit.Move (claimX, claimY, false);
			} else {
				unit.primaryWeapon.Attack ();
				if (unit.aiming) {
					unit.primaryWeapon.lockTarget ();
				}
				attacking = false;
			}
		}
	}

	public void ClaimRandomMove(){
		int rand3 = Random.Range (0, 101);
		int ylower = 0;
		int yupper = 1;
		int xlower = 0;
		int xupper = 1;
		if (rand3 < dodge) {
			//gameController.SetOccupation (unit.cordX, unit.cordY, 0);
		
			attacking = false;
			//move out the way!
			if(claimY + 1< gameController.GetLevelHeight() && gameController.GetOccupation(claimX, claimY + 1) == 0){
				yupper = 2;
			}
			if(claimY - 1 > 0 && gameController.GetOccupation(claimX, claimY - 1) == 0){
				ylower = -1;
			}
			if(claimX + 1 < gameController.GetLevelWidth() && gameController.GetOccupation(claimX + 1, claimY) == 0  ){
				xupper = 2;
			}
			if(claimX - 1 > 0 && gameController.GetOccupation(claimX - 1, claimY) == 0){
				xlower = -1;
			}
			claimX = Random.Range (unit.cordX + xlower, unit.cordX + xupper);
			claimY = Random.Range (unit.cordY + ylower, unit.cordY + yupper);
		}
	}
}
