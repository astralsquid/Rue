using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {
	int hp;
    public bool moving;
	public bool alive;
	public bool aiming;
	public int cordX;
	public int cordY;
	public GameObject weaponObject;
	public Reticle reticle;
	public Weapon primaryWeapon;
	public GameController gameController;
	float time_to_move = .25f;

    //flavor info
    public string name;
    public string wish;
    public int age;
	public Color my_color;
	public bool randomInfo;
    // Use this for initialization
    void Awake(){
        moving = false;
		hp = 1;
		gameController = GameObject.Find ("GameController").GetComponent<GameController>();
		cordX = 0;
		cordY = 0;
		aiming = false;
        weaponObject = Resources.Load("Prefabs/Weapon") as GameObject;
		if (randomInfo) {
			age = Random.Range (15, 60);
			my_color = new Color (Random.Range (.2f, 1f), Random.Range (.2f, 1f), Random.Range (.2f, 1f));
		}
		GetComponent<SpriteRenderer> ().color = my_color;
    }
    void Start () {
		alive = true;
		if (randomInfo) {
			if (GameObject.Find ("NameWizard") != null) {
				NameWizard nw = GameObject.Find ("NameWizard").GetComponent<NameWizard> ();
				name = nw.RandomName () + " " + nw.RandomLastName ();
			} else { 
				name = "nameless";
			}
		}
		gameController.unitList.Add (this);
    }

    public void GrantRandomWeapon()
    {
        GameObject primary_weapon_object = GameObject.Find("WeaponManager").GetComponent<WeaponManager>().GetRandomWeapon();
        primaryWeapon = GameObject.Instantiate (primary_weapon_object, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity).GetComponent<Weapon> ();

        primaryWeapon.owner = this;
		primaryWeapon.transform.position = transform.position;
			
        primaryWeapon.transform.parent = transform;
    }

    // Update is called once per frame
    void Update () {

	}

    public void SetColor(Color c)
    {
        my_color = c;
        GetComponent<SpriteRenderer>().color = my_color;
    }

    public void SetName(string n)
    {
        name = n;
    }

    public void SetWish(string w)
    {
        wish = w;
    }
	public bool Move(int x, int y, bool moveCamera){
		if (x < gameController.GetLevelWidth () && y < gameController.GetLevelHeight () && x >= 0 && y >= 0 && gameController.occupationGrid[y*gameController.GetLevelWidth() + x] == 0) {
			if(primaryWeapon!=null){primaryWeapon.Reset ();}
			gameController.unitGrid [cordY * gameController.GetLevelWidth () + cordX] = null;
			gameController.SetOccupation (cordX, cordY, 0);
			Vector3 movePosition;
			if (gameController.GetTile (x, y) != null) {
				 movePosition = gameController.GetTile (x, y).transform.position;
			} else {
				 movePosition = new Vector3((x - gameController.GetLevelWidth() / 2) - .5f, (y - gameController.GetLevelHeight() / 2) - .5f, 0);
			}
			StartCoroutine(MoveToPosition(transform, movePosition, time_to_move, x, y));
            if (moveCamera)
            {
                movePosition = new Vector3((x - gameController.GetLevelWidth() / 2) - .5f, (y - gameController.GetLevelHeight() / 2) - .5f, GameObject.Find("Main Camera").transform.position.z);
				StartCoroutine(MoveToPosition(GameObject.Find("Main Camera").transform, movePosition, time_to_move,x, y));
            }
            return true;
		}
		return false;
	}

    public bool Teleport(int x, int y, bool moveCamera)
    {
        if (x < gameController.GetLevelWidth() && y < gameController.GetLevelHeight() && x >= 0 && y >= 0 && gameController.occupationGrid[y * gameController.GetLevelWidth() + x] == 0)
        {
            gameController.unitGrid[cordY * gameController.GetLevelWidth() + cordX] = null;
            gameController.SetOccupation(cordX, cordY, 0);
            cordX = x;
            cordY = y;
            gameController.SetOccupation(cordX, cordY, 1);
            gameController.unitGrid[cordY * gameController.GetLevelWidth() + cordX] = this;

			Vector3 movePosition;
			if (gameController.GetTile (x, y) != null) {
				movePosition = gameController.GetTile (x, y).transform.position;
			} else {
				movePosition = new Vector3((x - gameController.GetLevelWidth() / 2) - .5f, (y - gameController.GetLevelHeight() / 2) - .5f, 0);
			}
            transform.position = movePosition;
            if (moveCamera)
            {
                movePosition = new Vector3((x - gameController.GetLevelWidth() / 2) - .5f, (y - gameController.GetLevelHeight() / 2) - .5f, GameObject.Find("Main Camera").transform.position.z);
				StartCoroutine(MoveToPosition(GameObject.Find("Main Camera").transform, movePosition, time_to_move, x, y));
            }
            return true;
        }
        return false;
    }


    public void TakeDamage(int d){
		hp -= d;
		if (hp < 1) {
			Die ();
		}
	}

	public void Die(){
		GetComponent<SpriteRenderer> ().color = Color.yellow;
		alive = false;
        gameController.occupationGrid[cordY * gameController.GetLevelWidth() + cordX] = 0;
		gameController.SetUnit (cordX, cordY, null);
		gameController.unitList.Remove (this);
		primaryWeapon.Reset ();
		GetComponent<SpriteRenderer> ().color = new Color (0f, 0f, 0f, 0f);
	}

	public bool MoveNorth (bool moveCamera){
		return Move (cordX, cordY + 1, moveCamera);
	}
	public bool MoveSouth (bool moveCamera)
    {
		return Move (cordX,cordY - 1, moveCamera);
	}
	public bool MoveEast (bool moveCamera)
    {
		return Move (cordX + 1, cordY, moveCamera);
	}
	public bool MoveWest (bool moveCamera)
    {
		return Move (cordX - 1, cordY, moveCamera);
	}
	public bool MoveNorthEast (bool moveCamera)
    {
		return Move (cordX + 1, cordY + 1, moveCamera);
	}
	public bool MoveSouthEast (bool moveCamera)
    {
		return Move (cordX + 1, cordY - 1, moveCamera);
	}
	public bool MoveNorthWest (bool moveCamera)
    {
		return Move (cordX - 1, cordY + 1, moveCamera);
	}
	public bool MoveSouthWest (bool moveCamera)
    {
		return Move (cordX - 1, cordY - 1, moveCamera);
	}
	public bool MoveRandom(bool moveCamera)
    {
		return MoveAdjacent (Random.Range (-1, 2), Random.Range (-1, 2), moveCamera);
	}
	public bool MoveAdjacent(int x, int y, bool moveCamera)
    {
		return Move (cordX + x, cordY + y, moveCamera);
	}

	public IEnumerator MoveToPosition(Transform transform, Vector3 position, float timeToMove, int x, int y)
    {

        moving = true;
        var currentPos = transform.position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
		cordX = x; 
		cordY = y;
		gameController.SetOccupation (cordX, cordY, 1);
		gameController.unitGrid [cordY * gameController.GetLevelWidth () + cordX] = this;
        moving = false;
    }
    public void LoadWeapon(WeaponCereal wc)
    {
        GameObject primary_weapon_object;
        switch (wc.myType)
        {
            case Weapon.WeaponType.sword:
                primary_weapon_object = GameObject.Find("WeaponManager").GetComponent<WeaponManager>().GetSword();
                Destroy(primaryWeapon);
			primaryWeapon = GameObject.Instantiate(primary_weapon_object, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity).GetComponent<Sword>();
                break;
            case Weapon.WeaponType.spear:
                primary_weapon_object = GameObject.Find("WeaponManager").GetComponent<WeaponManager>().GetSpear();
                Destroy(primaryWeapon);
			primaryWeapon = GameObject.Instantiate(primary_weapon_object, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity).GetComponent<Spear>();
                break;
			case Weapon.WeaponType.skyripper:
				primary_weapon_object = GameObject.Find("WeaponManager").GetComponent<WeaponManager>().GetSkyRipper();
				Destroy(primaryWeapon);
			primaryWeapon = GameObject.Instantiate(primary_weapon_object, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity).GetComponent<SkyRipper>();
				break;
            default:
                break;
        }
        primaryWeapon.owner = this;
        primaryWeapon.transform.parent = transform;
    }
}
