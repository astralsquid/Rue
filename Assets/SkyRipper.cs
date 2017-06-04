using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyRipper : Weapon {
	public GameObject pivot;
	float rotationKeeper;
	bool skyRipperRotationOn;
	bool pullBackMode;
	float slow_attack;
	float fast_attack;
	// Use this for initialization
	void Start () {
        base.Start();
		pullBackMode = false;
		skyRipperRotationOn = true;
        myType = WeaponType.skyripper;
        standardRotation = false;
		rotationKeeper = 0;
		attack_time = .1f;
		slow_attack = attack_time;
		fast_attack = slow_attack / 5.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if(skyRipperRotationOn){
			float m = Time.deltaTime * 800f % 360;
			transform.RotateAround (pivot.transform.position, new Vector3(0,0,1), m);
			rotationKeeper += m;
		}
	}


	void HaltSkyripperRotation(){
		skyRipperRotationOn = false;
		transform.RotateAround (transform.position, new Vector3(0,0,1), -rotationKeeper);
		rotationKeeper = 0;
	}
	void ResumeSkyripperRotation(){
		skyRipperRotationOn = true;
	}
	override public IEnumerator Stab(float timePerSquare)
	{
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
				HaltSkyripperRotation();
				attack_time = fast_attack;
				owner.gameController.playerInputController.EnableInput();
				step -= 1;
				ForceMoveReticleSouth();
				if (reticle.cordX == owner.cordX && reticle.cordY == owner.cordY) {
					Reset ();
				}

			}
			yield return null;
		}
	}

	override public void Reset(){
		//ResumeSkyripperRotation ();
		step = 0;
		if (reticle != null) {
			owner.gameController.SubTarget (reticle.cordX, reticle.cordY);
		}if (owner != null) {
			transform.position = new Vector3 (owner.transform.position.x, owner.transform.position.y, -4);
		}
		Destroy (myReticleObject);
		ResumeSkyripperRotation ();
		MakeInvisible();
		attack_time = slow_attack;
	}

  override public void WhipItOut()
	{
		MakeVisible ();
		myReticleObject = GameObject.Instantiate (reticleObject, owner.transform.position, Quaternion.identity);
		reticles = new List<Reticle> ();
		reticle = myReticleObject.GetComponent<Reticle> ();
		reticle.GetComponent<SpriteRenderer> ().color = new Color (owner.my_color.r, owner.my_color.g, owner.my_color.b, .5f);

		reticle.cordX = owner.cordX;
		reticle.cordY = owner.cordY;

		owner.aiming = true;
		Vector3 tempPosition;
		tempPosition = new Vector3 (owner.transform.position.x, owner.transform.position.y, -3);

		//move reticle in direction of closest enemy by default
		Debug.Log(transform.rotation.z);
		AutoAim ();
		Debug.Log(transform.rotation.z);
	}
}