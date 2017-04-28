using UnityEngine;
using System.Collections;

//december 2016 or may of 2016


public class PlayerInputController : MonoBehaviour {
	public Camera camera;
	public GameController gameController;
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
    public bool canLockCamera;
    public bool cameraLocked;
    public bool inputEnabled;
	// Use this for initialization
	void Start () {
		canMoveNorth = true;
		canMoveSouth = true;
		canMoveEast = true;
		canMoveWest = true;
		canAttack = true;
		canMoveReticle = true;
        cameraLocked = true;
        inputEnabled = true;

    }
	// Update is called once per frame
	void LateUpdate () {
	}
	void Update(){
		ResetCamera ();
        if (inputEnabled)
        {
            if (!playerUnit.aiming)
            {
                if (ScanForInput())
                {
					StartCoroutine(gameController.RunTurn());
                }
            }
            else if (ScanForAim())
            { //we are aiming
				StartCoroutine(gameController.RunTurn());
                playerUnit.aiming = false;
            }
        }
		ResetCamera ();
	}

	void ResetCamera(){
        if (cameraLocked)
        {
            camera.transform.position = new Vector3(playerUnit.transform.position.x, playerUnit.transform.position.y, camera.transform.position.z);
        }
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

        if (Input.GetAxisRaw("LockCamera") != 0 && canLockCamera)
        { //lock target
            cameraLocked = !cameraLocked;
            canLockCamera = false;
            return false;
        }
        else if (Input.GetAxisRaw("LockCamera") == 0)
        {
            canLockCamera = true;
        }
        return false;
	}

	bool ScanForInput(){

        if (Input.GetAxisRaw("LockCamera") != 0 && canLockCamera)
        { //lock target
            cameraLocked = !cameraLocked;
            canLockCamera = false;
            return false;
        }
        else if (Input.GetAxisRaw("LockCamera") == 0)
        {
            canLockCamera = true;
        }
        if (Input.GetAxisRaw ("MoveNorth") != 0 && canMoveNorth) {
			canMoveNorth = false;
			return playerUnit.MoveNorth (cameraLocked);
		} else if (Input.GetAxisRaw ("MoveNorth") == 0) {
			canMoveNorth = true;
		} 
		if (Input.GetAxisRaw ("MoveSouth") != 0 && canMoveSouth) {
			canMoveSouth = false;
			return playerUnit.MoveSouth (cameraLocked);
		} else if (Input.GetAxisRaw ("MoveSouth") == 0) {
			canMoveSouth = true;
		} 
		if (Input.GetAxisRaw ("MoveEast") != 0 && canMoveEast) {
			canMoveEast = false;
			return playerUnit.MoveEast (cameraLocked);
		} else if (Input.GetAxisRaw ("MoveEast") == 0) {
			canMoveEast = true;
		} 
		if (Input.GetAxisRaw ("MoveWest") != 0 && canMoveWest) {
			canMoveWest = false;
			return 	playerUnit.MoveWest (cameraLocked);
		} else if (Input.GetAxisRaw ("MoveWest") == 0) {
			canMoveWest = true;
		} 
		if (Input.GetAxisRaw ("Attack") != 0 && canAttack) {
			canAttack = false;
			bool attacked =playerUnit.primaryWeapon.Attack (); 
				if (playerUnit.aiming) {
					playerUnit.primaryWeapon.reticle.SetReticleAim ();
				}
			return attacked;
		} else if (Input.GetAxisRaw ("Attack") == 0) {
			canAttack = true;
		}
		return false;
	}

}