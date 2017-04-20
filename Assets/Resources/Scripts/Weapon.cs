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
    public int savedReticleX;
    public int savedReticleY;


	// Use this for initialization
	void Start () {
		step = 0;
		reticleObject = Resources.Load ("Prefabs/Reticle") as GameObject;
        savedReticleX = -1;
        savedReticleY = -1;
        GameObject.Find("Armory").GetComponent<Armory>().weapons.Add(this);
    }

	public bool Attack(){
		step += 1;
		if (step == aimStep) {
			//aim
			myReticleObject = GameObject.Instantiate (reticleObject, owner.transform.position, Quaternion.identity);
			reticle = myReticleObject.GetComponent<Reticle> ();
			reticle.cordX = owner.cordX;
			reticle.cordY = owner.cordY ;
			owner.aiming = true;
            Vector3 tempPosition;
            tempPosition = new Vector3(owner.transform.position.x, owner.transform.position.y, -3);
            reticle.cordX = owner.cordX;
            reticle.cordY = owner.cordY;


            int closestEnemyX = 1000;
            int closestEnemyY = 1000;
            int closestDistance = (2000);
            for(int w = 0; w < owner.gameController.GetLevelWidth(); w++)
            {
                for(int h = 0; h < owner.gameController.GetLevelHeight(); h++)
                {
                    if (owner.gameController.occupationGrid[h * owner.gameController.GetLevelWidth() + w] == 1 && (w != owner.cordX || h != owner.cordY))
                    {
                        int distance = Mathf.Abs(owner.cordX - w) + Mathf.Abs(owner.cordY - h);
                        if(distance < closestDistance)
                        {
                            closestDistance = distance;
                            closestEnemyX = w;
                            closestEnemyY = h;
                        }
                    }
                }
            }
            if(closestEnemyX > owner.cordX)
            {
                MoveReticleEast();
            }else if(closestEnemyX < owner.cordX)
            {
                MoveReticleWest();
            }

            if(closestEnemyY > owner.cordY)
            {
                MoveReticleNorth();
            }else if(closestEnemyY < owner.cordY)
            {
                MoveReticleSouth();
            }
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

	bool MoveReticle(int x, int y){
		if (x >= 0 && y >= 0 && x < owner.gameController.GetLevelWidth () && y < owner.gameController.GetLevelHeight ()) {
			if (x <= owner.cordX + range && x >= owner.cordX - range && y <= owner.cordY + range && y >= owner.cordY - range) {
				if (x != owner.cordX || y != owner.cordY) {
					reticle.cordX = x;
					reticle.cordY = y;
                    GameObject t = owner.gameController.tileGrid [reticle.cordY * owner.gameController.GetLevelWidth () + reticle.cordX];
					reticle.transform.position = new Vector3 (t.transform.position.x, t.transform.position.y, -3);
                    return true;
				}
			}
		}
        return false;
	}
	public bool MoveReticleNorth(){
		return MoveReticle (reticle.cordX, reticle.cordY + range);
	}
	public bool MoveReticleSouth(){
        return MoveReticle(reticle.cordX, reticle.cordY - range);
	}
	public bool MoveReticleEast(){
        return MoveReticle(reticle.cordX + range, reticle.cordY);
	}
	public bool MoveReticleWest(){
        return MoveReticle(reticle.cordX - range, reticle.cordY);
	}
}
