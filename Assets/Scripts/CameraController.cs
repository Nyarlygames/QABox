using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    GameManager GM;
	// Use this for initialization
	void Start ()
    {
        Debug.Log("CAMSTART");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ReplaceCam(GameObject obj)
    {
        Debug.Log("CAMSTART2");
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameObject.GetComponent<Transform>().position = new Vector3(obj.transform.position.x, obj.transform.position.y, GM.ZCamera);
    }
}
