using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Aircraft : MonoBehaviour {

    public GameObject persVarsObj;

    private float diveRate;
    private float climbRate;
    private float yawRate;
    private float  rollRate;

    private float drift;

    private float timeOnTask;
    private float pcComplete;
    private float taskTime;

    // Out of bounds - current seconds and allowed seconds
    private float oobTime;
    private float oobLimit;

    public Text timeRemainingText;

    public Text backButtonText;
    private List<string> backTextList;

    public Text resetButtonText;
    private List<string> resetTextList;

    public VerticalLayoutGroup endSceneUI;
    public Text endSceneBannerText;

    private Vector3 targetVect;
    public int segments;
    public float targetDist;
    private float retRadius;
    LineRenderer reticle;

    // Use this for initialization
    void Start() {

        endSceneUI.gameObject.SetActive(false);
        Time.timeScale = 1;
        persVarsObj = GameObject.Find("Persistent vars");
        PersistentVars persVars = persVarsObj.GetComponent<PersistentVars>();

        // Aircraft stats
        diveRate = persVars.diveRate;
        climbRate = persVars.climbRate;
        yawRate = persVars.yawRate;
        rollRate = persVars.rollRate;
        
        // Drift rate off centre
        drift = persVars.drift;

        // Reticle vars
        targetDist = 20;
        segments = 16;
        retRadius = 0.5f;

        // Set up reticle
        LineRenderer reticle = gameObject.GetComponent<LineRenderer>();
        reticle.positionCount = 5;
        reticle.useWorldSpace = false;
        reticle.startColor = Color.black;
        reticle.endColor = Color.black;
        reticle.startWidth = 0.1f;
        reticle.endWidth = 0.1f;
        reticle.loop = false;
        reticle.SetPositions(CreateCrossPoints(new Vector3(0, 0, transform.position.z + targetDist)));


        // Time in seconds to run for and start time
        taskTime = persVars.taskTime;
        timeOnTask = 0;
        oobTime = 3;

        // Back button text
        string[] bt = { "tea", "crumpets", "whiskey", "biscuits", "gin", "glory", "abuse", "indifference", "paperwork", "disaster", "cake", "medals" };
        backTextList = new List<string>(bt);

        // Reset button text
        string[] rt = { "glinty", "dark", "bright", "early", "late", "many JPEGs", "drunk", "happy" };
        resetTextList = new List<string>(rt);
    }
	
	// Update is called once per frame
	void Update () {
        // Aircraft control
		if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Rotate(diveRate * Time.deltaTime, 0.0f, 0.0f);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Rotate(-climbRate * Time.deltaTime, 0.0f, 0.0f);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(0, -yawRate * Time.deltaTime, rollRate * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(0, yawRate * Time.deltaTime, -rollRate * Time.deltaTime);
        }

        // Self-right roll
        transform.Rotate(0, 0, Mathf.Sign(transform.eulerAngles.z - 180) * (rollRate / 4) * Time.deltaTime);


        // Random drift away from centre
        // If dead level, nudge in random direction
        if (transform.localRotation.x == 0 && transform.localRotation.y == 0)
        {
            transform.Rotate(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1), 0);
        }
        // Else drift away from centre
        else
        {
            //TODO Vector3.RotateTowards();
        }


        // Check if aircraft is pointing within target
        // Draw reticle at distance to target
        RaycastHit hit;
        Ray targetRay = new Ray(transform.position, transform.forward);
        Color retCol;
        if (!Physics.Raycast(targetRay, out hit))
        {
            retCol = Color.red;
            retCol = Color.red;
            oobTime += Time.deltaTime;
        }
        else
        {
            retCol = Color.black;
            retCol = Color.black;
            oobTime = 0;
        }
        LineRenderer reticle = gameObject.GetComponent<LineRenderer>();
        reticle.startColor = retCol;
        reticle.endColor = retCol;


        // Survey time countdown
        timeOnTask += Time.deltaTime;
        pcComplete = (timeOnTask / taskTime) * 100;
        timeRemainingText.text = "Time on task: " + string.Format("{0, 4:000.0}s", timeOnTask) + "\nCompleted: " + string.Format("{0, 3:000}%", pcComplete);
        if (timeOnTask >= taskTime)
        {
            Time.timeScale = 0;
            timeOnTask = 0; // Make sure text reads exactly 0!
            ShowEndSceneUI("WIN");
        }

        // If out of bounds time is over the limit, complain and stop!
        if (oobTime > oobLimit)
        {
            Time.timeScale = 0;
            ShowEndSceneUI("FAIL");
        }
    }

    // Shows the survey complete UI elements and generates the back button text
    void ShowEndSceneUI(string cause)
    {
        if (!endSceneUI.IsActive())
        {
            endSceneUI.gameObject.SetActive(true);
            int backTextListLength = backTextList.Count;
            string backRandText1 = backTextList[UnityEngine.Random.Range(0, backTextListLength)];
            string backRandText2 = backTextList[UnityEngine.Random.Range(0, backTextListLength)];
            backButtonText.text = "Back for " + backRandText1 + " and " + backRandText2;

            if (string.Equals(cause, "WIN"))
            {
                endSceneBannerText.text = "TASK COMPLETE!";
                int resetTextListLength = resetTextList.Count;
                string resetRandText = resetTextList[UnityEngine.Random.Range(0, backTextListLength)];
                resetButtonText.text = "Too " + resetRandText + ", refly!";
            }
            else if (string.Equals(cause, "FAIL"))
            {
                endSceneBannerText.text = "TASK ABORTED";
            }
            else if (string.Equals(cause, "PAUSE"))
            {
                //                    /-\___/-\
                // THERE IS NO PAUSE    |̌' ̌'|
                //                       \"/
            }
        }
    }

    // Takes a Vector3 and returns a Vector3 array describing a circle around the point in the XY plane
    private Vector3[] CreateCirclePoints(Vector3 pos)
    {
        float x, y, z;
        Vector3[] spokes = new Vector3[segments + 1];

        float angle = 20f;


        for (int i = 0; i < (segments + 1); i++)
        {
            x = pos.x + Mathf.Sin(Mathf.Deg2Rad * angle) * retRadius;
            y = pos.y + Mathf.Cos(Mathf.Deg2Rad * angle) * retRadius;
            z = pos.z;

            spokes[i] = (new Vector3(x, y, z));

            angle += (360f / segments);
        }
        return spokes;
    }

    private Vector3[] CreateCrossPoints(Vector3 pos)
    {
        Vector3[] points = new Vector3[5];
        points[0] = new Vector3(pos.x + retRadius, pos.y, pos.z);
        points[1] = new Vector3(pos.x - retRadius, pos.y, pos.z);
        points[2] = new Vector3(pos.x, pos.y, pos.z);
        points[3] = new Vector3(pos.x, pos.y + retRadius, pos.z);
        points[4] = new Vector3(pos.x, pos.y - retRadius, pos.z);
        return points;
    }
}
