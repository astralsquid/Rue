using UnityEngine;
using System.Collections;

public class LineDrawer : MonoBehaviour
{

    public GameObject source;
    public GameObject destination;
    LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        //DrawLine();
    }
    private void DrawLine()
    {
		Vector3 unitPosition = new Vector3(source.transform.position.x, source.transform.position.y, source.transform.position.z);
		Vector3 panelPosition = new Vector3(destination.transform.position.x , destination.transform.position.y , destination.transform.position.z);
        lineRenderer.SetPosition(0, unitPosition);
        lineRenderer.SetPosition(1, panelPosition);
    }

	public void DrawLine(GameObject source, GameObject destination){
		lineRenderer.SetPosition (0, source.transform.position);
		lineRenderer.SetPosition (1, destination.transform.position);
	}
}