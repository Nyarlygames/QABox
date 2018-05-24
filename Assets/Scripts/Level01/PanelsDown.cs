using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelsDown : MonoBehaviour {

    PlayerController PControl;
    GameObject Ladder;
    GameManager GM;

    void Start() {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        PControl = gameObject.GetComponent<PlayerController>();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (Ladder != null)
            {
                SpriteRenderer LadderSprite = Ladder.GetComponent<SpriteRenderer>();
                SpriteRenderer PlayerSprite = gameObject.GetComponent<SpriteRenderer>();


                if ((LadderSprite.bounds.center.x - LadderSprite.bounds.extents.x + (PlayerSprite.size.x / 2) <= (PControl.PTransform.position.x)) && 
                    (LadderSprite.bounds.center.x + LadderSprite.bounds.extents.x - (PlayerSprite.size.x / 2) >= (PControl.PTransform.position.x)))
                {
                    switch (Ladder.GetComponent<ObjectController>().objSave.modifiers["panel"])
                    {
                        case "options":
                            SceneManager.LoadScene("OptionsMenu", LoadSceneMode.Additive);
                            break;
                        case "play":
                            SceneManager.LoadScene("GameOverMenu", LoadSceneMode.Single);
                            break;
                        case "nextlevel":
                            PlayerPrefs.SetString("map", PlayerPrefs.GetString("nextmap"));
                            GM.Cleanup();
                            GM.LoadLevel();
                            break;
                        default:
                            Debug.Log("woops");
                            break;
                    }
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.name.Contains("Ladder"))
            Ladder = coll.gameObject;
    }
    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.collider.name.Contains("Ladder"))
            Ladder = null;
    }
}
