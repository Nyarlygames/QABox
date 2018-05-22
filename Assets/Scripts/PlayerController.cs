using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public SpriteRenderer PSpriteRend;
    public Transform PTransform;
    BoxCollider2D PBoxCollider;
    public Rigidbody2D PRigidb;
    public float speed = 10.0f;
    public float speedjump = 1200.0f;
    public float gravity = 5.0f;
    public bool grounded = false;
    GameManager GM;
    Quaternion RotLeft;
    Quaternion RotRight;
    int direction = 0; // 1 left 2 right 0 none

    void Awake()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        PSpriteRend = gameObject.AddComponent<SpriteRenderer>();
        PSpriteRend.sprite = GM.SpriteList["Player"];
        PTransform = gameObject.GetComponent<Transform>();
        PBoxCollider = gameObject.AddComponent<BoxCollider2D>();
        PRigidb = gameObject.AddComponent<Rigidbody2D>();
        PRigidb.constraints = RigidbodyConstraints2D.FreezeRotation;
        PRigidb.gravityScale = gravity;
        RotLeft = Quaternion.Euler(new Vector3(0, 180, 0));
        RotRight = Quaternion.Euler(Vector3.zero);
    }

    void Update()
    {
        // set movement
        if (Input.GetKeyDown(KeyCode.LeftArrow) && (direction == 0) && (grounded == true))
        {
            direction = 1;
            if (PTransform.eulerAngles.y == 0)
                PTransform.rotation = RotLeft;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && (direction == 0) && (grounded == true))
        {
            direction = 2;
            if (PTransform.eulerAngles.y == 180)
                PTransform.rotation = RotRight;
        }

        // moves
        if (Input.GetKey(KeyCode.LeftArrow) && (direction == 1) && (grounded == true))
            PRigidb.velocity = Vector2.left * speed;
        if (Input.GetKey(KeyCode.RightArrow) && (direction == 2) && (grounded == true))
            PRigidb.velocity = Vector2.right * speed;

        // reset movement and check for double inputs
        if (Input.GetKeyUp(KeyCode.LeftArrow) && (direction == 1) && (grounded == true))
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                direction = 2;
                PTransform.rotation = RotRight;
            }
            else
                direction = 0;
            PRigidb.velocity = Vector2.zero;
        }
        if (Input.GetKeyUp(KeyCode.RightArrow) && (direction == 2) && (grounded == true))
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                direction = 1;
                PTransform.rotation = RotLeft;
            }
            else
                direction = 0;
            PRigidb.velocity = Vector2.zero;
        }

        // jumps
        if (Input.GetKey(KeyCode.UpArrow) && (grounded == true))
        {
            grounded = false;
            PRigidb.AddForce(Vector2.up * speedjump, ForceMode2D.Force);
        }
    }

    public void SetPosition(Transform spawnerPos, SpriteRenderer spawnerSprite)
    {
        PTransform.position = new Vector3(spawnerPos.position.x + ((PSpriteRend.size.x / 2) - (spawnerSprite.size.x / 2)), spawnerPos.position.y + ((PSpriteRend.size.y / 2) - (spawnerSprite.size.y / 2)), GM.ZPlayer);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.tag == "tile")
        {
            if (grounded == false)
            {
                grounded = true;
                if ((direction == 1) && (Input.GetKey(KeyCode.RightArrow)) && (!Input.GetKey(KeyCode.LeftArrow)))
                {
                    direction = 2;
                    PTransform.rotation = RotRight;
                }
                else if ((direction == 2) && (!Input.GetKey(KeyCode.RightArrow)) && (Input.GetKey(KeyCode.LeftArrow)))
                {
                    direction = 1;
                    PTransform.rotation = RotLeft;
                }
                else if ((!Input.GetKey(KeyCode.LeftArrow)) && (!Input.GetKey(KeyCode.RightArrow)))
                    direction = 0;
                PRigidb.velocity = Vector2.zero;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "camunlock")
            GM.Camera.UnlockCam(coll.GetComponent<ObjectController>());
    }
}