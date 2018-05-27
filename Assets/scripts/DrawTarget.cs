using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTarget : MonoBehaviour {
    
    private int segments = 32;
    private float xradius = 5;
    private float yradius = 5;
    LineRenderer circleLine;

    // Use this for initialization
    void Start () {
        circleLine = gameObject.GetComponent<LineRenderer>();

        circleLine.positionCount = segments + 1;
        circleLine.useWorldSpace = false;
        circleLine.startColor = Color.black;
        circleLine.startWidth = 0.5f;
        CreatePoints();
    }

    void CreatePoints()
    {
        float x;
        float y;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * yradius;

            circleLine.SetPosition(i, new Vector3(x, y, 0));

            angle += (360f / segments);
        }
    }
}