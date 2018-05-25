using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public MapSave map = new MapSave();
    public MapLoader maploader;
    public GameObject Player;
    public PlayerSave PlayerSave;
    public CameraController Camera;
    AsyncOperation asyncLoadLevel;
    public List<GameObject> TilesGO = new List<GameObject>();
    public List<GameObject> ObjectsGO = new List<GameObject>();
    public int ZBackground = 3;
    public int ZGround = 2;
    public int ZObject = 1;
    public int ZPlayer = 0;
    public int ZCamera = -20;
    public Dictionary<string, Sprite> SpriteList = new Dictionary<string, Sprite>();
    public bool loading = false;

    
    void Start () {
        DontDestroyOnLoad(this);
        SpriteList.Add("Block", Resources.Load<Sprite>("Map/Tileset/Block"));
        SpriteList.Add("Spawner", Resources.Load<Sprite>("Map/Tileset/Spawner"));
        SpriteList.Add("Panel_DontPlay", Resources.Load<Sprite>("Map/Tileset/Panel_DontPlay"));
        SpriteList.Add("Panel_Play", Resources.Load<Sprite>("Map/Tileset/Panel_Play"));
        SpriteList.Add("Panel_Options", Resources.Load<Sprite>("Map/Tileset/Panel_Options"));
        SpriteList.Add("Panel_Next", Resources.Load<Sprite>("Map/Tileset/Panel_Next"));
        SpriteList.Add("CamLock", Resources.Load<Sprite>("Map/Tileset/CamLock"));
        SpriteList.Add("CamUnlock", Resources.Load<Sprite>("Map/Tileset/CamUnlock"));
        SpriteList.Add("LevelVals", Resources.Load<Sprite>("Map/Tileset/LevelVals"));
        SpriteList.Add("Ladder", Resources.Load<Sprite>("Map/Tileset/Ladder"));
        SpriteList.Add("Player", Resources.Load<Sprite>("Map/Tileset/Player"));
        SpriteList.Add("Background1", Resources.Load<Sprite>("Map/Tileset/Background1"));
        SpriteList.Add("Background2", Resources.Load<Sprite>("Map/Tileset/Background2"));
    }

    public void Cleanup()
    {
        PlayerSave = null;
        map = new MapSave();
        maploader = null;
        TilesGO.Clear();
        ObjectsGO.Clear();
        Destroy(Player);
    }
    
	void Update () {

        if (loading == true)
        {
            if (asyncLoadLevel.isDone)
            {
                loading = false;
                Debug.Log("loaded");
                maploader = GameObject.Find("MapLoader").GetComponent<MapLoader>();
                maploader.LoadMap(PlayerPrefs.GetString("map"), map);
                maploader.LoadMapGO(map);
            }
            else
            {
                Debug.Log("loading");
            }
        }
        // cheat next level
        if (Input.GetKeyDown(KeyCode.N))
        {
            PlayerPrefs.SetString("map", PlayerPrefs.GetString("nextmap"));
            Cleanup();
            LoadLevel();
        }
    }

    public void LoadLevel()
    {
        if (loading == false)
        {
            StartCoroutine(AddLevel());
            loading = true;
        }
    }

    IEnumerator AddLevel()
    {
        asyncLoadLevel = SceneManager.LoadSceneAsync("Levels", LoadSceneMode.Single);
        while (!asyncLoadLevel.isDone)
        {
            yield return null;
        }
    }
}
