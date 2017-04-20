using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
	int damage = 1;
	public GameObject reticleObject;
	public Reticle reticle;
	public Unit owner;
	public GameObject myReticleObject;
	public int range = 1;
	public string name = "weapon";
	public int step = 0;
	public int steps = 2;
	public int aimStep = 1;
	public int strikeStep = 2;
	// Use this for initialization
	void Start () {
		step = 0;
		reticleObject = Resources.Load ("Prefabs/Reticle") as GameObject;
	}

	public bool Attack(){
		step += 1;
		if (step == aimStep) {
			//aim
			myReticleObject = GameObject.Instantiate (reticleObject, owner.transform.position, Quaternion.identity);
			reticle = myReticleObject.GetComponent<Reticle> ();
			reticle.cordX = owner.cordX;
			reticle.cordY = owner.cordY;
			owner.aiming = true;
			return false;
		} else if (step == strikeStep) {
			//strike
			Strike();
			Reset ();
		}

		return true;
	}

	public void lockTarget(){
		owner.aiming = false;
	}
	public void Strike(){
		Debug.Log ("Strike");
		if (owner.gameController.unitGrid [reticle.cordY * owner.gameController.GetLevelWidth() + reticle.cordX] != null) {
			owner.gameController.unitGrid [reticle.cordY * owner.gameController.GetLevelWidth() + reticle.cordX].TakeDamage (damage);
		}
	}

	public void Reset(){
		Debug.Log ("Reset");
		step = 0;
		Destroy (myReticleObject); 
	}

	void MoveReticle(int x, int y){
		if (x >= 0 && y >= 0 && x < owner.gameController.GetLevelWidth () && y < owner.gameController.GetLevelHeight ()) {
			if (x <= owner.cordX + range && x >= owner.cordX - range && y <= owner.cordY + range && y >= owner.cordY - range) {
				if (x != owner.cordX || y != owner.cordY) {
					reticle.cordX = x;
					reticle.cordY = y;
					GameObject t = owner.gameController.tileGrid [reticle.cordY * owner.gameController.GetLevelWidth () + reticle.cordX];
					reticle.transform.position = new Vector3 (t.transform.position.x, t.transform.position.y, -3);
				}
			}
		}
	}



	public void MoveReticleNorth(){
		MoveReticle (reticle.cordX, reticle.cordY + 1);
	}
	public void MoveReticleSouth(){
		MoveReticle (reticle.cordX, reticle.cordY - 1);
	}
	public void MoveReticleEast(){
		MoveReticle (reticle.cordX + 1, reticle.cordY);
	}
	public void MoveReticleWest(){
		MoveReticle (reticle.cordX - 1, reticle.cordY);
	}
}
