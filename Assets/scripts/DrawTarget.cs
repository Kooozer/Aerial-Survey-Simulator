using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTarget : MonoBehaviour {
    
    private int segments = 32;
    private float r = 5;
    LineRenderer line;
    SphereCollider mesh;

    // Use this for initialization
    void Start () {
        mesh = gameObject.GetComponent<SphereCollider>();
        mesh.radius = r;

        line = gameObject.GetComponent<LineRenderer>();
        line.positionCount = segments + 1;
        line.useWorldSpace = false;
        line.startColor = Color.black;
        line.endColor = Color.black;
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;
        line.loop = true;
        CreatePoints();
    }

    void CreatePoints()
    {
        float x;
        float y;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * r;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * r;

            line.SetPosition(i, new Vector3(x, y, 0));

            angle += (360f / segments);
        }
    }
}