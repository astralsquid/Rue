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
        //return gameController.GetTarget (unit.cordX, unit.cordY) > 0;
        return false;
	}
	void SelectTarget(){
		int rand = Random.Range (0, 101);
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
		myTarget = gameController.unitList [nui];

	}
	void ClaimStrategicMove(){
		List<Vector2> strategicMoves = new List<Vector2> ();
		if (myTarget.cordX > unit.cordX + unit.primaryWeapon.range) {
			strategicMoves.Add(new Vector2(1, 0));
		}
		if (myTarget.cordX < unit.cordX - unit.primaryWeapon.range) {
			strategicMoves.Add(new Vector2(-1, 0));
		}
		if (myTarget.cordX < unit.cordX + unit.primaryWeapon.range && myTarget.cordX > unit.cordX) {
			strategicMoves.Add(new Vector2(-1, 0));
		}
		if (myTarget.cordX > unit.cordX - unit.primaryWeapon.range && myTarget.cordX < unit.cordX) {
			strategicMoves.Add(new Vector2(1, 0));
		}
		if (myTarget.cordY > unit.cordY + unit.primaryWeapon.range) {
			strategicMoves.Add(new Vector2(0, 1));
		}
		if (myTarget.cordY < unit.cordY - unit.primaryWeapon.range) {
			strategicMoves.Add(new Vector2(0, -1));
		}
		if (myTarget.cordY < unit.cordY + unit.primaryWeapon.range && myTarget.cordY > unit.cordY) {
			strategicMoves.Add(new Vector2(0, -1));
		}
		if (myTarget.cordY > unit.cordY - unit.primaryWeapon.range && myTarget.cordY < unit.cordY) {
			strategicMoves.Add(new Vector2(0, 1));
		}

		if (strategicMoves.Count >= 1) {
			int index = Random.Range (0, strategicMoves.Count);
			claimX = (int)strategicMoves [index].x + unit.cordX;
			claimY = (int)strategicMoves [index].y + unit.cordY;
		} else {
			Debug.Log ("no strategic moves");
			claimX = 0 + unit.cordX;
			claimY = 0 + unit.cordY;
		}

	}
	bool TargetInRange(){
		if (myTarget != null) {
			if (Mathf.Abs(Mathf.Abs (myTarget.cordX - unit.cordX) - unit.primaryWeapon.range) <= 1) {
				if (Mathf.Abs(Mathf.Abs (myTarget.cordY - unit.cordY) - unit.primaryWeapon.range) <= 1) {
                    Debug.Log("Target in Range!");
					return true;
				}
			}
		}
		return false;

        int distX = Mathf.Abs(myTarget.cordX - unit.cordX);
        int distY = Mathf.Abs(myTarget.cordY - unit.cordY);

        if (myTarget != null)
        {
           if(Mathf.Abs(distX - unit.primaryWeapon.range) == 0 && Mathf.Abs(distY - unit.primaryWeapon.range) == 0)
            { //1 out of range
                return true;
            }
        }
        return false;
	}

	public void ClaimMove(){
		int rand = Random.Range (0, 100); //used for stat checks 
		//claim move location, current location by default
		claimX = unit.cordX;
		claimY = unit.cordY;


		//who am I going after
		SelectTarget();
		//can I hit them?

		//am I in danger? Do I need to dodge?
		//shift things up
		if (!attacking && Random.Range(0,101) < 15) {
			attacking = false;
			ClaimRandomMove ();
		}else if (NeedToDodge ()) {
			ClaimRandomMove ();
			attacking = false;
		}else if (TargetInRange ()) {
            attacking = true;
        }else
        { //if not, I need to pick out a spot where I can hit them, and find where to move next
            attacking = false;
            ClaimStrategicMove();
            PickMoveTowardsTarget();
        }
	}
	public void AimAtTarget(){
		if (Mathf.Abs (myTarget.cordX - unit.cordX) == unit.primaryWeapon.range && Mathf.Abs (myTarget.cordY - unit.cordY) <= unit.primaryWeapon.range) {
			unit.primaryWeapon.MoveReticle (myTarget.cordX, myTarget.cordY);
		}else if (Mathf.Abs (myTarget.cordY - unit.cordY) == unit.primaryWeapon.range && Mathf.Abs (myTarget.cordX - unit.cordX) <= unit.primaryWeapon.range) {
			unit.primaryWeapon.MoveReticle (myTarget.cordX, myTarget.cordY);
		}
	}

    public void PickMoveTowardsTarget()
    {
        
    }

	public void ClaimMove2(){
		if (!unit.alive) {
			gameController.enemyControllers.Remove (this);
            Destroy(unit);
            Destroy(this);
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
					unit.aiming = false;
					unit.primaryWeapon.lockTarget ();
					AimAtTarget ();
				}
				attacking = false;
			}
		}
	}

	public bool CheckMove(int x, int y){
		if(unit.cordX + x < gameController.GetLevelWidth() && unit.cordX + x >= 0){
			if(unit.cordY + y < gameController.GetLevelHeight() && unit.cordY + y >= 0 ){
				if(gameController.GetOccupation(unit.cordX + x, unit.cordY + y) == 0){
					return true;
				}
			}
		}
		return false;
	}

    bool CheckLocation(int x, int y)
    {
        if (unit.cordX + x  >= 0 && unit.cordX + x < gameController.GetLevelWidth() && unit.cordY + y >= 0 && unit.cordY + y < gameController.GetLevelHeight())
        {
            if(gameController.GetOccupation(unit.cordX + x, unit.cordY + y) == 0)
            {
                return true;
            }
        }
        return false;
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

		if(CheckMove(1,0)){
			validMoves.Add(new Vector2(1,0));
		}
		if(CheckMove(0,1)){
			validMoves.Add(new Vector2(0,1));
		}
		if(CheckMove(-1,0)){
			validMoves.Add(new Vector2(-1,0));
		}
		if(CheckMove(0,-1)){
			validMoves.Add(new Vector2(0,-1));
		}
		if(CheckMove(1,1)){
			validMoves.Add(new Vector2(1,1));
		}
		if(CheckMove(-1,1)){
			validMoves.Add(new Vector2(-1,1));
		}
		if(CheckMove(1,-1)){
			validMoves.Add(new Vector2(1,-1));
		}
		if (CheckMove (-1, -1)) {
			validMoves.Add (new Vector2 (-1, -1));
		}
		if(CheckLocation(0,1)){
			validMoves.Add(new Vector2(0,1));
        }
        if(CheckLocation(0, -1)){
			validMoves.Add(new Vector2(0,-1));
		}
		if(CheckLocation(1, 0))
        {
			validMoves.Add(new Vector2(1,0));
		}
		if(CheckLocation(-1, 0))
        {
			validMoves.Add(new Vector2(-1,0));
		}
        if(CheckLocation(1, 1))
        {
            validMoves.Add(new Vector2(1, 1));
        }
        if (CheckLocation(-1, 1))
        {
            validMoves.Add(new Vector2(-1, 1));
        }
        if (CheckLocation(-1, -1))
        {
            validMoves.Add(new Vector2(-1, -1));
        }
        if (CheckLocation(1, -1))
        {
            validMoves.Add(new Vector2(1, -1));
        }

		int attempt = 0;


        if (validMoves.Count > 0) {
			int index = Random.Range (0, validMoves.Count);
			claimX = (int)validMoves [index].x + unit.cordX;
			claimY = (int)validMoves [index].y + unit.cordY;
		} else {
			claimX = 0 + unit.cordX;
			claimY = 0 + unit.cordY;
		}

	}
}