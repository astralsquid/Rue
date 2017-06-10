using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontDeskManager : MonoBehaviour {
	public GameController gameController;
	bool highlight = false;
	public Color color1;
	public Color color2;
	public float duration;
	public Unit playerUnit;
	public Unit mistress;
	public AudioSource dingClip;
	public AudioSource musicClip;
	bool sequenceTriggered;
	bool sequenceDone;
	public PlayerInputController playerInputController;
	// Use this for initialization
	void Start () {
		StartCoroutine(Setup ());
		sequenceTriggered = false;
		sequenceDone = false;
		dingClip.volume = PlayerPrefs.GetFloat ("effects");
		musicClip.volume = PlayerPrefs.GetFloat ("music");
		musicClip.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		if (highlight) {
			float t = Mathf.PingPong (Time.time, duration) / duration;
			HighlightSquare(4,10, Color.Lerp (color1, color2, t));
		}
		if (playerUnit.cordX == 4 && playerUnit.cordY == 10 && !sequenceTriggered) {
			highlight = false;
			HighlightSquare (4, 10, color1);
			sequenceTriggered = true;
			mistress.cordX = 12;
			mistress.cordY = 12;
			dingClip.Play ();
			StartCoroutine (Sequence ());
		}
	}

	IEnumerator Setup(){
		yield return new WaitForSeconds (1);
		highlight = true;
		for (int w = 0; w < gameController.GetLevelWidth (); w++) {
			gameController.SetOccupation (w, gameController.GetLevelHeight () - 2, 1);
		}
	}

	void HighlightSquare(int x, int y, Color c){
		gameController.SetTileColor(x,y,c);
	}
					
	IEnumerator Sequence(){
		yield return new WaitForSeconds(1f);
		mistress.Move(12,12,false);

		playerInputController.DisableInput ();
		while (!sequenceDone) {
			yield return new WaitForSeconds(.5f);
			if (mistress.cordX == playerUnit.cordX) {
				sequenceDone = true;
			} else {
				mistress.MoveWest (false);
			}
		}

	}
		


}
