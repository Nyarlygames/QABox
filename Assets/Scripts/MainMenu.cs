using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    Resolution[] resolutions;
    public Dropdown dropdownMenu;
    GameManager GM;

    // Use this for initialization
    void Start () {
       /* resolutions = Screen.resolutions;
        dropdownMenu.onValueChanged.AddListener(delegate { Screen.SetResolution(resolutions[dropdownMenu.value].width, resolutions[dropdownMenu.value].height, true); });
        
        int value = 0;
        foreach (Resolution res in Screen.resolutions)
        {
            if ((res.width > 800) && (res.height > 600))
            {
                Dropdown.OptionData opt = new Dropdown.OptionData();
                opt.text = res.width + "/" + res.height;
                dropdownMenu.value = value;
                dropdownMenu.options.Add(opt);
                value++;
            }
        }*/
        GameObject.Find("Launch_B").GetComponent<Button>().onClick.AddListener(LaunchLevel);
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void LaunchLevel()
    {
        PlayerPrefs.SetString("level", "01");
        PlayerPrefs.SetString("map", "Assets/Resources/Map/Maps01.tmx");
        GM.LoadLevel();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
