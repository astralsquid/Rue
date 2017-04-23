using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public Color myColor;
	int damage = 1;
	public GameObject reticleObject;
	public Reticle reticle;
	public Unit owner;
	public GameObject myReticleObject;
	public int range;
	public string name = "weapon";
	public int step = 0;
	public int steps = 2;
	public int aimStep = 1;
	public int strikeStep = 2;
    public int savedReticleX;
    public int savedReticleY;
    bool weaponRotLocked;

    // Use this for initialization
    void Start () {
        weaponRotLocked = false;
        step = 0;
		reticleObject = Resources.Load ("Prefabs/Reticle") as GameObject;
        savedReticleX = -1;
        savedReticleY = -1;
        GameObject.Find("Armory").GetComponent<Armory>().weapons.Add(this);
        MakeVisible();
        Reset();
    }

    public IEnumerator Stab(float timeToMove)
    {
        weaponRotLocked = true;
        Vector3 position = new Vector3(reticle.transform.position.x, reticle.transform.position.y, transform.position.z);
        var currentPos = transform.position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            if (t > timeToMove / 2)
            {
                transform.position = Vector3.Lerp(position, currentPos, t);

            }
            else
            {
                transform.position = Vector3.Lerp(currentPos, position, t);
            }

            if(t >= timeToMove)
            {
               Reset();
               weaponRotLocked = false;

            }
            yield return null;
        }

    }

    void Update()
    {
        if (reticle != null && !weaponRotLocked)
        {
            Vector3 vectorToTarget = reticle.transform.position - transform.position;
            float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 90;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 10f);
        }
    }

    public IEnumerator RotateWeapon(float timeToMove)
    {
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            Vector3 vectorToTarget = reticle.transform.position - transform.position;
            float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 90;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, t);
            yield return null;
        }
    }


    public void MakeInvisible()
    {
       GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f); 
    }
    public void MakeVisible()
    {
        GetComponent<SpriteRenderer>().color = myColor;
    }

	public bool Attack(){
        MakeVisible();
		step += 1;
		if (step == aimStep) {
            //aim
            transform.Rotate(0, 0, Random.Range(0, 361));
            myReticleObject = GameObject.Instantiate (reticleObject, owner.transform.position, Quaternion.identity);
			reticle = myReticleObject.GetComponent<Reticle> ();
			reticle.GetComponent<SpriteRenderer> ().color = new Color (owner.myColor.r, owner.myColor.g, owner.myColor.b, .2f);
				
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
                MoveReticleEastInitial();
            }else if(closestEnemyX < owner.cordX)
            {
                MoveReticleWestInitial();
            }

            if(closestEnemyY > owner.cordY)
            {
                MoveReticleNorthInitial();
            }else if(closestEnemyY < owner.cordY)
            {
                MoveReticleSouthInitial();
            }
            Vector3 vectorToTarget = reticle.transform.position - transform.position;
            float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) + 90;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, 1);

            return false;
		} else if (step == strikeStep) {
            //strike
            StartCoroutine(Stab(.5f));
            Strike();
		    //Reset ();
		}

		return true;
	}

	public void lockTarget(){
		owner.aiming = false;
		owner.gameController.AddTarget (reticle.cordX, reticle.cordY);
	}
		
	public void Strike(){
		Debug.Log ("Strike");
		if (owner.gameController.unitGrid [reticle.cordY * owner.gameController.GetLevelWidth() + reticle.cordX] != null) {
			owner.gameController.unitGrid [reticle.cordY * owner.gameController.GetLevelWidth() + reticle.cordX].TakeDamage (damage);
		}
	}

	public void Reset(){
		step = 0;
		if (reticle != null) {
			owner.gameController.SubTarget (reticle.cordX, reticle.cordY);
            transform.position = new Vector3(owner.transform.position.x, owner.transform.position.y, -4);
        }
        Destroy (myReticleObject);
        MakeInvisible();
	}

	bool MoveReticle(int x, int y){
        bool canMove = false;
		if (x <= owner.cordX + range && x >= owner.cordX - range && (y == owner.cordY + range || y == owner.cordY - range)) {
			canMove = true;
		}
		if (y <= owner.cordY + range && y >= owner.cordY - range && (x == owner.cordX + range || x == owner.cordX - range)) {
			canMove = true;
		}

		if (canMove) {

            if (x >= 0 && y >= 0 && x < owner.gameController.GetLevelWidth () && y < owner.gameController.GetLevelHeight ()) {
				if (x != owner.cordX || y != owner.cordY) {					
					reticle.cordX = x;
					reticle.cordY = y;
					GameObject t = owner.gameController.tileGrid [reticle.cordY * owner.gameController.GetLevelWidth () + reticle.cordX];
					reticle.transform.position = new Vector3 (t.transform.position.x, t.transform.position.y, -3);
                   // StartCoroutine(RotateWeapon(.2f));

                    return true;
				}
			}
		}
        return false;
	}
	public bool MoveReticleNorthInitial(){
		return MoveReticle (reticle.cordX, reticle.cordY + range);
	}
	public bool MoveReticleSouthInitial(){
        return MoveReticle(reticle.cordX, reticle.cordY - range);
	}
	public bool MoveReticleEastInitial(){
        return MoveReticle(reticle.cordX + range, reticle.cordY);
	}
	public bool MoveReticleWestInitial(){
        return MoveReticle(reticle.cordX - range, reticle.cordY);
	}
	public bool MoveReticleNorth(){
		return MoveReticle (reticle.cordX, reticle.cordY + 1);
	}
	public bool MoveReticleSouth(){
		return MoveReticle(reticle.cordX, reticle.cordY - 1);
	}
	public bool MoveReticleEast(){
		return MoveReticle(reticle.cordX + 1, reticle.cordY);
	}
	public bool MoveReticleWest(){
		return MoveReticle(reticle.cordX - 1, reticle.cordY);
	}

}
