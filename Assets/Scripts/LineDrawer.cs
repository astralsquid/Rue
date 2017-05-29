using UnityEngine;
using System.Collections;

public class LineDrawer : MonoBehaviour
{

    public GameObject unit;
    public GameObject panel;
    LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        DrawLine();
    }
    private void DrawLine()
    {
        Vector3 unitPosition = new Vector3(unit.transform.position.x, unit.transform.position.y, unit.transform.position.z);
        Vector3 panelPosition = new Vector3(panel.transform.position.x - 1, panel.transform.position.y - 1, panel.transform.position.z);
        lineRenderer.SetPosition(0, unitPosition);
        lineRenderer.SetPosition(1, panelPosition);
    }
}