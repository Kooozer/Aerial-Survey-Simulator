using UnityEngine;

public class BackInstructions : MonoBehaviour {

    public GameObject panel;

	// Use this for initialization
	void Start () {
        panel.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
    }

    public void OnMouseClick()
    {
        panel.SetActive(false);
    }
}
