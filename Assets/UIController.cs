using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    Text LevelName;
    public GameObject LoadingScreen;

	void Start () {
        LevelName = GameObject.Find("LevelName_T").GetComponent<Text>();
        LoadingScreen = GameObject.Find("LoadingScreen");
        LoadingScreen.SetActive(false);
    }
	
	void Update () {
		
	}

    public void setLevelName()
    {
        LevelName.text = PlayerPrefs.GetString("level");
    }
}
