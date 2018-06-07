using UnityEngine;

public class InstructionsButton : MonoBehaviour {

    public GameObject panel;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void OnMouseClick()
    {
        panel.SetActive(true);
    }
}
