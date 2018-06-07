using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void OnMouseClick() {
        SceneManager.LoadScene("main menu");
    }
}
