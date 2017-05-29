using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour {
    public float time_to_move = 5.0f;
    public GameController gameController;
    public bool elevator_lowered = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator Raise( bool take_player)
    {
        float time_to_raise = time_to_move;
        if (take_player)
        {
            gameController.playerInputController.DisableInput();
        }
        float start_y = 25;
        GameObject middle_tile = gameController.GetMiddleTile();
        Vector3 tp = middle_tile.transform.position;
        transform.position = new Vector3(tp.x, start_y, -2);

        float t = 0.0f;
        while (t < time_to_raise)
        {
            middle_tile = gameController.GetMiddleTile();
            tp = middle_tile.transform.position;
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(new Vector3(middle_tile.transform.position.x, middle_tile.transform.position.y, -2), new Vector3(tp.x, start_y, 0), t / time_to_raise);
            if (take_player)
            {
                gameController.playerInputController.playerUnit.transform.position = transform.position;
            }
            yield return null;
        }
        elevator_lowered = false;

        gameController.playerInputController.EnableInput();
    }

    public IEnumerator Lower(bool take_player)
    {
        float time_to_lower = time_to_move;
        gameController.StopReflex();
        if (take_player)
        {
            gameController.playerInputController.DisableInput();
        }
        float start_y = 25;
        GameObject middle_tile = gameController.GetMiddleTile();
        Vector3 tp = middle_tile.transform.position;
        transform.position = new Vector3(tp.x, start_y, -2);


        float t = 0.0f;
        while (t < time_to_lower)
        {
            middle_tile = gameController.GetMiddleTile();
            tp = middle_tile.transform.position;
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(new Vector3(tp.x, start_y, 0), new Vector3(middle_tile.transform.position.x, middle_tile.transform.position.y, -2), t/time_to_lower);
            if (take_player)
            {
                gameController.playerInputController.playerUnit.transform.position = transform.position;
            }
            yield return null;
        }
        elevator_lowered = true;
        gameController.playerInputController.EnableInput();

    }
}
