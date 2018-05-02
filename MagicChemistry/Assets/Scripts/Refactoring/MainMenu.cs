using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public int startSceneIndex;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		
	}

    public void Quit()
    {
        Application.Quit();
    }

    public void Begin()
    {
        SceneManager.LoadScene(startSceneIndex);
    }
}
