using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public MapSave map = new MapSave();
    public MapLoader maploader;
    public GameObject Player;
    public GameObject Camera;
    AsyncOperation asyncLoadLevel;
    public List<GameObject> TilesGO = new List<GameObject>();
    public int ZGround = 0;
    public int ZObject = 0;
    public int ZCamera = -20;
    public Dictionary<string, Sprite> SpriteList = new Dictionary<string, Sprite>();
    public bool loading = false;


    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this);
        SpriteList.Add("Block", Resources.Load<Sprite>("Map/Tileset/Block"));
        SpriteList.Add("Spawner", Resources.Load<Sprite>("Map/Tileset/Spawner"));
        SpriteList.Add("Panel_DontPlay", Resources.Load<Sprite>("Map/Tileset/Panel_Play"));
        SpriteList.Add("Panel_Play", Resources.Load<Sprite>("Map/Tileset/Panel_DontPlay"));
        SpriteList.Add("Panel_Options", Resources.Load<Sprite>("Map/Tileset/Panel_Options"));
        SpriteList.Add("CamLock", Resources.Load<Sprite>("Map/Tileset/CamLock"));
    }
	
	// Update is called once per frame
	void Update () {

        if (loading == true)
        {
            if (asyncLoadLevel.isDone)
            {
                loading = false;
                Debug.Log("loaded");
                maploader = GameObject.Find("MapLoader").GetComponent<MapLoader>();
                maploader.LoadMap(maploader.mapfile, map);
                maploader.LoadMapGO(map);
            }
            else
            {
                Debug.Log("loading");
            }
        }
    }

    public void LoadLevel(string oldscene, string newscene)
    {
        if ((SceneManager.GetActiveScene().name == oldscene) && (loading == false))
        {
            StartCoroutine(AddLevel(newscene));
            loading = true;
        }
    }

    IEnumerator AddLevel(string level)
    {
        asyncLoadLevel = SceneManager.LoadSceneAsync(level, LoadSceneMode.Single);
        while (!asyncLoadLevel.isDone)
        {
            yield return null;
        }
    }
}
