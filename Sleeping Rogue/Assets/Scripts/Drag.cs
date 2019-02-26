using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{

    public GameObject forward1, forward2;

    public GameObject player;

    public static bool boxDrag;
    public static RaycastHit2D boxTouch;

    bool dragging = false;

    public GameObject dragged;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] checks = GameObject.FindGameObjectsWithTag("WallCheck");

        forward1 = checks[0];
        forward2 = checks[1];

        player = GameObject.Find("Player");
       this.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        boxTouch = Physics2D.Linecast(forward1.transform.position, forward2.transform.position, LayerMask.GetMask("Box"));


        if (boxTouch && player.GetComponent<PlatformerController>().grounded && !dragging && Input.GetAxis("Drag") != 0)
        {
            dragging = true;
            boxDrag = true;
            dragged = boxTouch.collider.gameObject;
            dragged.GetComponent<FixedJoint2D>().connectedBody = player.GetComponent<Rigidbody2D>();
            dragged.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        else if (!boxDrag && dragging)
        {
            dragging = false;

            dragged.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
            dragged.GetComponent<FixedJoint2D>().connectedBody = dragged.GetComponent<Rigidbody2D>();
            dragged = null;
        }

        if (!boxTouch)
        {
            boxDrag = false;

        }
        Debug.Log(boxTouch.collider.gameObject);

    }

}
