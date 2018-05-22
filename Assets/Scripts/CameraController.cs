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
    string lockid = "";
    List<ObjectController> forbids = new List<ObjectController>();

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
    }
	
	// Update is called once per frame
	void Update () {
        if (((PControl != null) && (PControl.PTransform != null)) && (locked == false))
        {
            CamPos = PControl.PRigidb.velocity;
            CRigidb.velocity = CamPos;
        }
        else if (((PControl != null) && (PControl.PTransform != null)) && (locked == true))
        {
            for (int i=0; i < forbids.Count; i++)
            {
                switch (forbids[i].objSave.modifiers["forbid"])
                {
                    case "left":
                        if (PControl.PTransform.position.x - (Cam.aspect * Cam.orthographicSize) < forbids[i].transform.position.x + forbids[i].GetComponent<SpriteRenderer>().size.x / 2)
                        {
                            Vector3 minPos = new Vector3(forbids[i].transform.position.x + forbids[i].GetComponent<SpriteRenderer>().size.x / 2 + (Cam.aspect * Cam.orthographicSize), PControl.PTransform.position.y, GM.ZCamera);
                            CTransform.position = minPos;
                        }
                        else
                        {
                            forbids.Remove(forbids[i]);
                            locked = false;
                        }
                        break;
                    case "right":
                        if (PControl.PTransform.position.x + (Cam.aspect * Cam.orthographicSize) > forbids[i].transform.position.x - forbids[i].GetComponent<SpriteRenderer>().size.x / 2)
                        {
                            Vector3 maxPos = new Vector3(forbids[i].transform.position.x - forbids[i].GetComponent<SpriteRenderer>().size.x / 2 - (Cam.aspect * Cam.orthographicSize), PControl.PTransform.position.y, GM.ZCamera);
                            CTransform.position = maxPos;
                        }
                        else
                        {
                            forbids.Remove(forbids[i]);
                            locked = false;
                        }
                        break;
                    case "up":
                        break;
                    case "down":
                        break;
                    default:
                        Debug.Log("wrong forbidden camera movement");
                        break;
                }
            }
        }
	}

    public void LockCam(ObjectController camlock)
    {
        lockid = camlock.objSave.modifiers["lockid"];
        CRigidb.velocity = Vector3.zero;
        Debug.Log("locked" + camlock.objSave.modifiers["lockid"] + " / " + camlock.objSave.modifiers["forbid"]);
        forbids.Add(camlock);
        locked = true;
    }
    public void UnlockCam(ObjectController camunlock)
    {
        if ((locked == true) && (lockid == camunlock.objSave.modifiers["unlockid"]))
        {
            lockid = "";
            Debug.Log("unlocked" + forbids.Find(obj => obj.objSave.modifiers["lockid"] == camunlock.objSave.modifiers["unlockid"]));
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
        CamPos = new Vector3(obj.transform.position.x, obj.transform.position.y, GM.ZCamera);
        CTransform.position = CamPos;
        PControl = PContr;
    }
}
