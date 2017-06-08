using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour {
	public GameController gameController;
	public Unit playerUnit;
	public PlayerInputController playerInputController;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(playerUnit.cordY == gameController.levelHeight - 1 && playerUnit.cordX == gameController.levelWidth/2){
			//knock knock
			playerInputController.DisableInput();
		}
		
	}
}
