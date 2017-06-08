using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyRipper : Weapon {
	public int mode;

	bool skyRipperRotationOn;
	bool pullBackMode;
	public LineDrawer lineDrawer;
	public GameObject connectionPivot;

	// Use this for initialization
	void Start () {
        base.Start();
		pullBackMode = false;
		skyRipperRotationOn = true;
        myType = WeaponType.skyripper;
		spinType = SpinType.lasso;

		attack_time = .05f;
		slow_attack = attack_time;
		fast_attack = slow_attack / 5.0f;
		mode = 1;
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
		if (visible) {
			lineDrawer.DrawLine (owner.gameObject, connectionPivot);
		} else {
			lineDrawer.DrawLine (owner.gameObject, owner.gameObject);
		}
	}
		
	override public IEnumerator Stab(float timePerSquare)
	{
		HaltLassoSpin();

		weaponRotLocked = true;
		float timeToMove = timePerSquare * range;
		owner.gameController.playerInputController.DisableInput();
		Vector3 position = new Vector3(reticle.transform.position.x, reticle.transform.position.y, transform.position.z);
		var currentPos = transform.position;
		var t = 0f;
		DealDamage ();
		bool pulling_back = false;
		while (t < 1 && !pulling_back)
		{
			owner.gameController.playerInputController.DisableInput();

			t += Time.deltaTime / (timeToMove);

			transform.position = Vector3.Lerp(currentPos, position, t);
			if(t >= 1)
			{
				//Reset();
				attack_time = fast_attack;
				owner.gameController.playerInputController.EnableInput();
				Debug.Log ("mode");

				Debug.Log (mode);
				switch (mode) {
				case 1:
					ForceMoveReticleSouth();
					break;
				case 2:
					ForceMoveReticleNorth();
					break;
				case 3:
					ForceMoveReticleWest();
					break;
				case 4:
					ForceMoveReticleEast();
					break;
				default:
					break;

				}
				step -= 1;

				if (reticle.cordX == owner.cordX && reticle.cordY == owner.cordY) {
					Reset ();
				}

			}
			yield return null;
		}
	}

	override public bool MoveReticleNorth(){
		mode = 1;
		return MoveReticle (owner.cordX, owner.cordY + range, false);
	}
	override public bool MoveReticleSouth(){
		mode = 2;
		return MoveReticle(owner.cordX, owner.cordY - range, false);
	}
	override public bool MoveReticleEast(){
		mode = 3;
		return MoveReticle(owner.cordX + range, owner.cordY, false);
	}
	override public bool MoveReticleWest(){
		mode = 4;
		return MoveReticle(owner.cordX - range, owner.cordY, false);
	}

	override public void HaltLassoSpin(){
		lassoSpinOn = false;
		transform.rotation = Quaternion.identity;
		switch (mode) {
		case 1:
			transform.RotateAround(transform.position, new Vector3(0,0,1), 0);
			break;
		case 2:
			transform.RotateAround(transform.position, new Vector3(0,0,1), 180);
			break;
		case 3:
			transform.RotateAround(transform.position, new Vector3(0,0,1), 270);
			break;
		case 4:
			transform.RotateAround(transform.position, new Vector3(0,0,1), 90);
			break;
		default:
		break;
		}

	}

	override protected void AutoAim()
	{
		int closestEnemyX = 1000;
		int closestEnemyY = 1000;
		int closestDistance = (2000);
		for (int w = 0; w < owner.gameController.GetLevelWidth(); w++)
		{
			for (int h = 0; h < owner.gameController.GetLevelHeight(); h++)
			{
				if (owner.gameController.occupationGrid[h * owner.gameController.GetLevelWidth() + w] == 1 && (w != owner.cordX || h != owner.cordY))
				{
					int distance = Mathf.Abs(Mathf.Abs(owner.cordX - w) + Mathf.Abs(owner.cordY - h) - range);
					if (distance < closestDistance)
					{
						closestDistance = distance;
						closestEnemyX = w;
						closestEnemyY = h;
					}
				}
			}
		}
		if (closestEnemyX > owner.cordX)
		{
			MoveReticleEastInitial();
			mode = 3;
		}
		else if (closestEnemyX < owner.cordX)
		{
			MoveReticleWestInitial();
			mode = 4;
		}else if (closestEnemyY > owner.cordY)
		{
			MoveReticleNorthInitial();
			mode = 1;
		}
		else if (closestEnemyY < owner.cordY)
		{
			MoveReticleSouthInitial();
			mode = 2;
		}
	}
}