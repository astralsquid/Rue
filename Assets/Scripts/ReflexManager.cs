using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReflexManager : MonoBehaviour {
	public GameController gameController;

	public float reflex = 3.0f;
	float current_reflex = 3.0f;

	public GameObject reflex_bar;

	float reflex_bar_max_width;
	float reflex_bar_max_height;

	bool reflex_stopped;

	public Color reflex_color;

	void Awake(){
		reflex_stopped = true;
		reflex_bar_max_height = reflex_bar.GetComponent<RectTransform>().sizeDelta.y;
		reflex_bar_max_width = reflex_bar.GetComponent<RectTransform>().sizeDelta.x;
		reflex_stopped = false;
		//reflex_bar.transform.position = new Vector3(0, Screen.height, -9);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		TickReflex();
	}

	public void StopReflex()
	{
		current_reflex = reflex;
		reflex_stopped = true;
		reflex_bar.GetComponent<Image>().color = new Color(0, 0, 0, 0);
	}

	public void ResetReflex()
	{

		current_reflex = reflex;
		if (!gameController.level_won)
		{
			reflex_stopped = false;
			reflex_bar.GetComponent<Image>().color = reflex_color;
		}
	}

	void TickReflex()
	{
		if (!reflex_stopped)
		{
			current_reflex -= Time.deltaTime;
			if (current_reflex > 0)
			{
				reflex_bar.GetComponent<RectTransform>().sizeDelta = new Vector2(reflex_bar_max_width * (current_reflex / reflex), reflex_bar_max_height);
			}
			else
			{
				current_reflex = reflex;
				if (reflex > .05f)
				{
					reflex = 0.9f * reflex;
				}
				reflex_bar.GetComponent<RectTransform>().sizeDelta = new Vector2(reflex_bar_max_width, reflex_bar_max_height);
				StartCoroutine(gameController.RunTurn());
				ResetReflex();
			}
		}
	}

}	
