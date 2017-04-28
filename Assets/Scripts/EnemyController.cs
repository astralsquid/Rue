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

	Unit myTarget;

	// Use this for initialization
	public GameController gameController;
	void Awake(){
		gameController = GameObject.Find ("GameController").GetComponent<GameController> ();
		unit = GameObject.Instantiate (unitObject, new Vector3 (0, 0, -1), Quaternion.identity).GetComponent<Unit>();
		dodge = 50;
	}

	// Update is called once per frame
	void Update () {
		 
	}
	//reserve spaces and queue up actions
	public void DecideTurn(){

	}

	bool NeedToDodge(){
		return gameController.GetTarget (unit.cordX, unit.cordY) > 0;
	}
	bool TargetInRange(){
		if (myTarget != null) {
			if (Mathf.Abs (myTarget.cordX - unit.cordX) < unit.primaryWeapon.range) {
				if (Mathf.Abs (myTarget.cordY - unit.cordY) < unit.primaryWeapon.range) {
					return true;
				}
			}
		}
		return false;
	}
	public void ClaimMove(){
		int rand = Random.Range (0, 100); //used for stat checks 
		//claim move location, current location by default
		claimX = unit.cordX;
		claimY = unit.cordY;

		//am I in danger? Do I need to dodge?
		if (NeedToDodge ()) {
			ClaimRandomMove ();
		}
		//okay I'm safe, who am I going after?
		if (myTarget == null) {
			//select target
		}
		//can I hit them?	
		if (TargetInRange ()) {
			attacking = true;
		} else { //if not, I need to pick out a spot where I can hit them, and find where to move next

		}
			

	}

	public void ClaimMove2(){
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
			if (attacking) {
				if (rand < 90) {
					attacking = true;
				}
			}else if (nearestDistance == unit.primaryWeapon.range) { //needs a bandaid
                if (rand < 50) {
					attacking = true;
				}
			} else if (nearestDistance < unit.primaryWeapon.range + 1) { //needs a bandaid
				if (rand < 40) {
					attacking = true;
				}
			} else if (nearestDistance < unit.primaryWeapon.range + 2) { //needs a bandaid
				if (rand < 30) {
					attacking = true;
				}
			} else if (nearestDistance < unit.primaryWeapon.range + 3) { //needs a bandaid
				if (rand < 20) {
					attacking = true;
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
		int ylower = 0;
		int yupper = 1;
		int xlower = 0;
		int xupper = 1;
		//gameController.SetOccupation (unit.cordX, unit.cordY, 0);
		List<int> validX = new List<int> ();
		List<int> validY = new List<int> ();
		List<Vector2> validMoves = new List<Vector2> ();
		
		attacking = false;
		//move out the way!
		if(unit.cordY + 1< gameController.GetLevelHeight() && gameController.GetOccupation(claimX, claimY + 1) == 0){
			validY.Add(1);
			validMoves.Add(new Vector2(0,1));
		}
		if(unit.cordY - 1 > 0 && gameController.GetOccupation(claimX, claimY - 1) == 0){
			validY.Add(-1);
			validMoves.Add(new Vector2(0,-1));
		}
		if(unit.cordX + 1 < gameController.GetLevelWidth() && gameController.GetOccupation(claimX + 1, claimY) == 0  ){
			validX.Add(1);
			validMoves.Add(new Vector2(1,0));
		}
		if(unit.cordX - 1 > 0 && gameController.GetOccupation(claimX - 1, claimY) == 0){
			validX.Add(-1);
			validMoves.Add(new Vector2(-1,0));
		}

		int attempt = 0;

		if (validMoves.Count > 1) {
			int index = Random.Range (0, validMoves.Count);
			claimX = (int)validMoves [index].x + unit.cordX;
			claimY = (int)validMoves [index].y + unit.cordY;
		} else {
			Debug.Log ("no valid dodges");
			claimX = 0 + unit.cordX;
			claimY = 0 + unit.cordY;
		}

		Debug.Log (claimX + ", " + claimY);

	}
}