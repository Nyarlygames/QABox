using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    GameManager GM;
	// Use this for initialization
	void Start ()
    {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ReplaceCam(GameObject obj)
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameObject.GetComponent<Transform>().position = new Vector3(obj.transform.position.x, obj.transform.position.y, GM.ZCamera);
    }
}
