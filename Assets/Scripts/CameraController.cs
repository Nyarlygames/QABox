using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    GameManager GM;
    Vector3 CamPos = Vector3.zero;
    PlayerController PControl;
    Transform CTransform;
    Rigidbody2D CRigidb;
    BoxCollider2D CBoxColl;
    Camera Cam;
    bool locked = false;
    bool lockedvert = true;
    string lockid = "";
    string lockidvert = "";
    List<ObjectController> forbids = new List<ObjectController>();
    List<ObjectController> forbidsvert = new List<ObjectController>();

    // Use this for initialization
    void Start ()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        CTransform = gameObject.GetComponent<Transform>();
        CRigidb = gameObject.AddComponent<Rigidbody2D>();
        CBoxColl = gameObject.AddComponent<BoxCollider2D>();
        Cam = gameObject.GetComponent<Camera>();
        CBoxColl.transform.localScale = new Vector2(Cam.aspect * 2.0f * Cam.orthographicSize, 2.0f * Cam.orthographicSize);
        CBoxColl.isTrigger = true;
        CRigidb.isKinematic = true;
        CamPos.z = GM.ZCamera;
        CRigidb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
    }
	
	// Update is called once per frame
	void Update () {
        if (((PControl != null) && (PControl.PTransform != null)) && (locked == false))
        {
            CamPos.x = PControl.PRigidb.velocity.x;
            if (lockedvert)
                CamPos.y = 0;
            CRigidb.velocity = CamPos;
        }
        if (((PControl != null) && (PControl.PTransform != null)) && (locked == true))
        {
            Vector3 newCamPos = CTransform.position;
            for (int i = 0; i < forbids.Count; i++)
            {
                switch (forbids[i].objSave.modifiers["forbid"])
                {
                    case "left":
                        if (PControl.PTransform.position.x - (Cam.aspect * Cam.orthographicSize) < forbids[i].transform.position.x + forbids[i].GetComponent<SpriteRenderer>().size.x / 2)
                            newCamPos.x = forbids[i].transform.position.x + forbids[i].GetComponent<SpriteRenderer>().size.x / 2 + (Cam.aspect * Cam.orthographicSize);
                        else
                            forbids.Remove(forbids[i]);
                        break;
                    case "right":
                        if (PControl.PTransform.position.x + (Cam.aspect * Cam.orthographicSize) > forbids[i].transform.position.x - forbids[i].GetComponent<SpriteRenderer>().size.x / 2)
                            newCamPos.x = forbids[i].transform.position.x - forbids[i].GetComponent<SpriteRenderer>().size.x / 2 - (Cam.aspect * Cam.orthographicSize);
                        else
                            forbids.Remove(forbids[i]);
                        break;
                    default:
                        Debug.Log("wrong forbidden camera movement");
                        break;
                }
            }
            CTransform.position = newCamPos;
            if (forbids.Count == 0)
                locked = false;
        }
        /*if (((PControl != null) && (PControl.PTransform != null)) && (lockedvert == true))
        {
            Vector3 newCamPos = CTransform.position;
            for (int i = 0; i < forbidsvert.Count; i++)
            {
                switch (forbidsvert[i].objSave.modifiers["forbid"])
                {
                    case "up":
                        break;
                    case "down":
                        if (CTransform.position.y < (PControl.PTransform.position.y + (PControl.PSpriteRend.size.y / 2) + (GM.map.tilesizey / 200)))
                        {
                            newCamPos.y = PControl.PTransform.position.y + (PControl.PSpriteRend.size.y / 2) + (GM.map.tilesizey / 200);
                        }
                        else
                        {
                            Debug.Log("unlocked" + forbidsvert[i].objSave.modifiers["lockid"] + " / " + forbidsvert[i].objSave.modifiers["forbid"]);
                            forbidsvert.Remove(forbidsvert[i]);
                        }
                        break;
                    default:
                        Debug.Log("wrong forbidden camera movement");
                        break;
                }
            }
            CTransform.position = newCamPos;
            if (forbidsvert.Count == 0)
            {
                Debug.Log("unlocked cam");
                lockedvert = false;
            }
        }*/
    }

    public void LockCam(ObjectController camlock)
    {
        if ((camlock.objSave.modifiers["forbid"] == "right") || (camlock.objSave.modifiers["forbid"] == "left"))
        {
            lockid = camlock.objSave.modifiers["lockid"];
            forbids.Add(camlock);
            CRigidb.velocity = Vector3.zero;
            locked = true;
        }
      /*  else
        {
            lockidvert = camlock.objSave.modifiers["lockid"];
            forbidsvert.Add(camlock);
            CRigidb.velocity = Vector3.zero;
            lockedvert = true;
        }*/
        //Debug.Log("locked" + camlock.objSave.modifiers["lockid"] + " / " + camlock.objSave.modifiers["forbid"]);
    }

    public void UnlockCam(ObjectController camunlock)
    {
        if ((locked == true) && (lockid == camunlock.objSave.modifiers["unlockid"]))
        {
            lockid = "";
            forbids.Remove(forbids.Find(obj => obj.objSave.modifiers["lockid"] == camunlock.objSave.modifiers["unlockid"]));
            locked = false;
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "camlock")
             LockCam(coll.GetComponent<ObjectController>());
    }
    public void ReplaceCam(GameObject obj, PlayerController PContr)
    {
        CamPos = new Vector3(obj.transform.position.x, PContr.PTransform.position.y + (PContr.PSpriteRend.size.y / 2) + (GM.map.tilesizey / 200), GM.ZCamera);
        CTransform.position = CamPos;
        PControl = PContr;
    }
}
