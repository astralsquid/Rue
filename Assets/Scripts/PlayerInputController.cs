using UnityEngine;
using System.Collections;

//december 2016 or may of 2016


public class PlayerInputController : MonoBehaviour {
	public Camera camera;
	public GameController gameController;
	public bool camera_locked = true;
	public Unit playerUnit;
	public bool canMoveNorth;
	public bool canMoveSouth;
	public bool canMoveEast;
	public bool canMoveWest;
	public bool canAttack;
	public bool canMoveReticle;
	public bool canMoveNorthReticle;
	public bool canMoveSouthReticle;
	public bool canMoveEastReticle;
	public bool canMoveWestReticle;
	// Use this for initialization
	void Start () {
		canMoveNorth = true;
		canMoveSouth = true;
		canMoveEast = true;
		canMoveWest = true;
		canAttack = true;
		canMoveReticle = true;
	}
	// Update is called once per frame
	void LateUpdate () {
	}
	void Update(){
		ResetCamera ();
		if (!playerUnit.aiming) {
			if (ScanForInput ()) {
				gameController.RunTurn ();
				if (!playerUnit.aiming) {
					playerUnit.primaryWeapon.Reset ();
				}
			}
		} else if(ScanForAim()) { //we are aiming
			gameController.RunTurn();
			playerUnit.aiming = false;
		}
		ResetCamera ();

	}

	void ResetCamera(){
		camera.transform.position = new Vector3 (playerUnit.transform.position.x, playerUnit.transform.position.y, camera.transform.position.z);
	}

	bool ScanForAim(){
		if (Input.GetAxisRaw ("MoveNorth") != 0 && canMoveNorthReticle) {
			canMoveNorthReticle = false;
			playerUnit.primaryWeapon.MoveReticleNorth ();
			return false;
		} else if (Input.GetAxisRaw ("MoveNorth") == 0) {
			canMoveNorthReticle = true;
		} 
		if (Input.GetAxisRaw ("MoveSouth") != 0 && canMoveSouthReticle) {
			canMoveSouthReticle = false;
			playerUnit.primaryWeapon.MoveReticleSouth ();
			return false;
		} else if (Input.GetAxisRaw ("MoveSouth") == 0) {
			canMoveSouthReticle = true;
		} 
		if (Input.GetAxisRaw ("MoveEast") != 0 && canMoveEastReticle) {
			canMoveEastReticle = false;
			playerUnit.primaryWeapon.MoveReticleEast ();
			return false;
		} else if (Input.GetAxisRaw ("MoveEast") == 0) {
			canMoveEastReticle = true;
		} 
		if (Input.GetAxisRaw ("MoveWest") != 0 && canMoveWestReticle) {
			canMoveWestReticle = false;
			playerUnit.primaryWeapon.MoveReticleWest ();
			return 	false;
		} else if (Input.GetAxisRaw ("MoveWest") == 0) {
			canMoveWestReticle = true;
		} 

		if (Input.GetAxisRaw ("Attack") != 0 && canAttack) { //lock target
			playerUnit.primaryWeapon.lockTarget ();
			canAttack = false;
			return true;
		} else if (Input.GetAxisRaw ("Attack") == 0) {
			canAttack = true;
		}
		return false;
	}

	bool ScanForInput(){
		if (Input.GetAxisRaw ("MoveNorth") != 0 && canMoveNorth) {
			canMoveNorth = false;
			return playerUnit.MoveNorth ();
		} else if (Input.GetAxisRaw ("MoveNorth") == 0) {
			canMoveNorth = true;
		} 
		if (Input.GetAxisRaw ("MoveSouth") != 0 && canMoveSouth) {
			canMoveSouth = false;
			return playerUnit.MoveSouth ();
		} else if (Input.GetAxisRaw ("MoveSouth") == 0) {
			canMoveSouth = true;
		} 
		if (Input.GetAxisRaw ("MoveEast") != 0 && canMoveEast) {
			canMoveEast = false;
			return playerUnit.MoveEast ();
		} else if (Input.GetAxisRaw ("MoveEast") == 0) {
			canMoveEast = true;
		} 
		if (Input.GetAxisRaw ("MoveWest") != 0 && canMoveWest) {
			canMoveWest = false;
			return 	playerUnit.MoveWest ();
		} else if (Input.GetAxisRaw ("MoveWest") == 0) {
			canMoveWest = true;
		} 
		if (Input.GetAxisRaw ("Attack") != 0 && canAttack) {
			canAttack = false;
			return playerUnit.primaryWeapon.Attack ();
		} else if (Input.GetAxisRaw ("Attack") == 0) {
			canAttack = true;
		}
		return false;
	}

}