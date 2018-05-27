using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Aircraft : MonoBehaviour {
    private float diveRate;
    private float climbRate;
    private float yawRate;
    private float  rollRate;

    private float drift;

    private float targetDist;
    private float targetRadius;

    private float timeOnTask;
    private float pcComplete;
    public float taskTime;

    public Text timeRemainingText;
    public Text backButtonText;
    private List<string> backTextList;
    public VerticalLayoutGroup completionUI;

    public Vector3 targetVect;
    public int segments;
    public float xradius;
    public float yradius;
    LineRenderer circleLine;

    // Use this for initialization
    void Start() {

        completionUI.gameObject.SetActive(false);

        // Aircraft stats
        diveRate = 30;
        climbRate = 30;
        yawRate = 30;
        rollRate = 30;
        
        // Drift rate off centre
        drift = 10;
        
        // Set up target/reticle
        targetDist = 20;
        segments = 32;
        xradius = 1;
        yradius = 1;
        Ray targetRay = new Ray(transform.position, transform.TransformDirection(Vector3.forward));
        targetVect = targetRay.GetPoint(targetDist);
        circleLine = NewCircle(targetVect, xradius, yradius, segments);

        // Time in seconds to run for and start time
        taskTime = 60;
        timeOnTask = 0;

        // Completion text
        string[] sa = { "tea", "crumpets", "whiskey", "biscuits", "gin", "adoration", "abuse", "indifference", "paperwork", "disaster", "ice cream", "coffee" };
        backTextList = new List<string>(sa);
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
            transform.Rotate(Random.Range(-1, 1), Random.Range(-1, 1), 0);
        }
        // Else drift away from centre
        else
        {
            //TODO Vector3.RotateTowards();
        }


        // TODO Check if aircraft is pointing within target, draw reticle
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit);
        // TODO if hit target, all is fine, if not, make ret/tar red and give 3 sec countdown before failure
        //circleLine.SetPositions();


        // Time countdown
        timeOnTask += Time.deltaTime;
        pcComplete = (timeOnTask / taskTime) * 100;
        timeRemainingText.text = "Time on task: " + string.Format("{0, 4:000.0}s", timeOnTask) + "\nCompleted: " + string.Format("{0, 3:000}%", pcComplete);
        if (timeOnTask >= taskTime)
        {
            Time.timeScale = 0;
            timeOnTask = 0; // Make sure text reads exactly 0!
            ShowCompletionUI();
            // End of survey, make visible victory text & button

        }
    }

    // Shows the survey complete UI elements and generates the back button text
    void ShowCompletionUI()
    {
        completionUI.gameObject.SetActive(true);
        int backTextListLength = backTextList.Count;
        string randText1 = backTextList[Random.Range(0, backTextListLength)];
        string randText2 = backTextList[Random.Range(0, backTextListLength)];
        backButtonText.text = "Back for " + randText1 + " and " + randText2;
    }

    // Creates Line obj circle points
    LineRenderer NewCircle(Vector3 targetVect, float xradius, float yradius, int segments)
    {
        LineRenderer circ = GameObject.FindGameObjectWithTag("Reticle").GetComponent<LineRenderer>();
        circ.positionCount = segments + 1;
        circ.useWorldSpace = false;
        circ.startColor = Color.black;
        circ.startWidth = 0.5f;

        float x;
        float y;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * yradius;

            circ.SetPosition(i, new Vector3(targetVect.x + x, targetVect.y + y, targetVect.z));

            angle += (360f / segments);
        }
        return circ;
    }
}
