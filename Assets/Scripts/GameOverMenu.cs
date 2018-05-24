using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour {

    GameManager GM;

	void Start () {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        GameObject.Find("Restart_B").GetComponent<Button>().onClick.AddListener(Restart);
        GameObject.Find("ReturnMain_B").GetComponent<Button>().onClick.AddListener(ReturnMain);
        GameObject.Find("Exit_B").GetComponent<Button>().onClick.AddListener(Exit);
    }
	
	void Update () {

    }

    void Restart()
    {
        GM.Cleanup();
        GM.LoadLevel();
    }

    void ReturnMain()
    {
        Destroy(GM.gameObject);
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    void Exit()
    {
        Application.Quit();
    }
}
