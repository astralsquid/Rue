using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public Color myColor;
    int damage = 1;
    public GameObject reticleObject;

    public float attack_time = .1f;

    //reticle
    public Reticle reticle;
    public List<Reticle> reticles;
    public enum WeaponType{other, sword, spear };
    WeaponType myType = WeaponType.other;
    //range this is a problem for later
    //public Sprite range_sprite;

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
	public float animationSpeed;
    // Use this for initialization
    protected void Start () {
        weaponRotLocked = false;
        step = 0;
		reticleObject = Resources.Load ("Prefabs/Reticle") as GameObject;
        savedReticleX = -1;
        savedReticleY = -1;
        GameObject.Find("Armory").GetComponent<Armory>().AddWeapon(this);
        MakeVisible();
        Reset();
    }

    public IEnumerator Stab(float timePerSquare)
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
                Reset();
                owner.gameController.playerInputController.EnableInput();
                pulling_back = true;
            }
            yield return null;
        }
    }

    protected void Update()
    {
        if (reticle != null && !weaponRotLocked)
        {
            Vector3 vectorToTarget = reticle.transform.position - transform.position;
            float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 90;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 8f);
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
    public WeaponType GetMyType()
    {
        return myType;
    }
    private void AutoAim()
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
        }
        else if (closestEnemyX < owner.cordX)
        {
            MoveReticleWestInitial();
        }
        if (closestEnemyY > owner.cordY)
        {
            MoveReticleNorthInitial();
        }
        else if (closestEnemyY < owner.cordY)
        {
            MoveReticleSouthInitial();
        }
    }

    public void WhipItOut()
    {
        MakeVisible();
        weaponRotLocked = false;
        transform.Rotate(0, 0, Random.Range(0, 361));
        myReticleObject = GameObject.Instantiate(reticleObject, owner.transform.position, Quaternion.identity);
        reticles = new List<Reticle>();
        reticle = myReticleObject.GetComponent<Reticle>();
        reticle.GetComponent<SpriteRenderer>().color = new Color(owner.my_color.r, owner.my_color.g, owner.my_color.b, .5f);

        reticle.cordX = owner.cordX;
        reticle.cordY = owner.cordY;

        owner.aiming = true;
        Vector3 tempPosition;
        tempPosition = new Vector3(owner.transform.position.x, owner.transform.position.y, -3);

        //move reticle in direction of closest enemy by default
        AutoAim();


        Vector3 vectorToTarget = reticle.transform.position - transform.position;
        float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) + 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, 1);

    }

    public void TuckItIn()
    {
        MakeInvisible();
        weaponRotLocked = true;
        Reset();
    }

	public bool Attack(){
        MakeVisible();
		step += 1;
		if (step == aimStep) {
            //aim
            WhipItOut();
            return false;
		} else if (step == strikeStep) {
            Strike();
		}

		return true;
	}

	public void lockTarget(){
		owner.aiming = false;
		owner.gameController.AddTarget (reticle.cordX, reticle.cordY);
		reticle.SetReticleTarget ();
	}
		
	public void Strike(){
		StartCoroutine(Stab(attack_time));
	}

	public void DealDamage(){
		if (owner.gameController.unitGrid [reticle.cordY * owner.gameController.GetLevelWidth() + reticle.cordX] != null) {
			owner.gameController.unitGrid [reticle.cordY * owner.gameController.GetLevelWidth() + reticle.cordX].TakeDamage (damage);
		}
	}

	public void Reset(){
		step = 0;
		if (reticle != null) {
			owner.gameController.SubTarget (reticle.cordX, reticle.cordY);
		}if (owner != null) {
			transform.position = new Vector3 (owner.transform.position.x, owner.transform.position.y, -4);
		}
        Destroy (myReticleObject);
        MakeInvisible();
	}

	public bool MoveReticle(int x, int y){
        bool canMove = false;
		if (x <= owner.cordX + range && x >= owner.cordX - range && (y == owner.cordY + range || y == owner.cordY - range)) {
			canMove = true;
		}
		else if (y <= owner.cordY + range && y >= owner.cordY - range && (x == owner.cordX + range || x == owner.cordX - range)) {
			canMove = true;
		}
        else if(x < range + owner.cordX && (x >= owner.cordX && reticle.cordX > owner.cordX) && reticle.cordY < range + owner.cordY && reticle.cordY > owner.cordY - range)
        {
            x += 1; x -= range*2;
            Debug.Log("1");
            canMove = true;
        }
        else if (x > owner.cordX - range && (x <= owner.cordX && reticle.cordX < owner.cordX) && reticle.cordY < range + owner.cordY && reticle.cordY > owner.cordY - range)
        {
            x -= 1; x += range * 2;
            Debug.Log("2");
            canMove = true;
        }else if (y < range + owner.cordY && (y >= owner.cordY && reticle.cordY > owner.cordY) && reticle.cordX < range + owner.cordX && reticle.cordX > owner.cordX - range)
        {
            y += 1; y -= range * 2;
            canMove = true;
        }
        else if (y > owner.cordY - range && (y <= owner.cordY && reticle.cordY < owner.cordY) && reticle.cordX < range + owner.cordX && reticle.cordX > owner.cordX - range)
        {
            y -= 1; y += range * 2;
            canMove = true;
        }

        if (canMove) {

            if (x >= 0 && y >= 0 && x < owner.gameController.GetLevelWidth () && y < owner.gameController.GetLevelHeight ()) {
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
	public bool MoveReticleNorthInitial(){
		if(reticle.cordY < owner.gameController.GetLevelHeight()){
			return MoveReticle (reticle.cordX, reticle.cordY + range);
		}
		return MoveReticleSouthInitial();
		
	}
	public bool MoveReticleSouthInitial(){
		if (reticle.cordY > 0) {
		if (reticle.cordY - range >= 0) {
			return MoveReticle (reticle.cordX, reticle.cordY - range);
		}
		}
		return MoveReticleNorthInitial ();
	}
	public bool MoveReticleEastInitial(){
		if (reticle.cordX  < owner.gameController.GetLevelWidth ()) {
			return MoveReticle (reticle.cordX + range, reticle.cordY);
		}
		return MoveReticleWestInitial ();
	}
	public bool MoveReticleWestInitial(){
		if (reticle.cordX  > 0) {
		if (reticle.cordX - range >= 0) {
			return MoveReticle (reticle.cordX - range, reticle.cordY);
		}
		}
		return MoveReticleEastInitial ();
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