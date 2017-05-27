using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour {

    public GameController gameController;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator Raise(float time_to_raise)
    {
        gameController.playerInputController.DisableInput();
        float start_y = 25;
        GameObject middle_tile = gameController.GetMiddleTile();
        Vector3 tp = middle_tile.transform.position;
        transform.position = new Vector3(tp.x, start_y, 0);

        float t = 0.0f;
        while (t < time_to_raise)
        {
            middle_tile = gameController.GetMiddleTile();
            tp = middle_tile.transform.position;
            t += Time.deltaTime;
            transform.position = Vector3.Lerp( middle_tile.transform.position, new Vector3(tp.x, start_y, 0), t / time_to_raise);
            yield return null;
        }
        gameController.playerInputController.EnableInput();
    }

    public IEnumerator Lower(float time_to_lower, bool raise_after)
    {
        gameController.playerInputController.DisableInput();
        float start_y = 25;
        GameObject middle_tile = gameController.GetMiddleTile();
        Vector3 tp = middle_tile.transform.position;
        transform.position = new Vector3(tp.x, start_y, 0);


        float t = 0.0f;
        while (t < time_to_lower)
        {
            middle_tile = gameController.GetMiddleTile();
            tp = middle_tile.transform.position;
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(new Vector3(tp.x, start_y, 0), middle_tile.transform.position, t/time_to_lower);
            gameController.playerInputController.playerUnit.transform.position = transform.position;
            yield return null;
        }
        if (raise_after)
        {
            Debug.Log("raising now!");
            StartCoroutine(Raise(time_to_lower));
        }
        else
        {
            gameController.playerInputController.EnableInput();
        }

    }
}
