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


    //flavor info
    public string name;
    public string wish;
    public int age;
	public Color myColor;
    // Use this for initialization
    void Awake(){
        moving = false;
		hp = 1;
		gameController = GameObject.Find ("GameController").GetComponent<GameController>();
		cordX = 0;
		cordY = 0;
		aiming = false;
        weaponObject = Resources.Load("Prefabs/Weapon") as GameObject;
		age = Random.Range (15, 60);

        GameObject primary_weapon_object = GameObject.Find("WeaponManager").GetComponent<WeaponManager>().GetRandomWeapon();
		primaryWeapon = GameObject.Instantiate (primary_weapon_object, new Vector3(transform.position.x, transform.position.y, -5), Quaternion.identity).GetComponent<Weapon> ();
		primaryWeapon.owner = this;
        primaryWeapon.transform.parent = transform;
        myColor = new Color (Random.Range (.2f, 1f), Random.Range (.2f, 1f), Random.Range (.2f, 1f));
		GetComponent<SpriteRenderer> ().color = myColor;
    }
    void Start () {
		alive = true;
        NameWizard nw = GameObject.Find("NameWizard").GetComponent<NameWizard>();
        name = nw.RandomName() + " " + nw.RandomLastName();
		gameController.unitList.Add (this);
    }

    // Update is called once per frame
    void Update () {

	}
	public bool Move(int x, int y, bool moveCamera){
		if (x < gameController.GetLevelWidth () && y < gameController.GetLevelHeight () && x >= 0 && y >= 0 && gameController.occupationGrid[y*gameController.GetLevelWidth() + x] == 0) {
			primaryWeapon.Reset ();
			gameController.unitGrid [cordY * gameController.GetLevelWidth () + cordX] = null;
			//transform.position = new Vector3 ((x - gameController.GetLevelWidth () / 2)-.5f, (y - gameController.GetLevelHeight () / 2)-.5f, transform.position.z);
			gameController.SetOccupation (cordX, cordY, 0);
			cordX = x; 
			cordY = y;
			gameController.SetOccupation (cordX, cordY, 1);
			gameController.unitGrid [cordY * gameController.GetLevelWidth () + cordX] = this;

            Vector3 movePosition = new Vector3((x - gameController.GetLevelWidth() / 2) - .5f, (y - gameController.GetLevelHeight() / 2) - .5f, transform.position.z);
            StartCoroutine(MoveToPosition(transform, movePosition, .2f));
            if (moveCamera)
            {
                movePosition = new Vector3((x - gameController.GetLevelWidth() / 2) - .5f, (y - gameController.GetLevelHeight() / 2) - .5f, GameObject.Find("Main Camera").transform.position.z);
                StartCoroutine(MoveToPosition(GameObject.Find("Main Camera").transform, movePosition, .2f));
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

    public IEnumerator MoveToPosition(Transform transform, Vector3 position, float timeToMove)
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
        moving = false;
    }
}
