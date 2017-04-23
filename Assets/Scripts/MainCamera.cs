/*
 * CameraScript manages user input for the camera, 
 * and adjusts orthographic size and other variables
 * depending on resolution
 * */
using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {
	public bool followPlayer;
    public PlayerInputController playerInputController;
	//player to track
	public GameObject player;

	//level controller and script
	GameObject levelController;
	GameController levelControllerScript;

	//number of rows and columns in the level grid
	private int mapRows;
	private int mapCols;

	//camera dimensions
	//private float camHeight;
	//private float camWidth;
	//the actual camera
	public Camera cam;

	//cuts camera semsitivity, should be adjustable in the future
	public float originalCameraSpeedCutBy = 10f;
	public float cameraSpeedCutBy;

	//pixels per unit and scale, needed for camera 
	public float ppuScale = 1f;
	public int ppu = 32;

	//camera zoom boundaries
	private float cameraCurrentZoom;
	private float cameraZoomMax;
	public float cameraZoomMin = 0;
	//used to view currentCameraZoom without changing it
	public float czm;

	//last position of cursor, used for panning the map
	private Vector3 lastPosition;

	//UIBank uiBank;

	void Awake(){
	}


	void Start ()
	{
		//connect with level controller and script
		levelController = GameObject.Find ("GameController");
		levelControllerScript = levelController.GetComponent<GameController> ();

		//find the rows and columns in of the level grid
		mapRows = levelControllerScript.GetLevelHeight();
		mapCols = levelControllerScript.GetLevelWidth();

		//calculate maximum camera zoom //Orthographic size = ((Vert Resolution)/(PPUScale * PPU)) * 0.5
		cameraCurrentZoom = ((Screen.height)/(ppuScale * 32))*0.5000f;
		cameraZoomMax = cameraCurrentZoom*2;
		Camera.main.orthographicSize = cameraCurrentZoom;

		//make the camera less sensitive as we zoom in
		cameraSpeedCutBy = originalCameraSpeedCutBy * (cameraCurrentZoom / cameraZoomMax);

		//czm is for viewing only
		czm = cameraCurrentZoom;
	}

	void LateUpdate()
	{

		mapRows = levelControllerScript.GetLevelHeight();
		mapCols = levelControllerScript.GetLevelWidth();


		czm = cameraCurrentZoom;

		//clamps camera position so we don't go off the board 
		Vector3 v3 = transform.position;
		v3.x = Mathf.Clamp(v3.x, - (mapCols / 2) - .5f, (mapCols / 2) + .5f);
		v3.y = Mathf.Clamp(v3.y, - (mapRows / 2) - .5f, (mapRows / 2) + .5f);
		transform.position = v3;

		//camera zooming

			if (Input.GetAxis ("Mouse ScrollWheel") < 0) { // back
				if (cameraCurrentZoom < cameraZoomMax) {
					cameraCurrentZoom += 1;
					Camera.main.orthographicSize = Mathf.Max (Camera.main.orthographicSize + 1);
				}
			} else if (Input.GetAxis ("Mouse ScrollWheel") > 0) { // forward
				if (cameraCurrentZoom > 2) {
					cameraCurrentZoom -= 1;
					Camera.main.orthographicSize = Mathf.Min (Camera.main.orthographicSize - 1);
				}
			}

		//ensure camera sensitivity adjusts for zoom level
		//cameraSpeedCutBy = (1f - ((cameraCurrentZoom) / cameraZoomMax));
		cameraSpeedCutBy = (cameraCurrentZoom+(cameraZoomMax*2)) / cameraZoomMax;
	}

	void Update()
	{
        if (playerInputController.cameraLocked == false)
        {
            float xAxisValue = Input.GetAxis("CameraHorizontal") / cameraSpeedCutBy;
            float yAxisValue = Input.GetAxis("CameraVertical") / cameraSpeedCutBy;
            if (Camera.current != null)
            {
                Camera.current.transform.Translate(new Vector3(xAxisValue, yAxisValue, 0.0f));
            }
        }
    }
}

