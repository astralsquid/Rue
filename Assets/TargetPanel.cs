using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetTargetSprite(SpriteRenderer targetRenderer)
    {
        Debug.Log("sprite changed");
        transform.Find("TargetAvatar").GetComponent<SpriteRenderer>().sprite = targetRenderer.sprite;
        transform.Find("TargetAvatar").GetComponent<SpriteRenderer>().color = targetRenderer.color;
    }
}
