using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//december 2016 or may of 2016


public class PlayerInputController : MonoBehaviour {
	public Camera camera;
	public GameController gameController;
	public Unit playerUnit;
    Unit target;
    public GameObject targetPanel;
    LineRenderer lineRenderer;

    //UI elements
    public Text targetNameText;
    public Text targetWishText;
    public TargetPanel targetPanelScript;


    bool first_move;
    int targetIndex = 0;
	 bool canMoveNorth;
	 bool canMoveSouth;
	 bool canMoveEast;
	 bool canMoveWest;
	 bool canAttack;
	 bool canMoveReticle;
	 bool canMoveNorthReticle;
	 bool canMoveSouthReticle;
	 bool canMoveEastReticle;
	 bool canMoveWestReticle;
     bool canLockCamera;
    bool canChangeTarget;
    public bool cameraLocked;
    bool inputEnabled;
	// Use this for initialization
	void Start () {
        first_move = true;
        lineRenderer = GetComponent<LineRenderer>();
        target = playerUnit;
        ChangeTarget(playerUnit);
        inputEnabled = true;
    }
    void ChangeTarget()
    {
        targetIndex++;
        if(targetIndex > gameController.unitList.Count - 1)
        {
            targetIndex = 0;
        }
        target = gameController.unitList[targetIndex];
        FillTargetPanel();
    }
    void ChangeTarget(Unit u)
    {
        target = u;
        FillTargetPanel();
    }
    void FillTargetPanel()
    {
       targetPanelScript.SetTargetSprite(target.GetComponent<SpriteRenderer>());
        targetNameText.text = target.name;
        
    }

	void Update(){
		ResetCamera ();
        ScanForInterfaceInputs();
        //DrawLine();
        if (inputEnabled)
        {
            if (!playerUnit.aiming)
            {
                if (ScanForInput())
                {
                    if (first_move)
                    {
                        StartCoroutine(gameController.elevator.Raise(false));
                        first_move = false;
                    }
                    gameController.ResetReflex();
                    StartCoroutine(gameController.RunTurn());
                }
            }
            else if (ScanForAim() && ! first_move)
            { //we are aiming
                gameController.ResetReflex();
                StartCoroutine(gameController.RunTurn());
                playerUnit.aiming = false;
            }
        }
		ResetCamera ();
	}



    void ScanForInterfaceInputs()
    {
        if(Input.GetAxisRaw("ChangeTarget") != 0 && canChangeTarget)
        {
            canChangeTarget = false;
            ChangeTarget();
        }else if(Input.GetAxisRaw("ChangeTarget") == 0)
        {
            canChangeTarget = true;
        }
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

    private void DrawLine()
    {
        if (target.alive)
        {
            Vector3 unitPosition = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z + 10);
            Vector3 panelPosition = new Vector3(targetPanel.transform.position.x - 1, targetPanel.transform.position.y - 1, targetPanel.transform.position.z);
            lineRenderer.SetPosition(0, target.transform.position);
            lineRenderer.SetPosition(1, panelPosition);
        }else
        {
            ChangeTarget(playerUnit);
        }
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
        if (Input.GetAxisRaw ("MoveNorth") != 0 && !playerUnit.moving) {
			canMoveNorth = false;
			return playerUnit.MoveNorth (cameraLocked);
		} 
		if (Input.GetAxisRaw ("MoveSouth") != 0 && !playerUnit.moving) {
			canMoveSouth = false;
			return playerUnit.MoveSouth (cameraLocked);
		} 
		if (Input.GetAxisRaw ("MoveEast") != 0 && !playerUnit.moving) {
			canMoveEast = false;
			return playerUnit.MoveEast (cameraLocked);
		} 
		if (Input.GetAxisRaw ("MoveWest") != 0 && !playerUnit.moving) {
			canMoveWest = false;
			return 	playerUnit.MoveWest (cameraLocked);
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
    public void EnableInput()
    {
        inputEnabled = true;
    }
    public void DisableInput()
    {
        inputEnabled = false;
    }
}