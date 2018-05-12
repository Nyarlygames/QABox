using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public int mapmaxsizex = 0;
    public int mapmaxsizey = 0;
    public int mapcursizex = 0;
    public int mapcursizey = 0;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            SceneManager.LoadScene("Level01", LoadSceneMode.Single);
        }
    }
}
