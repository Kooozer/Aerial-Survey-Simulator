using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickPlayButton : MonoBehaviour {

    private GameObject persVars;

	// Use this for initialization
	void Start () {
        persVars = GameObject.Find("Persistent vars");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnMouseClick()
    {
        PersistentVars persVarsScript = persVars.GetComponent<PersistentVars>();
        persVarsScript.taskTime = persVarsScript.taskTimeQP;    // Bit weird but prevents an if loop on every update for persVars and keeps all vars there. Probably a better way!
        SceneManager.LoadScene("survey");
    }
}
