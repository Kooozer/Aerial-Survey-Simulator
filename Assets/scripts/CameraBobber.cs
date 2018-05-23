using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBobber : MonoBehaviour {
    private float vertAccelMax, horzAccelMax, vertDriftPeriod, horzDriftPeriod;

    // Use this for initialization
    void Start () {
        vertAccelMax = 0.001f;
        horzAccelMax = 0.0005f;
        vertDriftPeriod = 17.0f;
        horzDriftPeriod = 23.0f;
    }
	
	// Update is called once per frame
	void Update () {
        // Gently bob camera to simulate flight
        transform.position += transform.up * Bob(vertAccelMax, vertDriftPeriod);
        transform.position += transform.right * Bob(horzAccelMax, horzDriftPeriod);
    }

    // Given a max accel. and time period, gives the current accel. based on a sine wave
    float Bob(float accel, float period)
    {
        // Gently bob camera to simulate flight
        float currentPeriod = Time.time % period;
        float radPeriod = (currentPeriod / period) * (2 * Mathf.PI);
        return accel * Mathf.Cos(radPeriod);
    }
}
