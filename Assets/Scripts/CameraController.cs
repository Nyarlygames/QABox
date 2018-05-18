using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    GameManager GM;
    Vector3 CamPos = Vector3.zero;
    PlayerController PControl;
    Transform CamControl;
	// Use this for initialization
	void Start ()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        CamControl = gameObject.GetComponent<Transform>();
        CamPos.z = GM.ZCamera;
    }
	
	// Update is called once per frame
	void Update () {
        if ((PControl != null) && (PControl.PTransform != null))
        {
            CamPos.x = PControl.PTransform.position.x;
            //CamPos.y = PControl.PTransform.position.y;
            CamControl.position = CamPos;
        }
	}

    public void ReplaceCam(GameObject obj, PlayerController PContr)
    {
        CamPos = new Vector3(obj.transform.position.x, obj.transform.position.y, GM.ZCamera);
        CamControl.position = CamPos;
        PControl = PContr;
    }
}
