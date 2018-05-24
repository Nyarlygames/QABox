using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour {

    GameManager GM;

    void Start ()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        GameObject.Find("Back_B").GetComponent<Button>().onClick.AddListener(Close);
    }
	
	void Update () {

    }

    void Close()
    {
        SceneManager.UnloadSceneAsync("OptionsMenu");
    }
}
