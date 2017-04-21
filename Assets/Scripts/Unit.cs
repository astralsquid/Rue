using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {
	int hp;
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
    public int age;
	public Color myColor;
    // Use this for initialization
    void Awake(){
		hp = 1;
		gameController = GameObject.Find ("GameController").GetComponent<GameController>();
		cordX = 0;
		cordY = 0;
		aiming = false;
        weaponObject = Resources.Load("Prefabs/Weapon") as GameObject;
		age = Random.Range (15, 60);
		primaryWeapon = GameObject.Instantiate (weaponObject, transform.position, Quaternion.identity).GetComponent<Weapon> ();
		primaryWeapon.owner = this;
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
	public bool Move(int x, int y){
		if (x < gameController.GetLevelWidth () && y < gameController.GetLevelHeight () && x >= 0 && y >= 0 && gameController.occupationGrid[y*gameController.GetLevelWidth() + x] == 0) {
			primaryWeapon.Reset ();
			gameController.unitGrid [cordY * gameController.GetLevelWidth () + cordX] = null;
			transform.position = new Vector3 ((x - gameController.GetLevelWidth () / 2)-.5f, (y - gameController.GetLevelHeight () / 2)-.5f, transform.position.z);
			gameController.SetOccupation (cordX, cordY, 0);
			cordX = x; 
			cordY = y;
			gameController.SetOccupation (cordX, cordY, 1);
			gameController.unitGrid [cordY * gameController.GetLevelWidth () + cordX] = this;
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

	public bool MoveNorth (){
		return Move (cordX, cordY + 1);
	}
	public bool MoveSouth (){
		return Move (cordX,cordY - 1);
	}
	public bool MoveEast (){
		return Move (cordX + 1, cordY);
	}
	public bool MoveWest (){
		return Move (cordX - 1, cordY);
	}
	public bool MoveNorthEast (){
		return Move (cordX + 1, cordY + 1);
	}
	public bool MoveSouthEast (){
		return Move (cordX + 1, cordY - 1);
	}
	public bool MoveNorthWest (){
		return Move (cordX - 1, cordY + 1);
	}
	public bool MoveSouthWest (){
		return Move (cordX - 1, cordY - 1);
	}
	public bool MoveRandom(){
		return MoveAdjacent (Random.Range (-1, 2), Random.Range (-1, 2));
	}
	public bool MoveAdjacent(int x, int y){
		return Move (cordX + x, cordY + y);
	}


}
