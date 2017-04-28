using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticle : MonoBehaviour {
	public int cordX = 0;
	public int cordY = 0;
	public Sprite aim_reticle; 
	public Sprite lock_reticle;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetReticleAim(){
		GetComponent<SpriteRenderer> ().sprite = aim_reticle;
	}
	public void SetReticleTarget(){
		GetComponent<SpriteRenderer> ().sprite = lock_reticle;
	}
}
