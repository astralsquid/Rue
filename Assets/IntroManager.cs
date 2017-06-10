using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour {
	public GameController gameController;
	public Unit playerUnit;
	public PlayerInputController playerInputController;
	public ScreenFader screenFader;
	bool endSequence;
	bool highlight;
	public Color color1;
	public Color color2;
	float duration = 1;
	// Use this for initialization
	void Start () {
		endSequence = false;
		highlight = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(playerUnit.cordY == gameController.levelHeight - 1 && playerUnit.cordX == gameController.levelWidth/2){
			//knock knock
			if (!endSequence) {
				endSequence = true;
				StartCoroutine(EndSquence ());
			}
		}
		if (highlight) {
			float t = Mathf.PingPong (Time.time, duration) / duration;
			HighlightSquare(1,20, Color.Lerp (color1, color2, t));
		}
		
	}

	IEnumerator EndSquence(){
		highlight = false;
		HighlightSquare(1,20, color1);
		GetComponent<AudioSource> ().Play ();
		playerInputController.DisableInput();
		screenFader.FadeToBlack ();
		yield return new WaitForSeconds(4);
		SceneManager.LoadScene ("FrontDesk");
	}

	void HighlightSquare(int x, int y, Color c){
		gameController.SetTileColor(x,y,c);
	}
}
