using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMover : MonoBehaviour {

    bool up = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

    void MoveButton()
    {
    }

    void OnMouseOver()
    {
        if (!up)
        {
            transform.Translate(new Vector3(.1f, .1f, 0));
            //transform.Find("Text").transform.Translate(new Vector3(.1f, .1f, 0));
            up = true;
        }
    }
    void OnMouseExit()
    {
        if (up)
        {
            transform.Translate(new Vector3(-.1f, -.1f, 0));
            //transform.Find("Text").transform.Translate(new Vector3(-.1f, -.1f, 0));
            up = false;
        }
    }
}
