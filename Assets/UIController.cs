using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    Text LevelName;

	void Start () {
        LevelName = GameObject.Find("LevelName_T").GetComponent<Text>();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setLevelName()
    {
        LevelName.text = PlayerPrefs.GetString("level");
    }
}
